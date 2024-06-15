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
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMemorialRepository _memorialRepository;
        private readonly IJwtService _jwtService;

        public OrderController(
            IOrderRepository orderRepository,
            IAccountRepository accountRepository,
            IMemorialRepository memorialRepository,
            IJwtService jwtService)
        {
            _orderRepository = orderRepository;
            _accountRepository = accountRepository;
            _memorialRepository = memorialRepository;
            _jwtService = jwtService;
        }

        [SwaggerOperation("Получить список заказов пользователей")]
        [SwaggerResponse(200, "Список заказов пользователей получен", typeof(PaginationResponse<OrderBody>))]
        [SwaggerResponse(403, "Недостаточно прав")]

        [HttpGet("orders"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrders(
            [FromQuery] int count = 10,
            [FromQuery] int offset = 0,
            [FromQuery] bool isOrderByDescending = true)
        {
            var orders = await _orderRepository.GetOrders(count, offset, isOrderByDescending);
            var ordersCount = await _orderRepository.GetOrdersCount();
            return Ok(new PaginationResponse<OrderBody>
            {
                Items = orders,
                Total = ordersCount,
                Count = count,
                Offset = offset
            });
        }


        [SwaggerOperation("Получить список заказов пользователя")]
        [SwaggerResponse(200, "Список заказов пользователя получен", typeof(PaginationResponse<OrderBody>))]
        [SwaggerResponse(403, "Недостаточно прав")]

        [HttpGet("orders/me"), Authorize]
        public async Task<IActionResult> GetOrdersByClientId(
            [FromHeader(Name = "Authorization")] string token,
            [FromQuery] int count = 10,
            [FromQuery] int offset = 0,
            [FromQuery] bool isOrderByDescending = true)
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);
            var orders = await _orderRepository.GetOrdersByClientId(tokenPayload.UserId, count, offset, isOrderByDescending);

            var ordersCount = await _orderRepository.GetOrdersByClientIdCount(tokenPayload.UserId);
            return Ok(new PaginationResponse<OrderBody>
            {
                Items = orders,
                Total = ordersCount,
                Count = count,
                Offset = offset
            });
        }

        [SwaggerOperation("Создать заказ")]
        [SwaggerResponse(200, "Заказ создан", typeof(OrderBody))]
        [SwaggerResponse(400, "Некорректные данные")]

        [HttpPost("order"), Authorize]
        public async Task<IActionResult> CreateOrder(
            [FromHeader(Name = "Authorization")] string token,
            CreateOrderBody body)
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);
            var account = await _accountRepository.GetById(tokenPayload.UserId);
            var memorial = await _memorialRepository.GetMemorialById(body.MemorialId);

            if (account == null || memorial == null)
                return BadRequest();

            var order = await _orderRepository.CreateOrder(account, memorial);
            return Ok(order.ToOrderBody());
        }

        [SwaggerOperation("Удалить заказ"), Authorize]
        [SwaggerResponse(204, "Заказ удален")]
        [SwaggerResponse(404, "Заказ не найден")]
        [SwaggerResponse(403, "Недостаточно прав")]

        [HttpDelete("order/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(
            [FromHeader(Name = "Authorization")] string token,
            [FromRoute] Guid id)
        {
            var result = await _orderRepository.DeleteOrder(id);
            return result ? NoContent() : NotFound();
        }

        [SwaggerOperation("Изменить детали заказа"), Authorize]
        [SwaggerResponse(200, "Детали заказа изменены", typeof(OrderBody))]
        [SwaggerResponse(400, "Некорректный идентификатор")]
        [SwaggerResponse(403, "Недостаточно прав")]

        [HttpPatch("order"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrder(OrderPatchBody body)
        {
            var result = await _orderRepository.UpdateOrder(body);
            return result == null ? BadRequest() : Ok(result.ToOrderBody());
        }
    }
}