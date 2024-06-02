using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.IRepository;
using stela_api.src.Domain.Models;
using stela_api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using webApiTemplate.src.App.Provider;

namespace stela_api.src.Infrastructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Account?> AddAsync(SignUpBody body, string role)
        {
            var oldUser = await GetByEmail(body.Email);
            if (oldUser != null)
                return null;

            var newUser = new Account
            {
                Email = body.Email,
                PasswordHash = Hmac512Provider.Compute(body.Password),
                RoleName = role,
                FirstName = body.FirstName,
                LastName = body.LastName,
                Phone = body.Phone,
            };

            var result = await _context.Accounts.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<Account?> GetById(Guid id)
            => await _context.Accounts
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<Account?> GetByEmail(string email)
            => await _context.Accounts
                .FirstOrDefaultAsync(e => e.Email == email);

        public async Task<Account?> GetByTokenAsync(string refreshTokenHash)
            => await _context.Accounts
            .FirstOrDefaultAsync(e => e.Token == refreshTokenHash);

        public async Task<Account?> UpdateImage(Guid userId, string filename)
        {
            var user = await GetById(userId);
            if (user == null)
                return null;

            user.Image = filename;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<string?> UpdateTokenAsync(string refreshToken, Guid userId, TimeSpan? duration = null)
        {
            var user = await GetById(userId);
            if (user == null)
                return null;

            if (duration == null)
                duration = TimeSpan.FromDays(15);

            if (user.TokenValidBefore <= DateTime.UtcNow || user.TokenValidBefore == null)
            {
                user.TokenValidBefore = DateTime.UtcNow.Add((TimeSpan)duration);
                user.Token = refreshToken;
                await _context.SaveChangesAsync();
            }

            return user.Token;
        }


        public async Task<Account?> Update(Guid id, UpdateAccountBody body)
        {
            var account = await GetById(id);
            if (account == null)
                return null;

            if (body.FirstName != null)
                account.FirstName = body.FirstName;

            if (body.LastName != null)
                account.LastName = body.LastName;

            if (body.Email != null)
                account.Email = body.Email;

            if (body.Phone != null)
                account.Phone = body.Phone;

            await _context.SaveChangesAsync();
            return account;
        }
    }
}