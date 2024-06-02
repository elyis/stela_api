using System.ComponentModel.DataAnnotations;

namespace stela_api.src.Domain.Entities.Request
{
    public class UpdateNameBody
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must contain only letters")]
        public string Name { get; set; }
    }
}