using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TestTelcoHub.Model.Model;

namespace TestTelcoHub.Model.DTOs
{
    public class ExpirationDTO
    {

        [JsonPropertyName("MomentType")]
        [NotMapped]
        public MonmentTypeDTO MomentTypeDTO { get; set; }
        public DateTime? ExactMoment { get; set; }
        public DateTime? AfterMoment { get; set; }
        public int? PeriodCount { get; set; }
    }
}
