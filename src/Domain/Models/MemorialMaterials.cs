namespace stela_api.src.Domain.Models
{
    public class MemorialMaterials
    {
        public Guid MemorialId { get; set; }
        public Memorial Memorial { get; set; }
        public Guid MaterialId { get; set; }
        public MemorialMaterial Material { get; set; }
    }
}