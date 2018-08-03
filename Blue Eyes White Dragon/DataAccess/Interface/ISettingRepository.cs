using Blue_Eyes_White_Dragon.Misc;

namespace Blue_Eyes_White_Dragon.DataAccess.Interface
{
    public interface ISettingRepository
    {
        void SavePathSetting(string filePath, Constants.Setting setting);
        string GetPathSetting(Constants.Setting setting);
    }
}