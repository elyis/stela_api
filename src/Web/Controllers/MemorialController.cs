using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using stela_api.src.App.IService;
using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.Entities.Response;
using stela_api.src.Domain.IRepository;

using Swashbuckle.AspNetCore.Annotations;

namespace stela_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class MemorialController : ControllerBase
    {
        private readonly ICreateMemorialService _createMemorialService;
        private readonly IMemorialRepository _memorialRepository;

        public MemorialController(
            ICreateMemorialService createMemorialService,
            IMemorialRepository memorialRepository)
        {
            _createMemorialService = createMemorialService;
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
    }
}

