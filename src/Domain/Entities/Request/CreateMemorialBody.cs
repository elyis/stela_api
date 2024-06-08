using System.ComponentModel.DataAnnotations;

using stela_api.src.Domain.Enums;

namespace stela_api.src.Domain.Entities.Request
{
    public class CreateMemorialBody
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public CemeteryNames CemeteryName { get; set; }

        [Required]
        public required IEnumerable<Guid> MaterialIds { get; set; }
    }
}

