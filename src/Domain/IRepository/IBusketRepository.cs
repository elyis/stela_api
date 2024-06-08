using stela_api.src.Domain.Models;

namespace stela_api.src.Domain.IRepository
{
    public interface IBusketRepository
    {
        Task<Busket?> CreateBusket(Account account);
        Task<Busket?> GetBusketById(Guid id);
        Task<Busket?> GetBusketByAccountId(Guid accountId);
        Task<BusketItem?> GetBusketItem(Guid busketItemId);
        Task<BusketItem?> AddBusketItem(Guid busketId, Memorial memorial, int quantity);
        Task<BusketItem?> GetBusketItem(Guid busketId, Guid memorialId);
        Task<BusketItem?> UpdateBusketItem(Guid busketItemId, int quantity);
        Task<bool> RemoveBusketItem(Guid busketId, Guid memorialId);
        Task<bool> RemoveBusketItem(Guid busketItemId);
        Task<int> GetCountItemsByBusketId(Guid id);
        Task<List<BusketItem>> GetItemsByBusketId(Guid id, int count, int offset);
    }
}

