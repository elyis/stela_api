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
    public class UserController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public UserController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [SwaggerOperation("Получить список пользователей")]
        [SwaggerResponse(200, Type = typeof(PaginationResponse<UserBody>))]
        [SwaggerResponse(401, Description = "Unauthorized")]
        [SwaggerResponse(403, Description = "Forbidden")]

        [HttpGet("users"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll(
            [FromQuery] int count = 10,
            [FromQuery] int offset = 0,
            [FromQuery] bool isOrderByAscending = true)
        {
            var users = await _accountRepository.GetAllAccounts(count, offset, isOrderByAscending);
            var result = users.Select(e => e.ToUserBody());
            var total = await _accountRepository.GetTotalAccounts();

            return Ok(new PaginationResponse<UserBody>
            {
                Items = result,
                Count = count,
                Offset = offset,
                Total = total
            });
        }

        [SwaggerOperation("Обновить поля пользователя")]
        [SwaggerResponse(200, Type = typeof(UserBody))]
        [SwaggerResponse(401, Description = "Unauthorized")]
        [SwaggerResponse(403, Description = "Forbidden")]
        [SwaggerResponse(404, Description = "Not found")]

        [HttpPatch("user"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UserPatchBody body)
        {
            var user = await _accountRepository.Update(body);
            return user == null ? NotFound() : Ok(user.ToUserBody());
        }

        [SwaggerOperation("Удалить пользователя")]
        [SwaggerResponse(204, Description = "No content")]
        [SwaggerResponse(401, Description = "Unauthorized")]
        [SwaggerResponse(403, Description = "Forbidden")]

        [HttpDelete("user/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([Required] Guid id)
        {
            var user = await _accountRepository.RemoveAccount(id);
            return NoContent();
        }
    }
}