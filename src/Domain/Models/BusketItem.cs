using stela_api.src.Domain.Entities.Response;

namespace stela_api.src.Domain.Models
{
    public class BusketItem
    {
        public Guid Id { get; set; }
        public Memorial Memorial { get; set; }
        public Guid MemorialId { get; set; }
        public Busket Busket { get; set; }
        public Guid BusketId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }

        public BusketItemBody ToBusketItemBody()
        {
            return new BusketItemBody
            {
                Id = Id,
                Memorial = Memorial.ToShortMemorialBody(),
                Quantity = Quantity,
            };
        }
    }
}

