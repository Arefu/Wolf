namespace Blue_Eyes_White_Dragon.Misc
{
    public static class Localization
    {
        public static readonly string ErrorEnterPath = "Please enter a path to the file";
        public static readonly string ErrorFileNotExist = "The file does not exist";
        public static string ErrorFileEmptyOrInvalid = "Error - the file is empty or invalid";
        public static string ErrorExecutableLocationNotFound = "Error loading the execution directory";
        public static string ErrorLoadingErrorImage = "Error loading the image for errors";
        public static string GetNewline = "\r\n";
        public static string InformationCalculatingImageDimensions = "Calculating image dimensions";
        public static string InformationCalculationComplete = "Calculating completed";
        public static string InformationLoading = "Loading";
        public static string InformationConvertingImages = "Converting images to jpg, quality 92 and subsampling 4x2x2";

        public static string InformationLoadComplete(string path) { return $"Load successful from {path}"; }
        public static string InformationSaveComplete(string path) { return $"Save successful to {path}"; }
        public static string InformationArtworkUpdated(string monsterName) { return $"Artwork have been changed for {monsterName}"; }
        public static string MessageFormattedForConsole(string message) { return $"{message}{Localization.GetNewline}"; }
        public static string InformationFoundPendulumImage(string artworkGameImageMonsterName, string imageFilePath) { return $"Found {imageFilePath} for {artworkGameImageMonsterName}"; }
        public static string InformationProcessingProgress(long progress, int totalAmount, string monsterName) { return $"{progress} of {totalAmount} processed - {monsterName}"; }
        public static string InformationProcessingDone(int amountProcessed, long secondsElapsed) { return $"Processed {amountProcessed} in {secondsElapsed}s"; }
        public static string InformationMultipleImagesFound(int imagesCount, int cardId, string imageFileName) { return $"{imagesCount} images found for {cardId} picking: {imageFileName}"; }

        public static string Exception(string exceptionMessage, string innerExceptionMessage) { return $"Exception: {exceptionMessage} \\r\\n InnerException: {innerExceptionMessage}"; }
        public static string ExceptionFormattedForConsole(string message) { return $"{message}{Localization.GetNewline}"; }

        public static string ErrorUnsupportedFileType(string fileType) { return $"Error can not calculate height and width for file type {fileType}"; }
        public static string ErrorCalculateNoMatch(string artworkGameImageMonsterName) { return $"Error can not calculate height and width for {artworkGameImageMonsterName} without a match"; }
        public static string ErrorPendulumNotFound(string monsterName) { return $"Error could not find the image for the pendulum card {monsterName}"; }
        public static string ErrorNoMatch(string monsterName) { return $"No match was found for {monsterName} - picking the error image"; }
        public static string ErrorNoImageFound(int cardId) { return $"no image was found for the card: {cardId}"; }
        public static string ErrorImageDoesNotExist(string path, string monstername) { return $"Error the file {path} does not exist for the monster {monstername}"; }
        public static string ErrorGameImagesMissing() { return "Error game images have not been extracted correctly, please extract them before running this tool"; }

        public static string BtnTextOk = "OK";
        public static string BtnTextCustom = "Open Art Picker";
        public static string BtnTextYes = "Yes";
        public static string BtnTextBlank = "";

    }
}
