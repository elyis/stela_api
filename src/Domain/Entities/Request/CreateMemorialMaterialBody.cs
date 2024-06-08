using System.ComponentModel.DataAnnotations;

namespace stela_api.src.Domain.Entities.Request
{
    public class CreateMemorialMaterialBody
    {
        public string Name { get; set; }

        [RegularExpression("^#?([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$")]
        public string? Hex { get; set; }
    }
}