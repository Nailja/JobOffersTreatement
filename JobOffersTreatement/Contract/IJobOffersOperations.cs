using JobOffersTreatement.Entities;

namespace JobOffersTreatement.Contract
{
    public interface IJobOffersOperations
    {
        Task<List<JobOffer>> CallSearchJobs();

        void InsertDataToBase(List<JobOffer> offers);

        void WriteStatistic();
    }
}
