using System;
using System.Configuration;

namespace Blue_Eyes_White_Dragon.Utility
{
    public static class Constants
    {
        public static string GameImagesLocation => ConfigurationManager.AppSettings["GameImagesLocation"];
        public static string ReplacementImagesLocation => ConfigurationManager.AppSettings["ReplacementImagesLocation"];
        public static string Jpg => Enum.GetName(typeof(Constants.SupportedImageTypes), Constants.SupportedImageTypes.jpg);
        public static string Png => Enum.GetName(typeof(Constants.SupportedImageTypes), Constants.SupportedImageTypes.png);
        public static string ResourcePendulumLocation = "Resources\\Pendulums";

        public static string BtnTextOk = "Ok";

        public static string BtnTextCustom = "Open Art Picker";
        public static string ErrorImageName = "error.bmp";
        public static int ConsoleLimit = 20000;
        public static string ArtworkMatchFileName = "ArtworkMatch.json";
        public static readonly string StringError = "Internal Error";

        public enum SupportedImageTypes
        {
            jpg,
            png,
        }
    }
}
