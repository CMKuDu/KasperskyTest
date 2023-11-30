using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TestTelcoHub.Model.Model
{
    public class ChangeLog
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string newValue { get; set; } = string.Empty;
        public string oldValue { get; set; } = string.Empty;
        public string CreateLog { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        [NotMapped]
        [JsonIgnore]
        public string? Data { get; set; } = string.Empty;
        public string? DataJson
        {
            get => Data;
            set => Data = value != null ? JsonSerializer.Deserialize<string>(value) : Data;
        }
        public string SubscriptionId { get; set; } = string.Empty;
        public string AspNetUserId { get; set; } = string.Empty;
    }
}
