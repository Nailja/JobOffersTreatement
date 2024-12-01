using JobOffersTreatement.Contract;
using JobOffersTreatement.Entities;
using Microsoft.Data.Sqlite;

namespace JobOffersTreatement.DataAccess
{
    public class JobOfferRepository : IJobOfferRepository
    {
        public JobOfferRepository() { }

        private static SqliteConnection GetConnection()
        {
            return new SqliteConnection("Data Source = C:\\Users\\CE_NAILYA\\source\\repos\\JobOffers\\JobOffersTreatement\\DataBase\\JobsOffers.sqlite");
        }

        public void InsertJobOffersToBase(List<JobOffer> offers)
        {
            try
            {
                using SqliteConnection connection = GetConnection();
                connection.Open();

                foreach (var offer in offers)
                {
                    try
                    {
                        SqliteCommand insertCommand = new(Requests.InsertJobOffers, connection);
                        SetParameters(insertCommand, offer);
                        insertCommand.ExecuteNonQuery();
                    }
                    catch (SqliteException ex)
                    {
                        // Afficher le code d'erreur et le message détaillé
                        Console.WriteLine($"SQLite Error Code: {ex.SqliteErrorCode}");
                        Console.WriteLine($"SQLite Extended Error Code: {ex.SqliteExtendedErrorCode}");
                        Console.WriteLine($"SQLite Error Message: {ex.Message}");

                        // Gestion de l'erreur de contrainte unique (code d'erreur 19)
                        if (ex.SqliteErrorCode == 19)
                        {
                            Console.WriteLine("A job offer with the same IdFromSite and Url already exists.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"SQLite Error: {ex.Message}");
                    }
                }
                connection.Close();
            }            
            catch (Exception ex)
            {
                Console.WriteLine($"SQLite Error: {ex.Message}");
            }
        }

        private static void SetParameters(SqliteCommand insertCommand, JobOffer offer)
        {
            insertCommand.Parameters.Clear();
            insertCommand.Parameters.AddWithValue("@title", offer.Title);
            insertCommand.Parameters.AddWithValue("@description", offer.Description);
            insertCommand.Parameters.AddWithValue("@url", offer.Url);
            insertCommand.Parameters.AddWithValue("@city", offer.City);
            insertCommand.Parameters.AddWithValue("@contractType", offer.ContractType);
            insertCommand.Parameters.AddWithValue("@company", offer.Company);
            insertCommand.Parameters.AddWithValue("@country", offer.Country);
            insertCommand.Parameters.AddWithValue("@idFromSite", offer.IdFromSite);
        }

        public void WriteStatisticIntoConsole()
        {
            using SqliteConnection connection = GetConnection();
            connection.Open();
            SqliteCommand getCommand = new(Requests.SelectJobOffers, connection);
            SqliteDataReader reader = getCommand.ExecuteReader();
            while (reader.Read())
            {
                JobOffer offer = Mapper.Mapper.ToJobOffer(reader);
                Console.WriteLine($"{offer.ContractType} | {offer.Company} | {offer.Country} | {offer.Statistic}");
            }
            connection.Close();
        }
    }
}
