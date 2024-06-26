using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.Models;

namespace stela_api.src.Domain.IRepository
{
    public interface IAccountRepository
    {
        Task<Account?> AddAsync(SignUpBody body, string role);
        Task<Account?> GetById(Guid id);
        Task<Account?> GetByEmail(string email);
        Task<Account?> ChangePassword(Guid id, string password, string newPassword);
        Task<string?> UpdateTokenAsync(string refreshToken, Guid userId, TimeSpan? duration = null);
        Task<Account?> GetByTokenAsync(string refreshTokenHash);
        Task<Account?> UpdateConfirmationCode(Guid id, string code);
        Task<Account?> VerifyConfirmationCode(Guid id, AccountVerificationBody body);
        Task<IEnumerable<Account>> GetAllAccounts(int count, int offset, bool isOrderByAscending = true);
        Task<Account?> Update(UserPatchBody body);
        Task<int> GetTotalAccounts();
        Task<Account?> Update(Guid id, UpdateAccountBody body);
        Task<Account?> UpdateImage(Guid userId, string filename);
        Task<bool> RemoveAccount(Guid id);
        Task<bool> RemoveAccount(string email);
    }
}