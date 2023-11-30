using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestTelcoHub.Model.DTOs
{
    public class DistributorDTO
    {
        [JsonPropertyName("Partner")]
        public string Partner { get; set; } = string.Empty;
        [JsonPropertyName("Reseller")]
        public string Reseller { get; set; } = string.Empty;
    }

}
