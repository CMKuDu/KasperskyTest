using System.ComponentModel.DataAnnotations;

namespace TestTelcoHub.Model.Prototype
{
    
    public class ResposeResult
    {
        [MaxLength(50)]
        public string SubscriptionId { get; set; } = string.Empty;
        public string LicenseId { get; set; } = string.Empty;
        public string ActivationCode { get; set; } = string.Empty;
        public object ErrorMessage { get; set; } = string.Empty;
        public bool Success { get; set; }
    }
}
