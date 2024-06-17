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
    public class PortfolioController : ControllerBase
    {
        private readonly ICreatePortfolioMemorialService _createMemorialService;
        private readonly IFileUploaderService _fileUploaderService;
        private readonly IPortfolioMemorialRepository _memorialRepository;

        private readonly string[] _supportedImageExtensions = new string[]
        {
            "gif",
            "jpg",
            "jpeg",
            "jfif",
            "png",
            "svg"
        };

        public PortfolioController(
            ICreatePortfolioMemorialService createMemorialService,
            IFileUploaderService fileUploaderService,
            IPortfolioMemorialRepository portfolioMemorialRepository)
        {
            _createMemorialService = createMemorialService;
            _fileUploaderService = fileUploaderService;
            _memorialRepository = portfolioMemorialRepository;
        }

        [SwaggerOperation("Создать памятник")]
        [SwaggerResponse(200, "Памятник создан", typeof(PortfolioMemorialBody))]
        [SwaggerResponse(400, "Материалы не указаны")]

        [HttpPost("portfolio-memorial"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePortfolioMemorial(CreatePortfolioMemorialBody body)
        {
            var result = await _createMemorialService.Invoke(body);
            return Ok(result);
        }

        [SwaggerOperation("Получить список памятников")]
        [SwaggerResponse(200, "Список памятников получен", typeof(PaginationResponse<ShortPortfolioMemorialBody>))]

        [HttpGet("portfolio-memorials")]
        public async Task<IActionResult> GetPortfolioMemorials(
            [FromQuery] int count = 10,
            [FromQuery] int offset = 0)
        {
            var memorials = await _memorialRepository.GetAllPortfolioMemorials(count, offset);
            var result = memorials.Select(e => e.ToShortPortfolioMemorialBody());
            var total = await _memorialRepository.GetPortfolioMemorialsCount();
            return Ok(new PaginationResponse<ShortPortfolioMemorialBody>
            {
                Count = count,
                Offset = offset,
                Total = total,
                Items = result
            });
        }

        [SwaggerOperation("Получить памятник по идентификатору")]
        [SwaggerResponse(200, Type = typeof(PortfolioMemorialBody))]
        [SwaggerResponse(404, "Памятник не найден")]

        [HttpGet("portfolio-memorial")]

        public async Task<IActionResult> GetPortfolioMemorialById([FromQuery, Required] Guid id)
        {
            var memorial = await _memorialRepository.GetPortfolioMemorialById(id);
            return memorial == null ? NotFound() : Ok(memorial.ToPortfolioMemorialBody());
        }

        [HttpPost("upload/portfolio-memorial"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Загрузить иконку памятника")]
        [SwaggerResponse(200, Description = "Успешно")]
        public async Task<IActionResult> UploadMemorialImage(
            [FromHeader(Name = nameof(HttpRequestHeader.Authorization))] string token,
            [FromForm, Required] IFormFile file,
            [FromQuery, Required] Guid memorialId
        )
        {
            var memorial = await _memorialRepository.GetPortfolioMemorialById(memorialId);
            if (memorial == null)
                return NotFound();

            var response = await _fileUploaderService.UploadFileAsync(Constants.LocalPathToPortfolioMemorialImages, file.OpenReadStream(), _supportedImageExtensions);

            if (response is OkObjectResult result)
            {
                var filename = (string)result.Value;
                await _memorialRepository.AddImage(memorialId, filename);
            }
            return response;
        }

        [HttpDelete("upload/portfolio-memorial"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Удалить иконку памятника")]
        [SwaggerResponse(204, Description = "Успешно")]
        public async Task<IActionResult> RemoveMemorialImage(
            [FromHeader(Name = nameof(HttpRequestHeader.Authorization))] string token,
            [FromQuery, Required] Guid memorialId,
            [FromQuery, Required] string filename
        )
        {
            await _memorialRepository.RemoveImage(memorialId, filename);
            return NoContent();
        }

        [SwaggerOperation("Получить иконку памятника")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(File))]
        [SwaggerResponse(404, Description = "Неверное имя файла")]

        [HttpGet("upload/portfolio-memorial/{filename}")]
        public async Task<IActionResult> GetPortfolioMemorialImage(string filename)
            => await _fileUploaderService.GetStreamFileAsync(Constants.LocalPathToPortfolioMemorialImages, filename);
    }
}