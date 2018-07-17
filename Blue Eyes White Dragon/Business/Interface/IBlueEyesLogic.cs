using BrightIdeasSoftware;

namespace Blue_Eyes_White_Dragon.Business.Interface
{
    public interface IBlueEyesLogic
    {
        void RunMatchAll();
        object GameImageGetter(object row);
        object ReplacementImageGetter(object row);
        void RunSaveMatch();
        void RunLoadMatch(string path);
    }
}