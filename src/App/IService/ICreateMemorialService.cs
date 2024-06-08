using Microsoft.AspNetCore.Mvc;

using stela_api.src.Domain.Entities.Request;

namespace stela_api.src.App.IService
{
    public interface ICreateMemorialService
    {
        Task<IActionResult> Invoke(CreateMemorialBody body);
    }
}

