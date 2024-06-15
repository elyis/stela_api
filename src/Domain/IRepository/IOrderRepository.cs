using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.Entities.Response;
using stela_api.src.Domain.Models;

namespace stela_api.src.Domain.IRepository
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrder(Account client, Memorial memorial);
        Task<Order?> UpdateOrder(OrderPatchBody body);
        Task<Order?> GetOrderById(Guid id);
        Task<IEnumerable<OrderBody>> GetOrders(int count, int offset, bool isOrderByDescending = true);
        Task<int> GetOrdersCount();
        Task<IEnumerable<OrderBody>> GetOrdersByClientId(Guid clientId, int count, int offset, bool isOrderByDescending = true);
        Task<int> GetOrdersByClientIdCount(Guid clientId);
        Task<bool> DeleteOrder(Guid id);
    }
}