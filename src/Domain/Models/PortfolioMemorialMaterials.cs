namespace stela_api.src.Domain.Models
{
    public class PortfolioMemorialMaterials
    {
        public Guid PortfolioMemorialId { get; set; }
        public PortfolioMemorial PortfolioMemorial { get; set; }
        public Guid MemorialMaterialId { get; set; }
        public MemorialMaterial Material { get; set; }
    }
}