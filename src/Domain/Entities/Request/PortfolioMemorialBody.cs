using stela_api.src.Domain.Entities.Response;

namespace stela_api.src.Domain.Entities.Request
{

    public class BasePortfolioMemorialBody
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public string CemeteryName { get; set; }

        public List<MemorialMaterialBody> Materials { get; set; } = new();
    }

    public class PortfolioMemorialBody : BasePortfolioMemorialBody
    {
        public string Description { get; set; }
    }

    public class ShortPortfolioMemorialBody : BasePortfolioMemorialBody
    {
    }
}