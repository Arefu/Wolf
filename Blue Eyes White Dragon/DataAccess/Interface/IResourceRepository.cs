using System.IO;

namespace Blue_Eyes_White_Dragon.DataAccess.Interface
{
    public interface IResourceRepository
    {
        DirectoryInfo GetPendulumPathFromResource();
        FileInfo LoadErrorImageFromResource();
        DirectoryInfo GetOutputPath();
    }
}