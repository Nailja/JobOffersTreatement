using System.Text.Json.Serialization;

namespace JobOffersTreatement.Entities
{
    public class ResultDto
    {
        [JsonPropertyName("resultats")]
        public List<JobOfferDto> JobOfferDtos { get; set; }
    }
}
