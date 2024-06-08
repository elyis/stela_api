using System.ComponentModel.DataAnnotations;

namespace stela_api.src.Domain.Entities.Request
{
    public class CreateBusketItemBody
    {
        public Guid MemorialId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}