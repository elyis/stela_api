using Microsoft.EntityFrameworkCore;

using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.IRepository;
using stela_api.src.Domain.Models;
using stela_api.src.Infrastructure.Data;

namespace stela_api.src.Infrastructure.Repository
{
    public class MemorialRepository : IMemorialRepository
    {
        private readonly AppDbContext _context;

        public MemorialRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Memorial?> AddImage(Guid id, string filename)
        {
            var memorial = await GetMemorialById(id);
            if (memorial != null)
            {
                if (string.IsNullOrEmpty(memorial.Images))
                    memorial.Images = filename;
                else
                {
                    var images = memorial.Images.Split(";").ToList();
                    images.Add(filename);
                    memorial.Images = string.Join(";", images);
                }
                await _context.SaveChangesAsync();
            }

            return memorial;
        }

        public async Task<Memorial> CreateMemorial(CreateMemorialBody createMemorialBody, IEnumerable<MemorialMaterial> materials)
        {
            var memorial = new Memorial
            {
                Name = createMemorialBody.Name,
                Description = createMemorialBody.Description,
                CemeteryName = createMemorialBody.CemeteryName.ToString(),
            };

            var memorialMaterials = materials.Select(e => new MemorialMaterials
            {
                Material = e,
                Memorial = memorial
            });

            await _context.Memorials.AddAsync(memorial);
            await _context.MemorialMaterials.AddRangeAsync(memorialMaterials);
            await _context.SaveChangesAsync();

            return memorial;
        }

        public async Task<int> GetMemorialsCount() => await _context.Memorials.CountAsync();

        public async Task<bool> DeleteMemorial(Guid id)
        {
            var memorial = await GetMemorialById(id);
            if (memorial != null)
            {
                _context.Memorials.Remove(memorial);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<IEnumerable<Memorial>> GetAllMemorials(int count, int offset)
        {
            return await _context.Memorials.Include(e => e.Materials)
                .ThenInclude(e => e.Material)
                .Skip(offset)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Memorial?> GetMemorialById(Guid id) => await _context.Memorials.Include(e => e.Materials)
            .ThenInclude(e => e.Material)
            .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<string?> RemoveImage(Guid id, string filename)
        {
            var memorial = await GetMemorialById(id);
            if (memorial != null)
            {
                if (string.IsNullOrEmpty(memorial.Images))
                    return null;

                var images = memorial.Images.Split(";").ToList();
                images.Remove(filename);
                memorial.Images = string.Join(";", images);
                await _context.SaveChangesAsync();
                return filename;
            }

            return null;
        }
    }
}