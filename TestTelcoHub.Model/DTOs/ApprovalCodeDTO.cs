using System.Text.Json.Serialization;

namespace TestTelcoHub.Model.DTOs
{
    public class ApprovalCodeDTO
    {
        public string Code { get; set; } = string.Empty;
        [JsonIgnore]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime ExpiredDate { get; set; } // thời hạn mã giả giá kết thuc.
        public int Discount { get; set; }
    }
}
 