using System.Text.Json.Serialization;

namespace TestTelcoHub.Model.DTOs
{
    public class PlanDTO
    {
        [JsonPropertyName("BillingPlan")]
        public string BillingPlan { get; set; } = string.Empty;
        [JsonPropertyName("Sku")]
        public string Sku { get; set; } = string.Empty;
        [JsonPropertyName("Quantity")]
        public int Quantity { get; set; }
        [JsonPropertyName("Expiration")]
        [JsonIgnore]
        public ExpirationDTO? ExpirationDTO { get; set; }
        [JsonPropertyName("Customer")]
        public CustomerDTO? Customer { get; set; }
        [JsonPropertyName("ApprovalCode")]
        public string ApprovalCode { get; set; } = string.Empty;
        [JsonPropertyName("DeliveryEmail")]
        public string DeliveryEmail { get; set; } = string.Empty;
        [JsonPropertyName("TermsAndConditions")]
        public TermsAndConditionsDTO? TermsAndConditions { get; set; }
        [JsonPropertyName("Comment")]
        public string Comment { get; set; } = string.Empty;
    }

}
