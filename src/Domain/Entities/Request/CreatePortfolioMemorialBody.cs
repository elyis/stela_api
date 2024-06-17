using System.ComponentModel.DataAnnotations;

namespace stela_api.src.Domain.Entities.Request
{
    public class CreatePortfolioMemorialBody
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public required IEnumerable<Guid> MaterialIds { get; set; }
    }
}