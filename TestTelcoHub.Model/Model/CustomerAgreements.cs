using System.Text.Json.Serialization;

namespace TestTelcoHub.Model.Model
{
    public class CustomerAgreements
    {
        [JsonIgnore]
        [JsonPropertyName("CustomerAgreementsId")]
        public Guid CustomerAgreementsId { get; set; } = Guid.NewGuid(); // Cờ (flag) đòng ý thỏa thuận từ Customer hay khồng
        [JsonPropertyName("AgreementAccepted")]
        public bool AgreementAccepted { get; set; } // Đồng ý hoặc không 
        [JsonPropertyName("AgreementText")]
        public string AgreementText { get; set; } = string.Empty;// thỏa thuận văn bản
        [JsonPropertyName("AgreementTextHash")]
        public string AgreementTextHash { get; set; } = string.Empty;// băm thỏa thuận văn bản
    }
}
