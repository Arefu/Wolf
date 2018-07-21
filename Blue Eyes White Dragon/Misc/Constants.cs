using System;
using System.Configuration;

namespace Blue_Eyes_White_Dragon.Misc
{
    public static class Constants
    {
        public static string GameImagesLocation => ConfigurationManager.AppSettings["GameImagesLocation"];
        public static string ReplacementImagesLocation => ConfigurationManager.AppSettings["ReplacementImagesLocation"];
        public static string Jpg => Enum.GetName(typeof(Constants.SupportedImageTypes), Constants.SupportedImageTypes.jpg);
        public static string Png => Enum.GetName(typeof(Constants.SupportedImageTypes), Constants.SupportedImageTypes.png);

        public static string ResourceLocation = "Resources";
        public static string ResourcePendulumLocation = "Pendulums";
        public static string ErrorImageName = "error.bmp";
        public static string ArtworkMatchFileName = "ArtworkMatch.json";

        public static int ConsoleLimit = 20000;
        public static readonly string StringError = "Internal Error";

        public enum SupportedImageTypes
        {
            jpg,
            png,
        }
    }
}
