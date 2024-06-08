namespace stela_api.src.Domain.Entities.Response
{
    public class BusketItemBody
    {
        public Guid Id { get; set; }
        public ShortMemorialBody Memorial { get; set; }
        public int Quantity { get; set; }
    }
}

