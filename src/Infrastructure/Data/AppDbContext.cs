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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Environment.GetEnvironmentVariable("PostgresConnectionStringForStela", EnvironmentVariableTarget.User) ?? throw new Exception("db connection is empty");
            optionsBuilder.UseNpgsql(connectionString);
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

}