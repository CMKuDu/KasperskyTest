using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestTelcoHub.Model.DTOs
{
    public class ContactsDTO
    {

        [JsonPropertyName("CompanyName")]
        public string CompanyName { get; set; } = string.Empty;
    }

}
