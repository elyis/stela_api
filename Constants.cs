namespace stela_api
{
    public static class Constants
    {
        public static readonly string ServerUrl = Environment.GetEnvironmentVariable("ServerUrl") ?? throw new Exception("ServerUrl is not found in enviroment variables");

        public static readonly string LocalPathToStorages = @"Resources/";
        public static readonly string LocalPathToProfileIcons = $"{LocalPathToStorages}ProfileIcons/";
        public static readonly string LocalPathToMaterialImages = $"{LocalPathToStorages}MaterialImages/";
        public static readonly string LocalPathToMemorialImages = $"{LocalPathToStorages}MemorialImages/";

        public static readonly string WebPathToProfileIcons = $"{ServerUrl}/api/upload/profileIcon/";
        public static readonly string WebPathToMaterialImages = $"{ServerUrl}/api/upload/material/";
        public static readonly string WebPathToMemorialImages = $"{ServerUrl}/api/upload/memorial/";
    }
}

