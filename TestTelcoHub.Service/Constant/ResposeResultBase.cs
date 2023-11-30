namespace TestTelcoHub.Service.Constant
{
    public class ResposeResultBase : CustomerResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
    public class CustomerResponse
    {
        public string CreateDate { get; set; } = string.Empty;
        public DateTime ExactMoment { get; set; }
        public DateTime AfterMoment { get; set; }
    }
}
