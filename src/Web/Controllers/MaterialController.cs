using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.Entities.Response;
using stela_api.src.Domain.IRepository;

using Swashbuckle.AspNetCore.Annotations;

namespace stela_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialRepository _materialRepository;

        public MaterialController(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
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

        // [SwaggerOperation("Получить материал по имени")]
        // [SwaggerResponse(200, Type = typeof(MemorialMaterialBody))]
        // [SwaggerResponse(404)]

        // [HttpGet("material/name")]
        // public async Task<IActionResult> GetMaterialByName(
        //     [FromQuery, Required] string key)
        // {
        //     var result = await _materialRepository.GetMaterialByName(key);
        //     return result == null ? NotFound() : Ok(result.ToMemorialMaterialBody());
        // }

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

    }
}