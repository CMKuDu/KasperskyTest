using System.Text.Json.Serialization;

namespace TestTelcoHub.Model.Model
{
    public class ExternalReference
    {
        [JsonIgnore]
        [JsonPropertyName("ExternalReferenceId")]
        public Guid ExternalReferenceId { get; set; } = Guid.NewGuid();
        public string ExternalSubscriptionId { get; set; } = string.Empty;// Mã định danh đối tác
        public string ExternalOrderId { get; set; } = string.Empty; // Mã đơn hàng trong Distributor
        public string ExternalLineItemId { get; set; } = string.Empty; // Mã positon trong hệ thôngs của Distributor
    }
}
