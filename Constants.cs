namespace stela_api
{
    public static class Constants
    {
        public static readonly string ServerUrl = Environment.GetEnvironmentVariable("ServerUrl", EnvironmentVariableTarget.User) ?? throw new Exception("ServerUrl is not found in enviroment variables");

        public static readonly string LocalPathToStorages = @"Resources/";
        public static readonly string LocalPathToProfileIcons = $"{LocalPathToStorages}ProfileIcons/";

        public static readonly string WebPathToProfileIcons = $"{ServerUrl}/stela_api/upload/profileIcon/";
    }
}