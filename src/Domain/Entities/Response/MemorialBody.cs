namespace stela_api.src.Domain.Entities.Response
{
    public class BaseMemorialBody
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<string> Images { get; set; } = new();

        public List<MemorialMaterialBody> Materials { get; set; } = new();
    }

    public class MemorialBody : BaseMemorialBody
    {
        public string Description { get; set; }
        public float Price { get; set; }
        public string StelaSize { get; set; }
    }

    public class ShortMemorialBody : BaseMemorialBody
    {
    }
}

