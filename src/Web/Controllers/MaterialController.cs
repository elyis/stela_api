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
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialRepository _materialRepository;
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

        public MaterialController(
            IMaterialRepository materialRepository,
            IFileUploaderService fileUploaderService)
        {
            _materialRepository = materialRepository;
            _fileUploaderService = fileUploaderService;
        }

        [SwaggerOperation("Создать материал")]
        [SwaggerResponse(200)]
        [SwaggerResponse(409)]

        [HttpPost("material"), Authorize]
        public async Task<IActionResult> CreateMaterial(CreateMemorialMaterialBody body)
        {
            var result = await _materialRepository.AddMaterial(body);
            return result == null ? Conflict() : Ok();
        }

        [SwaggerOperation("Получить материал по идентификатору")]
        [SwaggerResponse(200, Type = typeof(MemorialMaterialBody))]
        [SwaggerResponse(404)]

        [HttpGet("material")]
        public async Task<IActionResult> GetMaterialById(
            [FromQuery, Required] Guid key)
        {
            var result = await _materialRepository.GetMaterialById(key);
            return result == null ? NotFound() : Ok(result.ToMemorialMaterialBody());
        }

        [SwaggerOperation("Получить список материалов")]
        [SwaggerResponse(200, Type = typeof(PaginationResponse<MemorialMaterialBody>))]

        [HttpGet("materials")]
        public async Task<IActionResult> GetMaterials(
            [FromQuery] int count = 10,
            [FromQuery] int offset = 0)
        {
            var result = await _materialRepository.GetMaterials(count, offset);
            var countItems = await _materialRepository.GetMaterialCount();
            return Ok(new PaginationResponse<MemorialMaterialBody>
            {
                Count = count,
                Offset = offset,
                Total = countItems,
                Items = result.Select(e => e.ToMemorialMaterialBody())
            });
        }

        [HttpPost("upload/material"), Authorize]
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

        [HttpGet("upload/material/{filename}")]
        [SwaggerOperation("Получить иконку материала")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(File))]
        [SwaggerResponse(404, Description = "Неверное имя файла")]

        public async Task<IActionResult> GetMaterialImage(string filename)
            => await _fileUploaderService.GetStreamFileAsync(Constants.LocalPathToMaterialImages, filename);

    }
}