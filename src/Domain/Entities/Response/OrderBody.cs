namespace stela_api.src.Domain.Entities.Response
{
    public class OrderBody
    {
        public Guid Id { get; set; }
        public string MemorialName { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public string? ClientPhone { get; set; }
        public string StelaSize { get; set; }
        public float TotalPrice { get; set; }
        // public OrderStatus Status { get; set; }
        public string Date { get; set; }
    }
}