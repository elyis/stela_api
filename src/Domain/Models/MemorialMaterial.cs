using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

using stela_api.src.Domain.Entities.Response;

namespace stela_api.src.Domain.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class MemorialMaterial
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }

        [MaxLength(6)]
        public string? Hex { get; set; }

        public List<MemorialMaterials> Memorials { get; set; } = new List<MemorialMaterials>();

        public MemorialMaterialBody ToMemorialMaterialBody()
        {
            return new MemorialMaterialBody
            {
                Id = Id,
                Hex = Hex != null ? $"#{Hex}" : null,
                Image = Image == null ? null : $"{Constants.WebPathToMaterialImages}{Image}",
                Name = Name
            };
        }
    }
}