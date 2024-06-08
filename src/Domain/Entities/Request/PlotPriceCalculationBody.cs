using System.ComponentModel.DataAnnotations;

using stela_api.src.Domain.Enums;

namespace stela_api.src.Domain.Entities.Request
{
    public class PlotPriceCalculationBody
    {
        [EnumDataType(typeof(AdditionalService))]
        public AdditionalService AdditionalService { get; set; }

        [EnumDataType(typeof(GraniteColor))]
        public GraniteColor GraniteColor { get; set; }

        [EnumDataType(typeof(PlotSize))]
        public PlotSize PlotSize { get; set; }
    }
}

