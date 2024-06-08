using System.ComponentModel.DataAnnotations;
using System.Net;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using stela_api.src.App.IService;
using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.Entities.Response;
using stela_api.src.Domain.IRepository;

using Swashbuckle.AspNetCore.Annotations;

using webApiTemplate.src.App.IService;

namespace stela_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class MemorialController : ControllerBase
    {
        private readonly ICreateMemorialService _createMemorialService;
        private readonly IFileUploaderService _fileUploaderService;
        private readonly IMemorialRepository _memorialRepository;

        private readonly string[] _supportedImageExtensions = new string[]
        {
            "gif",
            "jpg",
            "jpeg",
            "jfif",
            "png",
            "svg"
        };

        public MemorialController(
            ICreateMemorialService createMemorialService,
            IFileUploaderService fileUploaderService,
            IMemorialRepository memorialRepository)
        {
            _createMemorialService = createMemorialService;
            _fileUploaderService = fileUploaderService;
            _memorialRepository = memorialRepository;
        }

        [SwaggerOperation("Создать памятник")]
        [SwaggerResponse(200, "Памятник создан", typeof(MemorialBody))]
        [SwaggerResponse(400, "Материалы не указаны")]

        [HttpPost("memorial"), Authorize]
        public async Task<IActionResult> CreateMemorial(CreateMemorialBody body)
        {
            var result = await _createMemorialService.Invoke(body);
            return Ok(result);
        }

        [SwaggerOperation("Получить список памятников")]
        [SwaggerResponse(200, "Список памятников получен", typeof(PaginationResponse<ShortMemorialBody>))]

        [HttpGet("memorials")]
        public async Task<IActionResult> GetMemorials(
            [FromQuery] int count = 10,
            [FromQuery] int offset = 0)
        {
            var memorials = await _memorialRepository.GetAllMemorials(count, offset);
            var result = memorials.Select(e => e.ToShortMemorialBody());
            var total = await _memorialRepository.GetMemorialsCount();
            return Ok(new PaginationResponse<ShortMemorialBody>
            {
                Count = count,
                Offset = offset,
                Total = total,
                Items = result
            });
        }

        [SwaggerOperation("Получить памятник по идентификатору")]
        [SwaggerResponse(200, Type = typeof(MemorialBody))]
        [SwaggerResponse(404, "Памятник не найден")]

        [HttpGet("memorial")]

        public async Task<IActionResult> GetMemorialById([FromQuery, Required] Guid id)
        {
            var memorial = await _memorialRepository.GetMemorialById(id);
            return memorial == null ? NotFound() : Ok(memorial.ToMemorialBody());
        }

        [HttpPost("upload/memorial"), Authorize]
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

        [HttpDelete("upload/memorial"), Authorize]
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

        [SwaggerOperation("Получить иконку памятника")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(File))]
        [SwaggerResponse(404, Description = "Неверное имя файла")]

        [HttpGet("upload/memorial/{filename}")]
        public async Task<IActionResult> GetMemorialImage(string filename)
            => await _fileUploaderService.GetStreamFileAsync(Constants.LocalPathToMemorialImages, filename);
    }
}

