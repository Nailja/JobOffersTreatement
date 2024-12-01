using JobOffersTreatement;
using JobOffersTreatement.Contract;
using JobOffersTreatement.DataAccess;
using JobOffersTreatement.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = new HostBuilder();

JobOfferRepository jobOfferRepository = new();

builder.ConfigureServices((context, services) =>
{
    services.AddScoped<IJobOffersOperations>(x => new JobOffersOperations(jobOfferRepository));
});

IHost host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var testOperations = services.GetRequiredService<IJobOffersOperations>();
    List<JobOffer> offers = testOperations.CallSearchJobs().Result;
    testOperations.InsertDataToBase(offers);
    testOperations.WriteStatistic();
}

Console.WriteLine("Data saved and report generated successfully!");





