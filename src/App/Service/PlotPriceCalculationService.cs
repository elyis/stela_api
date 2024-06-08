using Microsoft.AspNetCore.Mvc;

using stela_api.src.App.IService;
using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.Entities.Response;
using stela_api.src.Domain.Enums;
using stela_api.src.Domain.IRepository;

namespace stela_api.src.App.Service
{
    public class PlotPriceCalculationService : IPlotPriceCalculationService
    {
        private readonly IAdditionalServiceRepository _additionalServiceRepository;

        public PlotPriceCalculationService(IAdditionalServiceRepository additionalServiceRepository)
        {
            _additionalServiceRepository = additionalServiceRepository;
        }

        public async Task<IActionResult> Invoke(PlotPriceCalculationBody plotPriceCalculationBody)
        {
            var additionalService = await _additionalServiceRepository.GetByName(plotPriceCalculationBody.AdditionalService);
            if (additionalService == null)
                return new BadRequestObjectResult("Услуга не найдена");


            var valuationCheck = new PlotValuationCheckBody
            {
                PlotSizePrice = plotPriceCalculationBody.PlotSize.GetCost(),
                GraniteColorPrice = plotPriceCalculationBody.GraniteColor.GetCost(),
                AdditionalServicePrice = additionalService.Price
            };

            valuationCheck.TotalPrice = valuationCheck.PlotSizePrice
                + valuationCheck.GraniteColorPrice
                + valuationCheck.AdditionalServicePrice;

            return new OkObjectResult(valuationCheck);
        }
    }
}

