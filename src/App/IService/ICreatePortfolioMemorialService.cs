using Microsoft.AspNetCore.Mvc;
using stela_api.src.Domain.Entities.Request;

namespace stela_api.src.App.IService
{
    public interface ICreatePortfolioMemorialService
    {
        Task<IActionResult> Invoke(CreatePortfolioMemorialBody body);
    }
}