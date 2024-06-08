using Microsoft.AspNetCore.Mvc;

using stela_api.src.App.IService;
using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.Entities.Response;
using stela_api.src.Domain.Enums;

namespace stela_api.src.App.Service
{
    public class PlotPriceCalculationService : IPlotPriceCalculationService
    {
        public IActionResult Invoke(PlotPriceCalculationBody plotPriceCalculationBody)
        {
            var valuationCheck = new PlotValuationCheckBody
            {
                PlotSizePrice = plotPriceCalculationBody.PlotSize.GetCost(),
                GraniteColorPrice = plotPriceCalculationBody.GraniteColor.GetCost(),
                AdditionalServicePrice = plotPriceCalculationBody.AdditionalService.GetServiceInfo()?.Cost ?? 0
            };
            valuationCheck.TotalPrice = valuationCheck.PlotSizePrice
                + valuationCheck.GraniteColorPrice
                + valuationCheck.AdditionalServicePrice;

            return new OkObjectResult(valuationCheck);
        }
    }
}

