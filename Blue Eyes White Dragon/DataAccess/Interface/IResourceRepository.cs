using System.IO;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.DataAccess.Interface
{
    public interface IResourceRepository
    {
        DirectoryInfo GetPendulumPathFromResource();
        FileInfo LoadErrorImageFromResource();
        DirectoryInfo GetOutputPath();
    }
}