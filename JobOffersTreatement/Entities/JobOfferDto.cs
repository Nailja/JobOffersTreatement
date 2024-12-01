using System.Text.Json.Serialization;

namespace JobOffersTreatement.Entities
{
    public class JobOfferDto
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("intitule")]
        public string? Intitule { get; set; }
        [JsonPropertyName("dateCreation")]
        public DateTime DateCreation { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("lieuTravail")]
        public LieuTravail? LieuTravail { get; set; }
        [JsonPropertyName("entreprise")]
        public Entreprise? Entreprise { get; set; }
        [JsonPropertyName("typeContrat")]
        public string? TypeContrat { get; set; }
        [JsonPropertyName("origineOffre")]
        public OrigineOffre? OrigineOffre { get; set; }
        [JsonPropertyName("paysContinent")]
        public string? PaysContinent { get; set; } = "France";
    }
}
