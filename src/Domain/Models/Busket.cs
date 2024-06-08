namespace stela_api.src.Domain.Models
{
    public class Busket
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
        public List<BusketItem> BusketItems { get; set; } = new();
    }
}