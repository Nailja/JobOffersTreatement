using JobOffersTreatement.Entities;
using Microsoft.Data.Sqlite;

namespace JobOffersTreatement.Mapper
{
    public static class Mapper
    {
        public static JobOffer ToJobOffer(SqliteDataReader reader) => new()
        {
            ContractType = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
            Company = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
            Country = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
            Statistic = reader.GetInt32(3)
        };
    }
}
