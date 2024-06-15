using Microsoft.EntityFrameworkCore;
using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.Entities.Response;
using stela_api.src.Domain.IRepository;
using stela_api.src.Domain.Models;
using stela_api.src.Infrastructure.Data;

namespace stela_api.src.Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrder(Account client, Memorial memorial)
        {
            var order = new Order
            {
                TotalPrice = memorial.Price,
                Client = client,
                Memorial = memorial
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<bool> DeleteOrder(Guid id)
        {
            var order = await GetOrderById(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<Order?> GetOrderById(Guid id) => await _context.Orders
            .Include(e => e.Memorial)
            .Include(e => e.Client)
            .FirstOrDefaultAsync(o => o.Id == id);

        public async Task<IEnumerable<OrderBody>> GetOrders(int count, int offset, bool isOrderByDescending = true)
        {
            var query = _context.Orders
                .Include(e => e.Memorial)
                .Include(e => e.Client)
                .Take(count)
                .Skip(offset);

            if (isOrderByDescending)
                query = query.OrderByDescending(o => o.Date);
            else
                query = query.OrderBy(o => o.Date);

            var result = await query.ToListAsync();
            return result.Select(r => r.ToOrderBody());
        }

        public async Task<IEnumerable<OrderBody>> GetOrdersByClientId(Guid clientId, int count, int offset, bool isOrderByDescending = true)
        {
            var query = _context.Orders.Where(o => o.Client.Id == clientId)
                .Include(e => e.Memorial)
                .Include(e => e.Client)
                .Take(count)
                .Skip(offset);

            if (isOrderByDescending)
                query = query.OrderByDescending(o => o.Date);
            else
                query = query.OrderBy(o => o.Date);

            var result = await query.ToListAsync();
            return result.Select(r => r.ToOrderBody());
        }

        public async Task<int> GetOrdersByClientIdCount(Guid clientId) => await _context.Orders
            .Where(o => o.Client.Id == clientId)
            .CountAsync();

        public async Task<int> GetOrdersCount() => await _context.Orders.CountAsync();

        public async Task<Order?> UpdateOrder(OrderPatchBody body)
        {
            var order = await GetOrderById(body.Id);
            if (order == null)
                return null;

            if (body.TotalPrice != null)
                order.TotalPrice = body.TotalPrice.Value;

            await _context.SaveChangesAsync();
            return order;
        }
    }
}