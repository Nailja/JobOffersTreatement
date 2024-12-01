using System.Text.Json.Serialization;

namespace JobOffersTreatement.Entities
{
    public class Entreprise
    {
        [JsonPropertyName("nom")]
        public string? Nom { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("logo")]
        public string? Logo { get; set; }
        [JsonPropertyName("url")]
        public string? Url { get; set; }
        [JsonPropertyName("entrepriseAdaptee")]
        public bool EntrepriseAdaptee { get; set; }
    }
}
