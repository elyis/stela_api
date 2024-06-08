using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Headers;

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
    public class BusketController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMemorialRepository _memorialRepository;
        private readonly IBusketRepository _busketRepository;
        private readonly IJwtService _tokenService;

        public BusketController(
            IAccountRepository accountRepository,
            IMemorialRepository memorialRepository,
            IBusketRepository busketRepository,
            IJwtService tokenService)
        {
            _accountRepository = accountRepository;
            _memorialRepository = memorialRepository;
            _busketRepository = busketRepository;
            _tokenService = tokenService;
        }

        [SwaggerOperation("Добавить элемент в корзину")]
        [SwaggerResponse(200, Type = typeof(BusketItemBody))]
        [SwaggerResponse(400, "Memorial id is not valid")]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(409, "Busket item already exists")]

        [HttpPost("busket-item"), Authorize]
        public async Task<IActionResult> AddToBusket(
            CreateBusketItemBody body,
            [FromHeader(Name = nameof(HttpRequestHeaders.Authorization))] string token)
        {
            var tokenPayload = _tokenService.GetTokenPayload(token);

            var memorial = await _memorialRepository.GetMemorialById(body.MemorialId);
            if (memorial == null)
                return BadRequest("Memorial id is not valid");

            var busket = await _busketRepository.GetBusketByAccountId(tokenPayload.UserId);

            var busketItem = await _busketRepository.AddBusketItem(busket.Id, memorial, body.Quantity);
            return busketItem == null ? Conflict("Busket item already exists") : Ok(busketItem.ToBusketItemBody());
        }

        [SwaggerOperation("Обновить количество элемента в корзине")]
        [SwaggerResponse(200, Type = typeof(BusketItemBody))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(404, "Busket item not found")]

        [HttpPatch("busket-item"), Authorize]
        public async Task<IActionResult> UpdateBusketItem(UpdateBusketItemBody body)
        {
            var busketItem = await _busketRepository.UpdateBusketItem(body.BusketItemId, body.Quantity);
            return busketItem == null ? NotFound("Busket item not found") : Ok(busketItem.ToBusketItemBody());
        }

        [SwaggerOperation("Удалить элемент из корзины")]
        [SwaggerResponse(204, Type = typeof(BusketItemBody))]
        [SwaggerResponse(401, "Unauthorized")]

        [HttpDelete("busket-item"), Authorize]
        public async Task<IActionResult> DeleteBusketItem([FromQuery, Required] Guid busketItemId)
        {
            _ = await _busketRepository.RemoveBusketItem(busketItemId);
            return NoContent();
        }

        [SwaggerOperation("Получить элементы корзины")]
        [SwaggerResponse(200, Type = typeof(PaginationResponse<BusketItemBody>))]
        [SwaggerResponse(401, "Unauthorized")]

        [HttpGet("busket-items"), Authorize]
        public async Task<IActionResult> GetItems(
            [FromHeader(Name = nameof(HttpRequestHeaders.Authorization))] string token,
            [FromQuery] int count = 10,
            [FromQuery] int offset = 0)
        {
            var tokenPayload = _tokenService.GetTokenPayload(token);

            var busket = await _busketRepository.GetBusketByAccountId(tokenPayload.UserId);
            var busketItems = await _busketRepository.GetItemsByBusketId(busket.Id, count, offset);
            return Ok(new PaginationResponse<BusketItemBody>
            {
                Count = count,
                Items = busketItems.Select(e => e.ToBusketItemBody()),
                Total = await _busketRepository.GetCountItemsByBusketId(busket.Id),
                Offset = offset
            });
        }
    }
}

