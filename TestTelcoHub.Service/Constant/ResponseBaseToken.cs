namespace TestTelcoHub.Service.Constant
{
    public class ResponseBaseToken
    {
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();
        public string Token { get; internal set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
