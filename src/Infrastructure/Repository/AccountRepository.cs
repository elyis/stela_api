using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.IRepository;
using stela_api.src.Domain.Models;
using stela_api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using stela_api.src.Domain.Enums;
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
                PasswordHash = body.Password,
                RoleName = role,
                FirstName = body.FirstName,
                LastName = body.LastName,
                IsEmailVerified = true,
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

            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<Account?> UpdateConfirmationCode(Guid id, string code)
        {
            var account = await GetById(id);
            if (account == null)
                return null;

            account.ConfirmationCode = code;
            account.ConfirmationCodeValidBefore = DateTime.UtcNow.AddMinutes(5);
            await _context.SaveChangesAsync();

            return account;
        }

        public async Task<Account?> VerifyConfirmationCode(Guid id, AccountVerificationBody body)
        {
            var account = await GetById(id);
            if (account == null)
                return null;

            if (account.ConfirmationCode != body.Code || account.ConfirmationCodeValidBefore < DateTime.UtcNow)
                return null;

            if (body.Method == VerificationMethod.Email)
            {
                account.IsEmailVerified = true;
                account.Email = body.Identifier;
            }
            else
            {
                account.IsPhoneVerified = true;
                account.Phone = body.Identifier;
            }

            account.ConfirmationCode = null;
            account.ConfirmationCodeValidBefore = null;

            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<Account?> ChangePassword(Guid id, string password, string newPassword)
        {
            var account = await GetById(id);
            if (account == null)
                return null;

            var hashPassword = Hmac512Provider.Compute(password);
            if (hashPassword != account.PasswordHash)
                return null;

            account.PasswordHash = Hmac512Provider.Compute(newPassword);
            account.LastPasswordDateModified = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return account;
        }

        public async Task<IEnumerable<Account>> GetAllAccounts(int count, int offset, bool isOrderByAscending = true)
        {
            var query = _context.Accounts.Take(count).Skip(offset);
            if (isOrderByAscending)
                query = query.OrderBy(e => e.CreatedAt);
            else
                query = query.OrderByDescending(e => e.CreatedAt);

            return await query.ToListAsync();
        }

        public async Task<int> GetTotalAccounts() => await _context.Accounts.CountAsync();

        public async Task<Account?> Update(UserPatchBody body)
        {
            var account = await GetById(body.Id);
            if (account == null)
                return null;

            if (body.FirstName != null)
                account.FirstName = body.FirstName;

            if (body.LastName != null)
                account.LastName = body.LastName;

            if (body.Email != null)
                account.Email = body.Email;

            if (body.Role != null)
                account.RoleName = body.Role.ToString();

            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<bool> RemoveAccount(Guid id)
        {
            var account = await GetById(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> RemoveAccount(string email)
        {
            var account = await GetByEmail(email);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }

            return true;
        }
    }
}