using JobOffersTreatement.Entities;

namespace JobOffersTreatement.Contract
{
    public interface IJobOfferRepository
    {
        void InsertJobOffersToBase(List<JobOffer> offers);

        void WriteStatisticIntoConsole();
    }
}
