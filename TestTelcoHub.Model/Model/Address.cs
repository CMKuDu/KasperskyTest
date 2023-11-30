using FluentValidation;
using System.Text.Json.Serialization;

namespace TestTelcoHub.Model.Model
{
    public class Address
    {
        [JsonIgnore]
        [JsonPropertyName("AddressId")]
        public Guid AddressId { get; set; } = Guid.NewGuid();
        [JsonPropertyName("Country")]
        public string Country { get; set; } = string.Empty;
    }
    public class AddressValidator : AbstractValidator<Address>                              
    {
        public AddressValidator() 
        {
            RuleFor(x => x.Country)
                .NotNull().WithMessage("Country is requiresd")
                .Length(3).WithMessage("Country must be 3 characters");
        }
    }
}
