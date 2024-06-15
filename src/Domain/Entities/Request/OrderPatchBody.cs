using System.ComponentModel.DataAnnotations;

namespace stela_api.src.Domain.Entities.Request
{
    public class OrderPatchBody
    {
        [Required]
        public Guid Id { get; set; }

        [Required, Range(1, float.MaxValue)]
        public float? TotalPrice { get; set; }
    }
}