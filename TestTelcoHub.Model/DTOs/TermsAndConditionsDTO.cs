using System.Text.Json.Serialization;
using TestTelcoHub.Model.Model;

namespace TestTelcoHub.Model.DTOs
{
    public class TermsAndConditionsDTO
    {
        [JsonPropertyName("CustomerAgreements")]
        public List<CustomerAgreementsDTO>? CustomerAgreements { get; set; }
    }

}
