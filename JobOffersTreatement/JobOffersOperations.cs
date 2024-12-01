using System.Net.Http.Headers;
using System.Text.Json;
using JobOffersTreatement.Contract;
using JobOffersTreatement.DataAccess;
using JobOffersTreatement.Entities;

namespace JobOffersTreatement
{
    public class JobOffersOperations : IJobOffersOperations
    {
        private readonly JobOfferRepository _jobOfferRepository;

        private static readonly HttpClient client = new();
        internal const string URL = $"https://api.francetravail.io/partenaire/offresdemploi/v2/offres/search?commune={{0}}"; //&offres150-300/150

        public JobOffersOperations(JobOfferRepository jobOfferRepository)
        {
            _jobOfferRepository = jobOfferRepository;
        }

        private static async Task<string> GetToken()
        {
            string clientId = "PAR_testgetjobs_bbf4c4db311783078848841ea13608af459c1caa14b28732697dc889a9d9fb73";
            string clientSecret = "7988f768d0dbbe289cac7e6b2c060010843012cedb22edabd3085cbde1e3b7ef";
            string tokenUrl = "https://entreprise.francetravail.fr/connexion/oauth2//access_token?realm=%2Fpartenaire";
            var token = await GetOAuthTokenAsync(clientId, clientSecret, tokenUrl);

            if (token != null && !string.IsNullOrEmpty(token.AccessToken))
            {
                Console.WriteLine($"Token: {token.AccessToken}");
                return token.AccessToken;
            }
            return string.Empty;
        }

        private static async Task<TokenResponse?> GetOAuthTokenAsync(string clientId, string clientSecret, string tokenUrl)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("scope", "api_offresdemploiv2 o2dsoffre")
            });

            var response = await client.PostAsync(tokenUrl, formContent);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TokenResponse>(json);
            }

            Console.WriteLine($"Erreur: {response.StatusCode}");
            return null;
        }

        public async Task<List<JobOffer>> CallSearchJobs()
        {
            string token = await GetToken();

            List<JobOffer> offers = new();
            //List<string> cities = new() { "Rennes", "Bordeaux", "Paris" };
            //communes rennes: 35238
            //communes Bordeaux: 33063
            //communes Paris: 75056 => 75101 - 75120
            List<string> communes = new() { "35238", "33063", "75101" }; //, "75102", "75103", "75104", "75105",
            //"75106", "75107", "75108", "75109", "75110", "75111", "75112", "75113", "75114", "75115",
            //"75116", "75117", "75118", "75119", "75120"};            

            foreach (var city in communes)
            {
                var cityOffers = await SearchJobOffersAsync(city, token);
                offers.AddRange(cityOffers);
            }
            return offers;
        }

        private static async Task<List<JobOffer>> SearchJobOffersAsync(string city, string token)
        {
            List<JobOffer> jobOffersList = new();
            var url = string.Format(URL, city);

            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url)
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                var jobOffersDto = JsonSerializer.Deserialize<ResultDto>(result);

                FillJobOffersList(jobOffersList, jobOffersDto);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unexpected error: {e.Message}");
            }
            return jobOffersList;
        }

        private static void FillJobOffersList(List<JobOffer> jobOffers, ResultDto? jobOffersList)
        {
            if (jobOffersList != null && jobOffersList.JobOfferDtos.Any())
            {
                foreach (var offer in jobOffersList.JobOfferDtos)
                {
                    jobOffers.Add(new JobOffer
                    {
                        Title = offer.Intitule,
                        Description = offer.Description,
                        Url = offer.OrigineOffre?.UrlOrigine ?? string.Empty,
                        City = offer.LieuTravail?.Commune ?? string.Empty,
                        ContractType = offer.TypeContrat,
                        Company = offer.Entreprise?.Nom ?? string.Empty,
                        Country = offer.PaysContinent,
                        IdFromSite = offer.Id
                    });
                }
            }
        }

        public void InsertDataToBase(List<JobOffer> offers)
        {
            _jobOfferRepository.InsertJobOffersToBase(offers);
        }

        public void WriteStatistic()
        {
            _jobOfferRepository.WriteStatisticIntoConsole();
        }
    }
}
