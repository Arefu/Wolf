namespace Blue_Eyes_White_Dragon.Misc
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
        public static string ErrorPendulumNotFound(string monsterName){ return $"Error could not find the image for the pendulum card {monsterName}"; } 

        public static string InformationLoadComplete(string path) { return $"Load successful from {path}"; }
        public static string InformationSaveComplete(string path) { return $"Save successful to {path}"; }
        public static string ErrorUnsupportedFileType(string fileType) { return $"Error can not calculate height and width for file type {fileType}"; }
        public static string ErrorCalculateNoMatch(string artworkGameImageMonsterName) { return $"Error can not calculate height and width for {artworkGameImageMonsterName} without a match"; }
        public static string InformationArtworkUpdated(string monsterName) { return $"Artwork have been changed for {monsterName}"; }
        public static string Exception(string exceptionMessage, string innerExceptionMessage) { return $"Exception: {exceptionMessage} \\r\\n InnerException: {innerExceptionMessage}"; }
        public static string ExceptionFormattedForConsole(string message) { return $"{message}{Localization.GetNewline}"; }
        public static string MessageFormattedForConsole(string message) { return $"{message}{Localization.GetNewline}"; }

        public static string InformationFoundPendulumImage(string artworkGameImageMonsterName, string imageFilePath)
        {
            return $"Found {imageFilePath} for {artworkGameImageMonsterName}";
        }

        public static string ErrorNoMatch(string monsterName)
        {
            return $"No match was found for {monsterName} - picking the error image";
        }

        public static string ErrorExecutableLocationNotFound = "Error loading the execution directory";
        public static string ErrorLoadingErrorImage = "Error loading the image for errors";

        public static string BtnTextOk = "Ok";
        public static string BtnTextCustom = "Open Art Picker";

        public static string ProcessingProgress(long progress, int totalAmount, string monsterName)
        {
            return $"{progress} of {totalAmount} processed - {monsterName}";
        }

        public static string ProcessingDone(int amountProcessed, long secondsElapsed)
        {
            return $"Processed {amountProcessed} in {secondsElapsed}s";
        }
    }
}
