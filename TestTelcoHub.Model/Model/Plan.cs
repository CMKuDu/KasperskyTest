using FluentValidation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using TestTelcoHub.Model.Data;

namespace TestTelcoHub.Model.Model
{
    public class Plan
    {
        [JsonIgnore]
        [JsonPropertyName("PlanId")]
        public Guid PlanId { get; set; } = Guid.NewGuid();
        [JsonPropertyName("BillingPlan")]
        public string BillingPlan { get; set; } = string.Empty;
        [JsonPropertyName("Sku")]
        public string Sku { get; set; } = string.Empty;
        [JsonPropertyName("Quantity")]
        public int Quantity { get; set; }
        [JsonPropertyName("Customer")]
        public Customer? Customer { get; set; }
        [JsonIgnore]
        public ExternalReference? ExternalReference { get; set; }
        [JsonPropertyName("ApprovalCode")]
        public string ApprovalCode { get; set; } = string.Empty; // Max Discount: Special Price
        [JsonPropertyName("DeliveryEmail")]
        public string DeliveryEmail { get; set; } = string.Empty;
        [JsonPropertyName("TermsAndConditions")]
        public TermsAndConditions? TermsAndConditions { get; set; }
        [JsonPropertyName("Comment")]
        public string Comment { get; set; } = string.Empty;
        public class PlanValidator : AbstractValidator<Plan>
        {
            public PlanValidator()
            {
                RuleFor(x => x.BillingPlan)
                    .Must(x => x == "Yearly" || x == "PAYG")
                    .WithMessage("BillingPlan must be 'Yearly' or 'PAYG'");
                RuleFor(x => x.Sku)
                    .NotEmpty().WithMessage("Sku is required");
                RuleFor(x => x.Quantity)
                    .GreaterThanOrEqualTo(10)
                    .WithMessage("Quantity must be greater than or equal to 10");
                RuleFor(x => x.DeliveryEmail)
                    .NotEmpty()
                    .Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")
                    .WithMessage("DeliveryEmail is required");
                RuleFor(x => x.Comment)
                    .NotEmpty().WithMessage("Comment is required");
                RuleFor(x => x.ApprovalCode)
                    .MaximumLength(50);
            }
        }
    }
}
