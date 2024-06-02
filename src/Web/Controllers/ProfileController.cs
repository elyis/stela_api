using System.Net;
using stela_api.src.Domain.Entities.Response;
using stela_api.src.Domain.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using webApiTemplate.src.App.IService;
using stela_api.src.Domain.Entities.Request;

namespace stela_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProfileController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtService _jwtService;

        public ProfileController(
            IAccountRepository accountRepository,
            IJwtService jwtService)
        {
            _accountRepository = accountRepository;
            _jwtService = jwtService;
        }


        [HttpGet("me"), Authorize]
        [SwaggerOperation("Получить профиль")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(ProfileBody))]
        public async Task<IActionResult> GetProfileAsync(
            [FromHeader(Name = nameof(HttpRequestHeader.Authorization))] string token
        )
        {
            var tokenInfo = _jwtService.GetTokenPayload(token);
            var user = await _accountRepository.GetById(tokenInfo.UserId);
            return user == null ? NotFound() : Ok(user.ToProfileBody());
        }

        [HttpPatch("me"), Authorize]
        [SwaggerOperation("Обновить данные пользователя")]
        [SwaggerResponse(200)]
        [SwaggerResponse(400)]
        [SwaggerResponse(404)]

        public async Task<IActionResult> UpdateAccount(
            UpdateAccountBody body,
            [FromHeader(Name = nameof(HttpRequestHeader.Authorization))] string token)
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);
            var result = await _accountRepository.Update(tokenPayload.UserId, body);
            return result == null ? NotFound() : Ok();
        }
    }
}