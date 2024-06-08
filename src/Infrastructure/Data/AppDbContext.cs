using stela_api.src.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace stela_api.src.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _config;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IConfiguration config) : base(options)
        {
            _config = config;
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<UnconfirmedAccount> UnconfirmedAccounts { get; set; }
        public DbSet<MemorialMaterial> Materials { get; set; }
        public DbSet<Memorial> Memorials { get; set; }
        public DbSet<MemorialMaterials> MemorialMaterials { get; set; }
        public DbSet<Busket> Buskets { get; set; }
        public DbSet<BusketItem> BusketItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Environment.GetEnvironmentVariable("PostgresConnectionStringForStela") ?? throw new Exception("db connection is empty");
            optionsBuilder.UseNpgsql(connectionString);
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MemorialMaterials>()
                .HasKey(e => new { e.MemorialId, e.MaterialId });

            base.OnModelCreating(modelBuilder);
        }
    }

}