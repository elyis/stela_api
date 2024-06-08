using System.Net;
using stela_api.src.Domain.Entities.Response;
using stela_api.src.Domain.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using webApiTemplate.src.App.IService;
using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.Entities.Shared.Utility;
using stela_api.src.App.IService;
using System.ComponentModel.DataAnnotations;

namespace stela_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProfileController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly IFileUploaderService _fileUploaderService;
        private readonly IPhoneService _phoneService;
        private readonly string[] _supportedImageExtensions = new string[]
        {
            "gif",
            "jpg",
            "jpeg",
            "jfif",
            "png",
            "svg"
        };

        public ProfileController(
            IAccountRepository accountRepository,
            IJwtService jwtService,
            IEmailService emailService,
            IFileUploaderService fileUploaderService,
            IPhoneService phoneService)
        {
            _accountRepository = accountRepository;
            _jwtService = jwtService;
            _emailService = emailService;
            _fileUploaderService = fileUploaderService;
            _phoneService = phoneService;
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

        [HttpPatch("me/email"), Authorize]
        [SwaggerOperation("Обновить почту пользователя")]
        [SwaggerResponse(200)]
        [SwaggerResponse(400)]
        [SwaggerResponse(404)]

        public async Task<IActionResult> UpdateEmail(
            UpdateEmailBody body,
            [FromHeader(Name = nameof(HttpRequestHeader.Authorization))] string token)
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);

            var code = CodeGenerator.Generate();
            var isMessageSent = await _emailService.SendMessage(body.Email, "Confirm email", code);
            if (!isMessageSent)
                return BadRequest("message was not delivered");

            var result = await _accountRepository.UpdateConfirmationCode(tokenPayload.UserId, code);
            return result == null ? NotFound() : Ok();
        }

        [HttpPatch("me/phone"), Authorize]
        [SwaggerOperation("Обновить номер пользователя")]
        [SwaggerResponse(200)]
        [SwaggerResponse(400)]
        [SwaggerResponse(404)]

        public async Task<IActionResult> UpdatePhone(
            UpdatePhoneBody body,
            [FromHeader(Name = nameof(HttpRequestHeader.Authorization))] string token)
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);

            var code = CodeGenerator.Generate();
            var isMessageSent = await _phoneService.SendMessage(body.PhoneNumber, $"Code: {code}");
            if (!isMessageSent)
                return BadRequest("message was not delivered");

            var result = await _accountRepository.UpdateConfirmationCode(tokenPayload.UserId, code);
            return result == null ? NotFound() : Ok();
        }

        [HttpPatch("me/verify"), Authorize]
        [SwaggerOperation("Верифицировать почту или номер")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404)]

        public async Task<IActionResult> VerifyPhoneOrEmail(
            AccountVerificationBody body,
            [FromHeader(Name = nameof(HttpRequestHeader.Authorization))] string token)
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);
            var result = await _accountRepository.VerifyConfirmationCode(tokenPayload.UserId, body);
            return result == null ? NotFound() : Ok();
        }


        [HttpPost("upload/me"), Authorize]
        [SwaggerOperation("Загрузить иконку профиля")]
        [SwaggerResponse(200, Description = "Успешно")]

        public async Task<IActionResult> UploadProfileIcon(
            [FromHeader(Name = nameof(HttpRequestHeader.Authorization))] string token,
            [FromForm, Required] IFormFile file
        )
        {
            var response = await _fileUploaderService.UploadFileAsync(Constants.LocalPathToProfileIcons, file.OpenReadStream(), _supportedImageExtensions);

            if (response is OkObjectResult result)
            {
                var filename = (string)result.Value;
                var tokenInfo = _jwtService.GetTokenPayload(token);
                await _accountRepository.UpdateImage(tokenInfo.UserId, filename);
            }
            return response;
        }

        [HttpGet("upload/me/{filename}")]
        [SwaggerOperation("Получить иконку профиля")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(File))]
        [SwaggerResponse(404, Description = "Неверное имя файла")]

        public async Task<IActionResult> GetProfileIcon(string filename)
            => await _fileUploaderService.GetStreamFileAsync(Constants.LocalPathToProfileIcons, filename);
    }
}