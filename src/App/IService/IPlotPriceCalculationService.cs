using Microsoft.AspNetCore.Mvc;

using stela_api.src.Domain.Entities.Request;

namespace stela_api.src.App.IService
{
    public interface IPlotPriceCalculationService
    {
        Task<IActionResult> Invoke(PlotPriceCalculationBody plotPriceCalculationBody);
    }
}

