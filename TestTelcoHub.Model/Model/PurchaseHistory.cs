using System.Text.Json.Serialization;

namespace TestTelcoHub.Model.Model
{
    public class PurchaseHistory
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        [JsonPropertyName("ProductName")]
        public string ProductName { get; set; } = string.Empty;
        [JsonIgnore]
        [JsonPropertyName("StatusSubscription")]
        public SubscriptionStatus StatusSubscription { get; set; }
        [JsonPropertyName("SubscriptionStatus")]
        public string StatusSubscriptionString => StatusSubscription.ToString();
        [JsonPropertyName("Nodes")]
        public int Nodes { get; set; }
        [JsonPropertyName("CompanyName")]
        public string CompanyName { get; set; } = string.Empty;
        [JsonPropertyName("Country")]
        public string Country { get; set; } = string.Empty;
        [JsonPropertyName("DeliveryEmail")]
        public string DeliveryEmail { get; set; } = string.Empty;
        [JsonPropertyName("CreateDate")]
        public string CreateDate { get; set; } = string.Empty;
        [JsonPropertyName("ExactMoment")]
        public DateTime? ExactMoment { get; set; }

        [JsonPropertyName("AfterMoment")]
        public DateTime? AfterMoment { get; set; }
        [JsonIgnore]
        public PeriordType PeriordType { get; set; }
        [JsonPropertyName("PeriordType")]
        public string PeriordTypeString => PeriordType.ToString();

        [JsonPropertyName("PeriodCount")]
        [JsonIgnore]
        public int PeriodCount { get; set; }
        //
        [JsonPropertyName("SubscriptionId")]
        public string SubscriptionId { get; set; } = string.Empty;
        [JsonPropertyName("ActivationCode")]
        public string ActivationCode { get; set; } = string.Empty;
        [JsonPropertyName("LicenseId")]
        public string LicenseId { get; set; } = string.Empty;
        public string AspNetUserId { get; set; } = string.Empty;
    }
    public enum SubscriptionStatus
    {
        Active,
        HardCancel,
        PendingCancellation,
        Expired,
    }
    public enum PeriordType
    {
        Free,
        Paid,
    }
}
