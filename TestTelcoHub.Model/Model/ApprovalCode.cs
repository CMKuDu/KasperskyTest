using System.ComponentModel.DataAnnotations.Schema;

namespace TestTelcoHub.Model.Model
{
    public class ApprovalCode
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public ApprovalCodeStatus Status { get; set; }
        [NotMapped]
        public string ApprovalCodeString => Status.ToString();
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime ExpiredDate { get; set; } // thời hạn mã giả giá kết thuc.
        public int Discount { get; set; }
    }
    public enum ApprovalCodeStatus 
    {
        Active,
        Expired,
    }
}
