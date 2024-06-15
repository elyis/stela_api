using System.ComponentModel.DataAnnotations;

namespace stela_api.src.Domain.Entities.Request
{
    public class CreateOrderBody
    {
        [Required]
        public Guid MemorialId { get; set; }
    }
}