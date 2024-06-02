using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.Models;

namespace stela_api.src.Domain.IRepository
{
    public interface IAccountRepository
    {
        Task<Account?> AddAsync(SignUpBody body, string role);
        Task<Account?> GetById(Guid id);
        Task<Account?> GetByEmail(string email);
        Task<string?> UpdateTokenAsync(string refreshToken, Guid userId, TimeSpan? duration = null);
        Task<Account?> GetByTokenAsync(string refreshTokenHash);

        Task<Account?> UpdateFirstName(Guid id, string firstName);
        Task<Account?> UpdateLastName(Guid id, string lastName);
        Task<Account?> UpdatePhone(Guid id, string phone);
        Task<Account?> UpdateEmail(Guid id, string email);
        Task<Account?> UpdateImage(Guid userId, string filename);
    }
}