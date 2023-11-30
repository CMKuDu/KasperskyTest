using System.Text.Json.Serialization;
using TestTelcoHub.Model.Model;

namespace TestTelcoHub.Model.ViewModel
{
    public class ModifyExpirationViewModel
    {
        [JsonPropertyName("SubscriptionId")]
        public string SubscriptionId { get; set; } = string.Empty;
        [JsonPropertyName("Expiration")]
        public Expiration? Expiration { get; set; }
    }
}
