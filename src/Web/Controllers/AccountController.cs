using stela_api.src.App.IService;
using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.Entities.Shared;
using stela_api.src.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace stela_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;


        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }


        [SwaggerOperation("Регистрация")]
        [SwaggerResponse(200, "Успешно создан", Type = typeof(TokenPair))]
        [SwaggerResponse(400, "Токен не валиден или активирован")]
        [SwaggerResponse(409, "Почта уже существует")]


        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync(SignUpBody signUpBody)
        {
            string role = Enum.GetName(UserRole.User)!;
            var result = await _authService.SignUp(signUpBody, role);
            return result;
        }


        [SwaggerOperation("Авторизация")]
        [SwaggerResponse(200, "Успешно", Type = typeof(TokenPair))]
        [SwaggerResponse(400, "Пароли не совпадают")]
        [SwaggerResponse(404, "Email не зарегистрирован")]

        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync(SignInBody signInBody)
        {
            var result = await _authService.SignIn(signInBody);
            return result;
        }

        [SwaggerOperation("Восстановление токена")]
        [SwaggerResponse(200, "Успешно создан", Type = typeof(TokenPair))]
        [SwaggerResponse(404, "Токен не используется")]

        [HttpPatch("restore-token")]
        public async Task<IActionResult> RestoreTokenAsync(TokenBody body)
        {
            var result = await _authService.RestoreToken(body.Value);
            return result;
        }
    }
}