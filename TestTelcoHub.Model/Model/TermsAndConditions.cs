using System.Text.Json.Serialization;

namespace TestTelcoHub.Model.Model
{
    public class TermsAndConditions
    {

        [JsonPropertyName("TermsAndConditionsId")]
        [JsonIgnore]
        public Guid TermsAndConditionsId { get; set; } = Guid.NewGuid();
        public List<CustomerAgreements>? CustomerAgreements { get; set; } = new List<CustomerAgreements>();

    }
}
    