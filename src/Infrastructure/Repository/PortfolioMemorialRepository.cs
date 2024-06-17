using Microsoft.EntityFrameworkCore;
using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.IRepository;
using stela_api.src.Domain.Models;
using stela_api.src.Infrastructure.Data;

namespace stela_api.src.Infrastructure.Repository
{
    public class PortfolioMemorialRepository : IPortfolioMemorialRepository
    {
        private readonly AppDbContext _context;

        public PortfolioMemorialRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PortfolioMemorial?> AddImage(Guid id, string filename)
        {
            var portfolioMemorial = await GetPortfolioMemorialById(id);
            if (portfolioMemorial != null)
            {
                var images = portfolioMemorial.Images?.Split(";").ToList() ?? new List<string>();
                images.Add(filename);
                portfolioMemorial.Images = string.Join(";", images);
                await _context.SaveChangesAsync();
            }

            return portfolioMemorial;
        }

        public async Task<PortfolioMemorial> CreatePortfolioMemorial(CreatePortfolioMemorialBody body, IEnumerable<MemorialMaterial> materials)
        {
            var portfolioMemorial = new PortfolioMemorial
            {
                Name = body.Name,
                Description = body.Description,
                CemeteryName = body.CemeteryName,
            };

            var portfolioMemorialMaterials = materials.Select(e => new PortfolioMemorialMaterials
            {
                Material = e,
                PortfolioMemorial = portfolioMemorial
            });

            await _context.PortfolioMemorials.AddAsync(portfolioMemorial);
            await _context.SaveChangesAsync();

            return portfolioMemorial;
        }

        public async Task<int> GetPortfolioMemorialsCount() => await _context.PortfolioMemorials.CountAsync();

        public async Task<bool> DeletePortfolioMemorial(Guid id)
        {
            var portfolioMemorial = await GetPortfolioMemorialById(id);
            if (portfolioMemorial != null)
            {
                _context.PortfolioMemorials.Remove(portfolioMemorial);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<IEnumerable<PortfolioMemorial>> GetAllPortfolioMemorials(int count, int offset)
        {
            return await _context.PortfolioMemorials.Include(e => e.Materials)
                .ThenInclude(e => e.Material)
                .Skip(offset)
                .Take(count)
                .ToListAsync();
        }

        public async Task<PortfolioMemorial?> GetPortfolioMemorialById(Guid id) => await _context.PortfolioMemorials.Include(e => e.Materials)
            .ThenInclude(e => e.Material)
            .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<bool> RemoveImage(Guid id, string filename)
        {
            var portfolioMemorial = await GetPortfolioMemorialById(id);
            if (portfolioMemorial != null)
            {
                if (string.IsNullOrEmpty(portfolioMemorial.Images))
                    return true;

                var images = portfolioMemorial.Images.Split(";").ToList();
                images.Remove(filename);
                portfolioMemorial.Images = string.Join(";", images);
                await _context.SaveChangesAsync();
            }

            return true;
        }
    }
}