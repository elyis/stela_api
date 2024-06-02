using System.ComponentModel.DataAnnotations;

namespace stela_api.src.Domain.Entities.Request
{
    public class UpdatePhoneBody
    {
        [Phone, Required]
        public string PhoneNumber { get; set; }
    }
}