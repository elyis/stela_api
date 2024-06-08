using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.Models;

namespace stela_api.src.Domain.IRepository
{
    public interface IMaterialRepository
    {
        Task<MemorialMaterial?> GetMaterialById(Guid id);
        Task<MemorialMaterial?> GetMaterialByName(string name);

        Task<List<MemorialMaterial>> GetMaterials(int count, int offset);
        Task<List<MemorialMaterial>> GetMaterials(IEnumerable<Guid> ids);
        Task<long> GetMaterialCount();
        Task<MemorialMaterial?> AddMaterial(CreateMemorialMaterialBody body);
        Task<MemorialMaterial?> UpdateMaterialImage(Guid id, string filename);
    }
}