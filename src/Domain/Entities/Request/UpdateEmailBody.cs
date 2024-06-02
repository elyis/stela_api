using System.ComponentModel.DataAnnotations;

namespace stela_api.src.Domain.Entities.Request
{
    public class UpdateEmailBody
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}