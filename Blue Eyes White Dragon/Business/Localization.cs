namespace Blue_Eyes_White_Dragon.Business
{
    public static class Localization
    {
        public static readonly string ErrorEnterPath = "Please enter a path to the file";
        public static readonly string ErrorFileNotExist = "The file does not exist";
        public static string ErrorFileEmptyOrInvalid = "Error - the file is empty or invalid";
        public static string InformationLoadComplete(string path) { return $"Load successful from {path}"; }
        public static string InformationSaveComplete(string path) { return $"Save successful to {path}"; }
        public static string ErrorUnsupportedFileType(string fileType) { return $"Error can not calculate height and width for file type {fileType}"; }
        public static string ErrorCalculateNoMatch(string artworkGameImageMonsterName) { return $"Error can not calculate height and width for {artworkGameImageMonsterName} without a match"; }
    }
}
