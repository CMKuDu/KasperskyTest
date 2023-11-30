using System.Text.Json.Serialization;

namespace TestTelcoHub.Model.ViewModel
{
    public class ModifyQuantityViewModel
    {
        [JsonPropertyName("SubscriptionId")]
        public string? SubscriptionId { get; set; }
        [JsonPropertyName("Quantity")]
        public int Quantity { get; set; }
    }
}
    