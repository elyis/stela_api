using System.ComponentModel.DataAnnotations;

namespace stela_api.src.Domain.Entities.Request
{
    public class SignInBody
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Min length password must be 3 characters")]
        public string Password { get; set; }
    }
}