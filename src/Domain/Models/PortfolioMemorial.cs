using stela_api.src.Domain.Entities.Request;

namespace stela_api.src.Domain.Models
{
    public class PortfolioMemorial
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Images { get; set; }
        public string CemeteryName { get; set; }

        public List<PortfolioMemorialMaterials> Materials { get; set; } = new();

        public PortfolioMemorialBody ToPortfolioMemorialBody()
        {
            return new PortfolioMemorialBody
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Images = GetImages(),
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
                Images = GetImages(),
                CemeteryName = CemeteryName,
                Materials = Materials.Select(m => m.Material.ToMemorialMaterialBody()).ToList()
            };
        }

        private List<string> GetImages()
        {
            return string.IsNullOrEmpty(Images)
                ? new List<string>()
                : Images.Split(";").Select(e => $"{Constants.WebPathToPortfolioMemorialImages}{e}").ToList();
        }
    }
}