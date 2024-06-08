using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace stela_api.src.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AdditionalService
    {
        FreeExpertVisit,
        CustomDesignProject,
        EngravingWorks,
        Delivery,
        InstallationAndLandscaping,
        AllServices
    }

    public class AdditionalServiceInfo
    {
        public string Name { get; }
        public float Cost { get; }

        public AdditionalServiceInfo(string name, float cost)
        {
            Name = name;
            Cost = cost;
        }
    }

    public static class AdditionalServiceExtensions
    {
        private static readonly Dictionary<AdditionalService, AdditionalServiceInfo> ServiceInfoMap = new()
    {
        { AdditionalService.FreeExpertVisit, new AdditionalServiceInfo("Бесплатный выезд эксперта для замера", 0) },
        { AdditionalService.CustomDesignProject, new AdditionalServiceInfo("Авторский дизайн-проект", 5000) },
        { AdditionalService.EngravingWorks, new AdditionalServiceInfo("Гравёрные работы", 3000) },
        { AdditionalService.Delivery, new AdditionalServiceInfo("Доставка", 1000) },
        { AdditionalService.InstallationAndLandscaping, new AdditionalServiceInfo("Монтаж и благоустройство", 7000) },
        { AdditionalService.AllServices, new AdditionalServiceInfo("Все услуги", 16000) }
    };

        public static AdditionalServiceInfo? GetServiceInfo(this AdditionalService service)
        {
            return ServiceInfoMap.TryGetValue(service, out var info) ? info : null;
        }
    }
}