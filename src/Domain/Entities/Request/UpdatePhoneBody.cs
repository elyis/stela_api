using System.ComponentModel.DataAnnotations;

namespace stela_api.src.Domain.Entities.Request
{
    public class UpdatePhoneBody
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}