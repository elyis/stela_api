using Microsoft.EntityFrameworkCore;

using stela_api.src.Domain.Entities.Response;

namespace stela_api.src.Domain.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class AdditionalService
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string? Image { get; set; }

        public AdditionalServiceBody ToAdditionalServiceBody()
        {
            return new AdditionalServiceBody
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Price = Price,
                UrlImage = Image != null ? $"{Constants.WebPathToAdditionalServiceImages}{Image}" : null
            };
        }
    }
}

