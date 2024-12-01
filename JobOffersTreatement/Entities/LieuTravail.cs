using System.Text.Json.Serialization;

namespace JobOffersTreatement.Entities
{
    public class LieuTravail
    {
        [JsonPropertyName("libelle")]
        public string? Libelle { get; set; }
        [JsonPropertyName("codePostal")]
        public string? CodePostal { get; set; }
        [JsonPropertyName("commune")]
        public string? Commune { get; set; }
    }
}
