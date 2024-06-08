using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.Models;

namespace stela_api.src.Domain.IRepository
{
    public interface IMemorialRepository
    {
        Task<IEnumerable<Memorial>> GetAllMemorials(int count, int offset);
        Task<Memorial?> GetMemorialById(Guid id);
        Task<int> GetMemorialsCount();
        Task<Memorial> CreateMemorial(CreateMemorialBody createMemorialBody, IEnumerable<MemorialMaterial> materials);
        Task<bool> DeleteMemorial(Guid id);
        Task<Memorial?> AddImage(Guid id, string filename);
        Task<string?> RemoveImage(Guid id, string filename);
    }
}

