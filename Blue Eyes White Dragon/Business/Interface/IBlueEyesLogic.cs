using BrightIdeasSoftware;

namespace Blue_Eyes_White_Dragon.Business.Interface
{
    public interface IBlueEyesLogic
    {
        void RunMatchAll();
        void RunSaveMatch();
        void RunLoadMatch(string path);
        void SavePathSetting(string filePath);
    }
}