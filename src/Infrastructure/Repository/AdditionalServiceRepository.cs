using Microsoft.EntityFrameworkCore;

using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.IRepository;
using stela_api.src.Domain.Models;
using stela_api.src.Infrastructure.Data;

namespace stela_api.src.Infrastructure.Repository
{
    public class AdditionalServiceRepository : IAdditionalServiceRepository
    {
        private readonly AppDbContext _context;

        public AdditionalServiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AdditionalService?> CreateService(CreateAdditionalServiceBody body)
        {
            var service = await GetByName(body.Name);
            if (service == null)
            {
                service = new AdditionalService
                {
                    Name = body.Name,
                    Price = body.Price,
                    Description = body.Description,
                };

                service = (await _context.AdditionalServices.AddAsync(service)).Entity;
                await _context.SaveChangesAsync();
            }

            return service;
        }

        public async Task<bool> DeleteService(Guid id)
        {
            var service = await GetById(id);
            if (service != null)
            {
                _context.AdditionalServices.Remove(service);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<IEnumerable<AdditionalService>> GetAll(int count, int offset)
        {
            return await _context.AdditionalServices.Skip(offset)
                .Take(count)
                .ToListAsync();
        }

        public async Task<AdditionalService?> GetById(Guid id) => await _context.AdditionalServices.FirstOrDefaultAsync(service => service.Id == id);

        public async Task<AdditionalService?> GetByName(string name)
        {
            var nameInLowerCase = name.ToLower();
            return await _context.AdditionalServices.FirstOrDefaultAsync(service => service.Name.ToLower() == nameInLowerCase);
        }

        public async Task<AdditionalService?> UpdateImage(Guid id, string filename)
        {
            var service = await GetById(id);
            if (service != null)
            {
                service.Image = filename;
                await _context.SaveChangesAsync();
            }
            return service;
        }
    }
}