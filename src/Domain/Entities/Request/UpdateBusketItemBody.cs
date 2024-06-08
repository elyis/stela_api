namespace stela_api.src.Domain.Entities.Request
{
    public class UpdateBusketItemBody
    {
        public Guid BusketItemId { get; set; }
        public int Quantity { get; set; }
    }
}

