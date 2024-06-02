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

        [HttpPatch("me/first-name"), Authorize]
        [SwaggerOperation("Изменить имя пользователя")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404)]

        public async Task<IActionResult> UpdateFirstName(
            UpdateNameBody body,
            [FromHeader(Name = nameof(HttpRequestHeader.Authorization))] string token)
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);
            var result = await _accountRepository.UpdateFirstName(tokenPayload.UserId, body.Name);
            return result == null ? NotFound() : Ok();
        }


        [HttpPatch("me/last-name"), Authorize]
        [SwaggerOperation("Изменить фамилию пользователя")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404)]

        public async Task<IActionResult> UpdateLastName(
            UpdateNameBody body,
            [FromHeader(Name = nameof(HttpRequestHeader.Authorization))] string token)
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);
            var result = await _accountRepository.UpdateLastName(tokenPayload.UserId, body.Name);
            return result == null ? NotFound() : Ok();
        }

        [HttpPatch("me/phone"), Authorize]
        [SwaggerOperation("Изменить номер пользователя")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404)]

        public async Task<IActionResult> UpdatePhone(
            UpdatePhoneBody body,
            [FromHeader(Name = nameof(HttpRequestHeader.Authorization))] string token)
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);
            var result = await _accountRepository.UpdatePhone(tokenPayload.UserId, body.PhoneNumber);
            return result == null ? NotFound() : Ok();
        }

        [HttpPatch("me/email"), Authorize]
        [SwaggerOperation("Изменить почту пользователя")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404)]

        public async Task<IActionResult> UpdateEmail(
            UpdateEmailBody body,
            [FromHeader(Name = nameof(HttpRequestHeader.Authorization))] string token)
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);
            var result = await _accountRepository.UpdateEmail(tokenPayload.UserId, body.Email);
            return result == null ? NotFound() : Ok();
        }
    }
}