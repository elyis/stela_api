using System.ComponentModel.DataAnnotations;
using System.Net;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.Entities.Response;
using stela_api.src.Domain.IRepository;

using Swashbuckle.AspNetCore.Annotations;

using webApiTemplate.src.App.IService;

namespace stela_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class AdditionalServiceController : ControllerBase
    {
        private readonly IAdditionalServiceRepository _additionalServiceRepository;
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

        public AdditionalServiceController(
            IAdditionalServiceRepository additionalServiceRepository,
            IFileUploaderService fileUploaderService)
        {
            _additionalServiceRepository = additionalServiceRepository;
            _fileUploaderService = fileUploaderService;
        }

        [SwaggerOperation("Получить список дополнительных услуг")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<AdditionalServiceBody>))]

        [HttpGet("additional-services")]
        public async Task<IActionResult> GetAdditionalServices(
            [FromQuery] int count = 10,
            [FromQuery] int offset = 0)
        {
            var services = await _additionalServiceRepository.GetAll(count, offset);
            var response = services.Select(e => e.ToAdditionalServiceBody());
            return Ok(response);
        }

        [SwaggerOperation("Создать дополнительную услугу")]
        [SwaggerResponse(200, Type = typeof(AdditionalServiceBody))]
        [SwaggerResponse(409, "Данная услуга уже существует")]

        [HttpPost("additional-service"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAdditionalService(CreateAdditionalServiceBody body)
        {
            var service = await _additionalServiceRepository.CreateService(body);
            return service == null ? Conflict() : Ok(service.ToAdditionalServiceBody());
        }


        [SwaggerOperation("Удалить дополнительную услугу")]
        [SwaggerResponse(204)]
        [SwaggerResponse(404, "Услуга не найдена")]

        [HttpDelete("additional-service"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAdditionalService([FromQuery, Required] Guid key)
        {
            var service = await _additionalServiceRepository.DeleteService(key);
            return service ? NoContent() : NotFound();
        }

        [SwaggerOperation("Получить услугу по имени")]
        [SwaggerResponse(200, Type = typeof(AdditionalServiceBody))]
        [SwaggerResponse(404, "Услуга не найдена")]

        [HttpGet("additional-service/name")]
        public async Task<IActionResult> GetAdditionalServiceByName([FromQuery, Required] string key)
        {
            var service = await _additionalServiceRepository.GetByName(key);
            return service == null ? NotFound() : Ok(service.ToAdditionalServiceBody());
        }

        [SwaggerOperation("Получить услугу по идентификатору")]
        [SwaggerResponse(200, Type = typeof(AdditionalServiceBody))]
        [SwaggerResponse(404, "Услуга не найдена")]

        [HttpGet("additional-service/id")]
        public async Task<IActionResult> GetAdditionalServiceByName([FromQuery, Required] Guid key)
        {
            var service = await _additionalServiceRepository.GetById(key);
            return service == null ? NotFound() : Ok(service.ToAdditionalServiceBody());
        }

        [HttpPost("upload/additional-service"), Authorize]
        [SwaggerOperation("Загрузить иконку дополнительной услуги")]
        [SwaggerResponse(200, Description = "Успешно")]
        public async Task<IActionResult> UploadMemorialImage(
            [FromHeader(Name = nameof(HttpRequestHeader.Authorization))] string token,
            [FromForm, Required] IFormFile file,
            [FromQuery, Required] Guid key
        )
        {
            var additionalService = await _additionalServiceRepository.GetById(key);
            if (additionalService == null)
                return NotFound();

            var response = await _fileUploaderService.UploadFileAsync(Constants.LocalPathToMemorialImages, file.OpenReadStream(), _supportedImageExtensions);

            if (response is OkObjectResult result)
            {
                var filename = (string)result.Value;
                await _additionalServiceRepository.UpdateImage(key, filename);
            }
            return response;
        }

        [SwaggerOperation("Получить иконку дополнительной услуги")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(File))]
        [SwaggerResponse(404, Description = "Неверное имя файла")]

        [HttpGet("upload/additional-service/{filename}")]
        public async Task<IActionResult> GetMemorialImage(string filename)
            => await _fileUploaderService.GetStreamFileAsync(Constants.LocalPathToAdditionalServiceImages, filename);

    }
}

