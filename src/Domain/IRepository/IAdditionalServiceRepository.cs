using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.Models;

namespace stela_api.src.Domain.IRepository
{
    public interface IAdditionalServiceRepository
    {
        Task<IEnumerable<AdditionalService>> GetAll(int count, int offset);
        Task<AdditionalService?> GetById(Guid id);
        Task<AdditionalService?> GetByName(string name);
        Task<AdditionalService?> CreateService(CreateAdditionalServiceBody body);
        Task<bool> DeleteService(Guid id);
        Task<AdditionalService?> UpdateImage(Guid id, string filename);
    }
}

