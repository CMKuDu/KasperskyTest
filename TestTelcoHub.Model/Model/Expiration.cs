using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestTelcoHub.Model.Model
{

    public class Expiration
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        [JsonPropertyName("MomentType")]
        [NotMapped]
        public MonmentType MomentType { get; set; }
        [JsonPropertyName("ExactMoment")]
        public DateTime? ExactMoment { get; set; }
        [JsonPropertyName("AfterMoment")]
        public DateTime? AfterMoment { get; set; }
        [JsonPropertyName("PeriodCount")]
        public int PeriodCount { get; set; }
    }
}
