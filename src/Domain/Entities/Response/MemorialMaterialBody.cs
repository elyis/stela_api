namespace stela_api.src.Domain.Entities.Response
{
    public class MemorialMaterialBody
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public string? Hex { get; set; }
    }
}