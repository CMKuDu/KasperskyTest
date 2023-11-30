using FluentValidation;
using System.Text.Json.Serialization;

namespace TestTelcoHub.Model.Model
{
    public class Distributor
    {
        [JsonIgnore]
        [JsonPropertyName("DistributorId")]
        public Guid DistributorId { get; set; } = Guid.NewGuid();
        [JsonPropertyName("Partner")]
        public string Partner { get; set; } = string.Empty;
        [JsonPropertyName("Reseller")]
        public string Reseller { get; set; } = string.Empty;// R/O Ở đây có thể null hoặc có thể không mã pin của Reseller (TE27PT00)
    }
    public class DistributorValidator : AbstractValidator<Distributor>
    {
        public DistributorValidator()
        {
            RuleFor(x => x.Partner)
                .NotNull().WithMessage("Partner is riquiresd");
        }

    }
}
