using System.Text.Json.Serialization;

namespace TestTelcoHub.Model.Prototype
{
    public class ResponseContants
    {
        public const string Success = "success";
        public const string Error = "Error";
    }
    public class ProductConst 
    {
        public const string ProductExits = "Cant find Subscription";
        public const string ProductCantNull = "Cant null here";
    }

    public class CreateConst
    {
        public const string CreatedSuccessfully = "Create Subscription Successfully!";
        public const string CreateFail = "Create Subscription Fail";
    }
    public class ModifyConst
    {
        public const string ModifySuccess = "Modify Successfully";
        public const string Modifyfail = "Modify Failed";
        public const string ModifyQuantityError = "Chinh sua so nodes that bai";
        public const string ModifyExpirationError = "Thời gian chỉnh sửa không đúng";

    }
    public class CancleConst 
    {
        public const string CacnleSuccessfully = "Cancle Successfully";
        public const string CancleFail = "Cancle Failed";
    }
    public class  RenewConst
    {
        public const string RenewSuccessfully = "Renew Successfully";
        public const string RenewFail = "Renew Failed";
    }
    public class ErrorResponseModel
    {
        [JsonPropertyName("ErrorId")]
        public Guid ErrorId { get; set; }

        [JsonPropertyName("Message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("ErrorCode")]
        public string ErrorCode { get; set; } = string.Empty;
    }
    public class ExpirationInfo
    {
        public DateTime ExactMoment { get; set; }
        public DateTime AfterMoment { get; set; }
    }
}
