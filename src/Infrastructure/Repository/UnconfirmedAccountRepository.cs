using Microsoft.EntityFrameworkCore;

using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.IRepository;
using stela_api.src.Domain.Models;
using stela_api.src.Infrastructure.Data;

using webApiTemplate.src.App.Provider;

namespace stela_api.src.Infrastructure.Repository
{
    public class UnconfirmedAccountRepository : IUnconfirmedAccountRepository
    {
        private readonly AppDbContext _context;

        public UnconfirmedAccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UnconfirmedAccount?> AddAsync(SignUpBody body, string confirmationCode)
        {
            var account = await GetByEmail(body.Email);
            if (account != null)
                return null;

            account = new UnconfirmedAccount
            {
                Email = body.Email,
                ConfirmationCode = confirmationCode,
                ConfirmationCodeValidBefore = DateTime.UtcNow.AddMinutes(5),
                FirstName = body.FirstName,
                LastName = body.LastName,
                PasswordHash = Hmac512Provider.Compute(body.Password)
            };

            account = (await _context.UnconfirmedAccounts.AddAsync(account))?.Entity;
            await _context.SaveChangesAsync();

            return account;
        }

        public async Task<UnconfirmedAccount?> GetByEmail(string email) =>
            await _context.UnconfirmedAccounts.FirstOrDefaultAsync(e => e.Email == email);

        public async Task<bool> Remove(string email)
        {
            var account = await GetByEmail(email);
            if (account == null)
                return true;

            _context.UnconfirmedAccounts.Remove(account);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}