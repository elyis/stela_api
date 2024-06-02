using System.ComponentModel.DataAnnotations;

namespace stela_api.src.Domain.Entities.Request
{
    public class UpdateEmailBody
    {
        [EmailAddress, Required]
        public string Email { get; set; }
    }
}