using System.Net;
using stela_api.src.Domain.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMemorialRepository _memorialRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IJwtService _jwtService;
        private readonly IFileUploaderService _fileUploaderService;
        private readonly string[] _supportedImageExtensions = new string[]
        {
            "gif",
            "jpg",
            "jpeg",
            "jfif",
            "png",
            "svg"
        };

        public UploadController(
            IAccountRepository accountRepository,
            IMemorialRepository memorialRepository,
            IMaterialRepository materialRepository,
            IJwtService jwtService,
            IFileUploaderService fileUploaderService)
        {
            _accountRepository = accountRepository;
            _memorialRepository = memorialRepository;
            _materialRepository = materialRepository;
            _jwtService = jwtService;
            _fileUploaderService = fileUploaderService;
        }

        [HttpPost("me"), Authorize]
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

        [HttpGet("me/{filename}")]
        [SwaggerOperation("Получить иконку профиля")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(File))]
        [SwaggerResponse(404, Description = "Неверное имя файла")]

        public async Task<IActionResult> GetProfileIcon(string filename)
            => await _fileUploaderService.GetStreamFileAsync(Constants.LocalPathToProfileIcons, filename);


        [HttpPost("memorial"), Authorize]
        [SwaggerOperation("Загрузить иконку памятника")]
        [SwaggerResponse(200, Description = "Успешно")]
        public async Task<IActionResult> UploadMemorialImage(
            [FromHeader(Name = nameof(HttpRequestHeader.Authorization))] string token,
            [FromForm, Required] IFormFile file,
            [FromQuery, Required] Guid memorialId
        )
        {
            var memorial = await _memorialRepository.GetMemorialById(memorialId);
            if (memorial == null)
                return NotFound();

            var response = await _fileUploaderService.UploadFileAsync(Constants.LocalPathToMemorialImages, file.OpenReadStream(), _supportedImageExtensions);

            if (response is OkObjectResult result)
            {
                var filename = (string)result.Value;
                await _memorialRepository.AddImage(memorialId, filename);
            }
            return response;
        }

        [HttpPost("material"), Authorize]
        [SwaggerOperation("Загрузить иконку материала")]
        [SwaggerResponse(200, Description = "Успешно")]
        public async Task<IActionResult> UploadMaterialImage(
            [FromHeader(Name = nameof(HttpRequestHeader.Authorization))] string token,
            [FromForm, Required] IFormFile file,
            [FromQuery, Required] Guid materialId
        )
        {
            var material = await _materialRepository.GetMaterialById(materialId);
            if (material == null)
                return NotFound();

            var response = await _fileUploaderService.UploadFileAsync(Constants.LocalPathToMaterialImages, file.OpenReadStream(), _supportedImageExtensions);

            if (response is OkObjectResult result)
            {
                var filename = (string)result.Value;
                await _materialRepository.UpdateMaterialImage(materialId, filename);
            }
            return response;
        }

        [HttpGet("material/{filename}")]
        [SwaggerOperation("Получить иконку материала")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(File))]
        [SwaggerResponse(404, Description = "Неверное имя файла")]

        public async Task<IActionResult> GetMaterialImage(string filename)
            => await _fileUploaderService.GetStreamFileAsync(Constants.LocalPathToMaterialImages, filename);


        [HttpDelete("memorial"), Authorize]
        [SwaggerOperation("Удалить иконку памятника")]
        [SwaggerResponse(204, Description = "Успешно")]
        [SwaggerResponse(404, Description = "Памятник не найден")]
        public async Task<IActionResult> RemoveMemorialImage(
            [FromHeader(Name = nameof(HttpRequestHeader.Authorization))] string token,
            [FromQuery, Required] Guid memorialId,
            [FromQuery, Required] string filename
        )
        {
            var result = await _memorialRepository.RemoveImage(memorialId, filename);
            return result == null ? new NotFoundResult() : new NoContentResult();
        }

        [HttpGet("memorial/{filename}")]
        [SwaggerOperation("Получить иконку памятника")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(File))]
        [SwaggerResponse(404, Description = "Неверное имя файла")]

        public async Task<IActionResult> GetMemorialImage(string filename)
            => await _fileUploaderService.GetStreamFileAsync(Constants.LocalPathToMemorialImages, filename);
    }
}

