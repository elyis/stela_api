namespace stela_api.src.Domain.Entities.Shared.Utility
{
    public class CodeGenerator
    {
        public static string Generate()
        {
            var rnd = new Random();
            return rnd.Next(100_0, 100_000_0).ToString();
        }
    }
}