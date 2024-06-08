using Microsoft.AspNetCore.Mvc;

using stela_api.src.App.IService;
using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.IRepository;

namespace stela_api.src.App.Service
{
    public class CreateMemorialService : ICreateMemorialService
    {
        private readonly IMemorialRepository _memorialRepository;
        private readonly IMaterialRepository _materialRepository;

        public CreateMemorialService(
            IMemorialRepository memorialRepository,
            IMaterialRepository materialRepository)
        {
            _memorialRepository = memorialRepository;
            _materialRepository = materialRepository;
        }

        public async Task<IActionResult> Invoke(CreateMemorialBody body)
        {
            var materials = await _materialRepository.GetMaterials(body.MaterialIds);
            if (!materials.Any())
                return new BadRequestObjectResult(new { message = "Materials not found" });

            var result = await _memorialRepository.CreateMemorial(body, materials);
            return result == null ? new BadRequestResult() : new OkObjectResult(result.ToMemorialBody());
        }
    }
}

