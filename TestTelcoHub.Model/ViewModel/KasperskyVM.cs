using System.Text.Json.Serialization;
using TestTelcoHub.Model.DTOs;

namespace TestTelcoHub.Model.ViewModel
{
    public class KasperskyVM
    {
        [JsonPropertyName("BillingPlan")]
        public string BillingPlan { get; set; } = string.Empty;

        [JsonPropertyName("Sku")]
        public string Sku { get; set; } = string.Empty;

        [JsonPropertyName("Quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("Expiration")]
        public ExpirationDTO? Expiration { get; set; } // Dán cứng Expiration

        [JsonPropertyName("Customer")]
        public CustomerDTO? Customer { get; set; }

        [JsonPropertyName("Distributor")]
        public DistributorDTO Distributor { get; set; } = new DistributorDTO // Dán cứng Distributor
        {
            Partner = "NTS_ICT_Co",
            Reseller = ""
        };

        [JsonPropertyName("approvalCode")]
        public string ApprovalCode { get; set; } = string.Empty;

        [JsonPropertyName("DeliveryEmail")]
        public string DeliveryEmail { get; set; } = string.Empty;

        [JsonPropertyName("TermsAndConditions")]
        public TermsAndConditionsDTO? TermsAndConditions { get; set; }

        [JsonPropertyName("Comment")]
        public string Comment { get; set; } = string.Empty;

        // Các property khác mà bạn muốn bổ sung cho KasperskyVM
    }
}
