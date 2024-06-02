using System.Net;
using stela_api.src.Domain.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeDetective;
using Swashbuckle.AspNetCore.Annotations;
using webApiTemplate.src.App.IService;
using System.ComponentModel.DataAnnotations;

namespace stela_api.src.Web.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class UploadController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtService _jwtService;
        private readonly IFileUploaderService _fileUploaderService;
        private readonly ContentInspector _contentInspector;
        private readonly string _supportedIconMime;


        public UploadController(
            IAccountRepository accountRepository,
            IJwtService jwtService,
            IFileUploaderService fileUploaderService,
            ContentInspector contentInspector
        )
        {
            _accountRepository = accountRepository;
            _jwtService = jwtService;
            _fileUploaderService = fileUploaderService;
            _contentInspector = contentInspector;
            _supportedIconMime = "image/";
        }

        [HttpPost("me"), Authorize]
        [SwaggerOperation("Загрузить иконку профиля")]
        [SwaggerResponse(200, Description = "Успешно")]

        public async Task<IActionResult> UploadProfileIcon(
            [FromHeader(Name = nameof(HttpRequestHeader.Authorization))] string token,
            [FromForm, Required] IFormFile file
        )
        {
            if (file.Length == 0)
                return BadRequest("File size cannot be equal to zero");

            var resultUpload = await UploadIconAsync(Constants.LocalPathToProfileIcons, file);

            if (resultUpload is OkObjectResult result)
            {
                var filename = (string)result.Value;
                var tokenInfo = _jwtService.GetTokenPayload(token);
                await _accountRepository.UpdateImage(tokenInfo.UserId, filename);
            }
            return resultUpload;
        }

        [HttpGet("me/{filename}")]
        [SwaggerOperation("Получить иконку профиля")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(File))]
        [SwaggerResponse(404, Description = "Неверное имя файла")]

        public async Task<IActionResult> GetProfileIcon(string filename)
            => await GetIconAsync(Constants.LocalPathToProfileIcons, filename);

        private async Task<IActionResult> GetIconAsync(string path, string filename)
        {
            var bytes = await _fileUploaderService.GetStreamFileAsync(path, filename);
            if (bytes == null)
                return NotFound();

            var fileExtension = Path.GetExtension(filename);
            return File(bytes, $"image/{fileExtension}", filename);
        }

        private async Task<IActionResult> UploadIconAsync(string path, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var stream = file.OpenReadStream();
            var mimeTypes = _contentInspector.Inspect(stream).ByMimeType();
            var bestMatchMimeType = mimeTypes.MaxBy(e => e.Points)?.MimeType;
            if (string.IsNullOrEmpty(bestMatchMimeType) || !bestMatchMimeType.StartsWith(_supportedIconMime))
                return new UnsupportedMediaTypeResult();

            var fileExtension = bestMatchMimeType.Split("/").Last();
            string? filename = await _fileUploaderService.UploadFileAsync(path, stream, $".{fileExtension}");
            return filename == null ? BadRequest("Failed to upload the file") : Ok();
        }
    }
}