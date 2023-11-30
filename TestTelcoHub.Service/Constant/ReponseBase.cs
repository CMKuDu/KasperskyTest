using TestTelcoHub.Model.Prototype;

namespace TestTelcoHub.Constant
{
    public class ReponseBase
    {
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();
        public Dictionary<string, List<string>> ValidationErrors { get; set; } = new Dictionary<string, List<string>>();
        public ExpirationInfo? ExpirationInfo { get; set; }
    }
}
