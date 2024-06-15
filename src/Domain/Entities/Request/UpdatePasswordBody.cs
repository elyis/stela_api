using System.ComponentModel.DataAnnotations;

namespace stela_api.src.Domain.Entities.Request
{
    public class UpdatePasswordBody
    {
        [Required]
        [MinLength(3)]
        public string Password { get; set; }

        [Required, MinLength(3)]
        public string NewPassword { get; set; }
    }
}