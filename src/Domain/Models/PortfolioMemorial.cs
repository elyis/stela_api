using stela_api.src.Domain.Entities.Request;

namespace stela_api.src.Domain.Models
{
    public class PortfolioMemorial
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public string CemeteryName { get; set; }

        public List<PortfolioMemorialMaterials> Materials { get; set; } = new();

        public PortfolioMemorialBody ToPortfolioMemorialBody()
        {
            return new PortfolioMemorialBody
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Image = Image == null ? null : $"{Constants.WebPathToPortfolioMemorialImages}{Image}",
                CemeteryName = CemeteryName,
                Materials = Materials.Select(m => m.Material.ToMemorialMaterialBody()).ToList()
            };
        }

        public ShortPortfolioMemorialBody ToShortPortfolioMemorialBody()
        {
            return new ShortPortfolioMemorialBody
            {
                Id = Id,
                Name = Name,
                Image = Image == null ? null : $"{Constants.WebPathToPortfolioMemorialImages}{Image}",
                CemeteryName = CemeteryName,
                Materials = Materials.Select(m => m.Material.ToMemorialMaterialBody()).ToList()
            };
        }
    }
}