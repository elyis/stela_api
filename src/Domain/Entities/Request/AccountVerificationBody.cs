using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

using stela_api.src.Domain.Enums;

namespace stela_api.src.Domain.Entities.Request
{
    public class AccountVerificationBody : IValidatableObject
    {
        [Required]
        public string Identifier { get; set; }

        [Required]
        [MinLength(4)]
        public string Code { get; set; }
        [EnumDataType(typeof(VerificationMethod))]
        public VerificationMethod Method { get; set; } = VerificationMethod.Email;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Method == VerificationMethod.Email && !IsValidEmail(Identifier))
            {
                yield return new ValidationResult(new("Identifier must be email format"));
            }

            if (Method == VerificationMethod.Phone && !IsValidPhoneNumber(Identifier))
            {
                yield return new ValidationResult(new("Identifier must be phone number format"));
            }
        }

        private bool IsValidEmail(string email)
        {
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            var regex = new Regex(@"^[1-9]\d{1,14}$");
            return regex.IsMatch(phoneNumber);
        }
    }
}