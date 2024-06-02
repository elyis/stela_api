using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.Models;

namespace stela_api.src.Domain.IRepository
{
    public interface IUnconfirmedAccountRepository
    {
        Task<UnconfirmedAccount?> GetByEmail(string email);
        Task<UnconfirmedAccount?> AddAsync(SignUpBody body, string confirmationCode);
        Task<bool> Remove(string email);
    }
}