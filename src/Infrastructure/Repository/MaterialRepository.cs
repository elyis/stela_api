
using Microsoft.EntityFrameworkCore;

using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.IRepository;
using stela_api.src.Domain.Models;
using stela_api.src.Infrastructure.Data;

namespace stela_api.src.Infrastructure.Repository
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly AppDbContext _context;

        public MaterialRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MemorialMaterial?> AddMaterial(CreateMemorialMaterialBody body)
        {
            var material = await GetMaterialByName(body.Name);
            if (material != null)
                return null;

            material = new MemorialMaterial
            {
                Name = body.Name,
                Hex = body.Hex,
            };

            await _context.Materials.AddAsync(material);
            await _context.SaveChangesAsync();

            return material;
        }

        public async Task<MemorialMaterial?> GetMaterialById(Guid id) => await _context.Materials.FirstOrDefaultAsync(e => e.Id == id);

        public async Task<MemorialMaterial?> GetMaterialByName(string name)
        {
            var nameInLowerCase = name.ToLower();
            return await _context.Materials.FirstOrDefaultAsync(e => e.Name.ToLower() == nameInLowerCase);
        }

        public async Task<long> GetMaterialCount()
        {
            return await _context.Materials.LongCountAsync();
        }

        public async Task<List<MemorialMaterial>> GetMaterials(int count, int offset)
        {
            return await _context.Materials.Skip(offset)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<MemorialMaterial>> GetMaterials(IEnumerable<Guid> ids) => await _context.Materials.Where(e => ids.Contains(e.Id))
            .ToListAsync();

        public async Task<MemorialMaterial?> UpdateMaterialImage(Guid id, string filename)
        {
            var material = await GetMaterialById(id);
            if (material == null)
                return null;

            material.Image = filename;
            await _context.SaveChangesAsync();

            return material;
        }
    }
}