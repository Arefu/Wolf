namespace Blue_Eyes_White_Dragon.Utility
{
    public static class Localization
    {
        public static readonly string ErrorEnterPath = "Please enter a path to the file";
        public static readonly string ErrorFileNotExist = "The file does not exist";
        public static string ErrorFileEmptyOrInvalid = "Error - the file is empty or invalid";
        public static string GetNewline = "\r\n";
        public static string InformationCalculatingImageDimensions = "Calculating image dimensions";
        public static string InformationCalculationComplete = "Calculating completed";
        public static string InformationLoading = "Loading";

        public static string InformationLoadComplete(string path) { return $"Load successful from {path}"; }
        public static string InformationSaveComplete(string path) { return $"Save successful to {path}"; }
        public static string ErrorUnsupportedFileType(string fileType) { return $"Error can not calculate height and width for file type {fileType}"; }
        public static string ErrorCalculateNoMatch(string artworkGameImageMonsterName) { return $"Error can not calculate height and width for {artworkGameImageMonsterName} without a match"; }
        public static string InformationArtworkUpdated(string monsterName) { return $"Artwork have been changed for {monsterName}"; }
        public static string Exception(string exceptionMessage, string innerExceptionMessage) { return $"Exception: {exceptionMessage} \\r\\n InnerException: {innerExceptionMessage}"; }
        public static string ExceptionFormattedForConsole(string message) { return $"{message}{Localization.GetNewline}"; }
        public static string MessageFormattedForConsole(string message) { return $"{message}{Localization.GetNewline}"; }
    }
}
