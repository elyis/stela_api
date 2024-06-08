using System.ComponentModel.DataAnnotations;

namespace stela_api.src.Domain.Entities.Request
{
    public class CreateAdditionalServiceBody
    {
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(1, float.MaxValue)]
        public float Price { get; set; }
    }
}