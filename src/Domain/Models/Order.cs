using stela_api.src.Domain.Entities.Response;

namespace stela_api.src.Domain.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }
        public Account Client { get; set; }
        public Guid MemorialId { get; set; }
        public Memorial Memorial { get; set; }
        public float TotalPrice { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public OrderBody ToOrderBody()
        {
            return new OrderBody
            {
                Id = Id,
                ClientFirstName = Client.FirstName,
                ClientLastName = Client.LastName,
                ClientPhone = Client.Phone,
                MemorialName = Memorial.Name,
                UrlImage = Memorial.Image == null ? null : $"{Constants.WebPathToMemorialImages}{Memorial.Image}",
                StelaSize = $"{Memorial.StelaLength}x{Memorial.StelaWidth}x{Memorial.StelaHeight}",
                TotalPrice = TotalPrice,
                Date = Date.ToString("yyyy-MM-dd"),
            };
        }
    }
}