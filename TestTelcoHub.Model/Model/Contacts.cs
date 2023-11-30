using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestTelcoHub.Model.Model
{
    public class Contacts
    {
        [Key]
        [JsonIgnore]
        [JsonPropertyName("ContactsId")]
        public Guid ContactsId { get; set; } = Guid.NewGuid();
        [JsonPropertyName("CompanyName")]
        public string CompanyName { set; get; } = string.Empty;

    }
    public class ContactsValidator : AbstractValidator<Contacts>
    {
        public ContactsValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotNull()
                .WithMessage("CompanyName is required");
        }
    }
}
