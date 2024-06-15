namespace stela_api.src.Domain.Entities.Response
{
    public class AdditionalServiceBody
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string? UrlImage { get; set; }
    }
}