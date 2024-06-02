using stela_api.src.Domain.Entities.Request;
using Microsoft.AspNetCore.Mvc;

namespace stela_api.src.App.IService
{
    public interface IAuthService
    {
        Task<IActionResult> ApplyForRegistration(SignUpBody body);
        Task<IActionResult> SignIn(SignInBody body);
        Task<IActionResult> RestoreToken(string refreshToken);
        Task<IActionResult> VerifyUnconfirmedAccount(string email, string code);
    }
}