using System.Text.Json.Serialization;

namespace TestTelcoHub.Model.DTOs
{
    public class CustomerAgreementsDTO
    {
       
        [JsonPropertyName("AgreementAccepted")]
        public bool AgreementAccepted { get; set; }
        [JsonPropertyName("AgreementText")]
        public string AgreementText { get; set; } = string.Empty;
        [JsonPropertyName("AgreementTextHash")]
        public string AgreementTextHash { get; set; } = string.Empty;
    }

}
