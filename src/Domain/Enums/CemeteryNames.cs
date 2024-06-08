using System.ComponentModel;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace stela_api.src.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CemeteryNames
    {
        Old,
        MemorialOrenburg_Salmyshskaya,
        MuslimMilitary,
        ComplexStepnoi1,
        Orthodox,
        MemorialOrenburg_Chicherina,
        ComplexStepnoi2,
        ComplexStepnoi3,
        ComplexStepnoi4,
        Cemetery_Ivanovka,
        Cemetery_Voskresenovka,
        Muslim_Berdyanka,
        Muslim_Chebenki,
        Muslim_SakmaraDistrict,
        Muslim,
        Pluto,
        Sulak,
        Nikolskoye,
        MassGraveOfRepressedPeople,
        Verkhnebekenskoye,
    }

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? ((DescriptionAttribute)attributes[0]).Description : value.ToString();
        }
    }
}