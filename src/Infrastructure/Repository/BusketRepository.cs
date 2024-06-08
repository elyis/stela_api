
using Microsoft.EntityFrameworkCore;

using stela_api.src.Domain.IRepository;
using stela_api.src.Domain.Models;
using stela_api.src.Infrastructure.Data;

namespace stela_api.src.Infrastructure.Repository
{
    public class BusketRepository : IBusketRepository
    {
        private readonly AppDbContext _context;

        public BusketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BusketItem?> AddBusketItem(Guid busketId, Memorial memorial, int quantity)
        {
            var busket = await GetBusketById(busketId);
            if (busket == null)
                return null;

            var busketItem = new BusketItem
            {
                Busket = busket,
                Memorial = memorial,
                Quantity = quantity
            };

            await _context.BusketItems.AddAsync(busketItem);
            await _context.SaveChangesAsync();

            return busketItem;
        }

        public async Task<Busket?> CreateBusket(Account account)
        {
            var busket = await GetBusketByAccountId(account.Id);
            if (busket != null)
                return null;

            busket = new Busket
            {
                Account = account
            };

            await _context.Buskets.AddAsync(busket);
            await _context.SaveChangesAsync();

            return busket;
        }

        public async Task<Busket?> GetBusketByAccountId(Guid accountId) => await _context.Buskets.FirstOrDefaultAsync(b => b.AccountId == accountId);

        public async Task<Busket?> GetBusketById(Guid id) => await _context.Buskets.FirstOrDefaultAsync(b => b.Id == id);

        public async Task<BusketItem?> GetBusketItem(Guid busketId, Guid memorialId) => await _context.BusketItems.Include(e => e.Memorial)
            .FirstOrDefaultAsync(b => b.BusketId == busketId && b.MemorialId == memorialId);
        public async Task<BusketItem?> GetBusketItem(Guid busketItemId) => await _context.BusketItems.Include(e => e.Memorial)
            .FirstOrDefaultAsync(b => b.Id == busketItemId);

        public async Task<int> GetCountItemsByBusketId(Guid id) => await _context.BusketItems.CountAsync(e => e.BusketId == id);

        public async Task<List<BusketItem>> GetItemsByBusketId(Guid id, int count, int offset)
        {
            return await _context.BusketItems.Include(e => e.Memorial)
                .Where(e => e.BusketId == id)
                .Skip(offset)
                .Take(count)
                .ToListAsync();
        }

        public async Task<bool> RemoveBusketItem(Guid busketId, Guid memorialId)
        {
            var busketItem = await GetBusketItem(busketId, memorialId);
            if (busketItem != null)
            {
                _context.BusketItems.Remove(busketItem);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> RemoveBusketItem(Guid busketItemId)
        {
            var busketItem = await GetBusketItem(busketItemId);
            if (busketItem != null)
            {
                _context.BusketItems.Remove(busketItem);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<BusketItem?> UpdateBusketItem(Guid busketItemId, int quantity)
        {
            var busketItem = await GetBusketItem(busketItemId);
            if (busketItem != null)
            {
                busketItem.Quantity = quantity;
                await _context.SaveChangesAsync();
                return busketItem;
            }

            return null;
        }
    }
}

