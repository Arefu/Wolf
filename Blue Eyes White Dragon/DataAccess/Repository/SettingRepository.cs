using System;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.Misc;

namespace Blue_Eyes_White_Dragon.DataAccess.Repository
{
    public class SettingRepository : ISettingRepository
    {
        public void SavePathSetting(string filePath, Constants.Setting setting)
        {
            switch (setting)
            {
                case Constants.Setting.LastUsedLoadPath:
                    Properties.Settings.Default.LastUsedLoadPath = filePath;
                    break;
                case Constants.Setting.LastUsedGameImagePath:
                    Properties.Settings.Default.LastUsedGameImagePath = filePath;
                    break;
                case Constants.Setting.LastUsedReplacementImagePath:
                    Properties.Settings.Default.LastUsedReplacementImagePath = filePath;
                    break;
                case Constants.Setting.LastUsedCardDbPath:
                    Properties.Settings.Default.LasstUsedCardDbPath = filePath;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(setting), setting, null);
            }
            Properties.Settings.Default.Save();
        }

        public string GetPathSetting(Constants.Setting setting)
        {
            switch (setting)
            {
                case Constants.Setting.LastUsedLoadPath:
                    return Properties.Settings.Default.LastUsedLoadPath;
                case Constants.Setting.LastUsedGameImagePath:
                    return Properties.Settings.Default.LastUsedGameImagePath ;
                case Constants.Setting.LastUsedReplacementImagePath:
                    return Properties.Settings.Default.LastUsedReplacementImagePath;
                case Constants.Setting.LastUsedCardDbPath:
                    return Properties.Settings.Default.LasstUsedCardDbPath;
                default:
                    throw new ArgumentOutOfRangeException(nameof(setting), setting, null);
            }
        }
    }
}
