using Microsoft.AspNetCore.Mvc;

using stela_api.src.App.IService;
using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.Entities.Response;

using Swashbuckle.AspNetCore.Annotations;

namespace stela_api.src.Web.Controllers
{
    [ApiController]
    [Route("api/tools")]
    public class ToolsController : ControllerBase
    {
        private readonly IPlotPriceCalculationService _plotPriceCalculationService;

        public ToolsController(IPlotPriceCalculationService plotPriceCalculationService)
        {
            _plotPriceCalculationService = plotPriceCalculationService;
        }

        [SwaggerOperation("Расчитать стоимость участка")]
        [SwaggerResponse(200, Type = typeof(PlotValuationCheckBody))]
        [SwaggerResponse(400)]

        [HttpPost("calculate-plot-price")]
        public IActionResult CalculatePlotPrice(PlotPriceCalculationBody plotPriceCalculationBody)
        {
            var result = _plotPriceCalculationService.Invoke(plotPriceCalculationBody);
            return Ok(result);
        }
    }
}

