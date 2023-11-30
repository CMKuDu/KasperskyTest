using System.Text.Json.Serialization;

namespace TestTelcoHub.Model.Model
{
    public class Customer
    {
        [JsonIgnore]
        [JsonPropertyName("CustomerId")]
        public Guid CustomerId { get; set; } = Guid.NewGuid();
        [JsonPropertyName("Contacts")]
        public Contacts? Contacts { get; set; }
        [JsonPropertyName("Address")]
        public Address? Address { get; set; }
    }
}
