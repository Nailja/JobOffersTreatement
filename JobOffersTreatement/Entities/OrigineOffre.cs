using System.Text.Json.Serialization;
namespace JobOffersTreatement.Entities
{
    public class OrigineOffre
    {
        [JsonPropertyName("urlOrigine")]
        public string? UrlOrigine { get; set; }
    }
}
