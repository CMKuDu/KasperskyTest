using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestTelcoHub.Model.DTOs
{
    public class AddressDTO
    {
        [JsonPropertyName("Country")]
        public string Country { get; set; } = string.Empty;
    }

}
