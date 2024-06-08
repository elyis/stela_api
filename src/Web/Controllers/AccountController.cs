using stela_api.src.App.IService;
using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.Entities.Shared;
using stela_api.src.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using stela_api.src.Domain.IRepository;
using System.Net.Http.Headers;
using webApiTemplate.src.App.IService;

namespace stela_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly IAccountRepository _accountRepository;


        public AccountController(
            IAuthService authService,
            IJwtService jwtService,
            IAccountRepository accountRepository)
        {
            _authService = authService;
            _jwtService = jwtService;
            _accountRepository = accountRepository;
        }


        [SwaggerOperation("Подать заявку на регистрацию")]
        [SwaggerResponse(200, "Успешно создан")]
        [SwaggerResponse(400, "Код не был отправлен на почту")]


        [HttpPost("apply-registration")]
        public async Task<IActionResult> ApplyForRegistrationAsync(SignUpBody signUpBody)
        {
            var result = await _authService.ApplyForRegistration(signUpBody);
            return result;
        }

        [SwaggerOperation("Подтвердить регистрацию")]
        [SwaggerResponse(200, "Успешно создан", Type = typeof(TokenPair))]
        [SwaggerResponse(400, "Неверный метод верификации, ошибочный код или время жизни истекло")]
        [SwaggerResponse(404)]
        [SwaggerResponse(409, "Почта уже существует")]

        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync(AccountVerificationBody body)
        {
            if (body.Method == VerificationMethod.Email)
            {
                var result = await _authService.VerifyUnconfirmedAccount(body.Identifier, body.Code);
                return result;
            }

            return BadRequest();
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

        [SwaggerOperation("Изменение пароля")]
        [SwaggerResponse(200, "Успешно создан")]

        [HttpPatch("change-password"), Authorize]
        public async Task<IActionResult> ChangePassword(
            UpdatePasswordBody body,
            [FromHeader(Name = nameof(HttpRequestHeaders.Authorization))] string token)
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);
            var result = await _accountRepository.ChangePassword(tokenPayload.UserId, body.Password);
            return Ok();
        }
    }
}