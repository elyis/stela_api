using stela_api.src.Domain.Entities.Response;

namespace stela_api.src.Domain.Models
{
    public class Memorial
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CemeteryName { get; set; }
        public string Description { get; set; }
        public string? Images { get; set; }

        public List<MemorialMaterials> Materials { get; set; } = new List<MemorialMaterials>();
        public List<BusketItem> BusketItems { get; set; } = new List<BusketItem>();

        public MemorialBody ToMemorialBody()
        {
            return new MemorialBody
            {
                Id = Id,
                Name = Name,
                CemeteryName = CemeteryName,
                Description = Description,
                Images = GetImages(),
                Materials = Materials.Select(e => e.Material.ToMemorialMaterialBody()).ToList()
            };
        }

        public ShortMemorialBody ToShortMemorialBody()
        {
            return new ShortMemorialBody
            {
                Id = Id,
                Name = Name,
                CemeteryName = CemeteryName,
                Images = GetImages(),
                Materials = Materials.Select(e => e.Material.ToMemorialMaterialBody()).ToList()
            };
        }

        private List<string> GetImages()
        {
            return string.IsNullOrEmpty(Images)
                ? new List<string>()
                : Images.Split(";").Select(e => $"{Constants.WebPathToMemorialImages}{e}").ToList();
        }
    }
}

