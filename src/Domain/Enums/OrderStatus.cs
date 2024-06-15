using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace stela_api.src.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrderStatus
    {
        Paid,
        InProcess,
        Completed
    }
}