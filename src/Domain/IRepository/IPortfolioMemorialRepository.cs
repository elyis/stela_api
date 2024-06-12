using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.Models;

namespace stela_api.src.Domain.IRepository
{
    public interface IPortfolioMemorialRepository
    {
        Task<PortfolioMemorial?> UpdateImage(Guid id, string filename);
        Task<PortfolioMemorial> CreatePortfolioMemorial(CreatePortfolioMemorialBody body, IEnumerable<MemorialMaterial> materials);
        Task<int> GetPortfolioMemorialsCount();
        Task<bool> DeletePortfolioMemorial(Guid id);
        Task<IEnumerable<PortfolioMemorial>> GetAllPortfolioMemorials(int count, int offset);
        Task<PortfolioMemorial?> GetPortfolioMemorialById(Guid id);
        Task<bool> RemoveImage(Guid id);
    }
}