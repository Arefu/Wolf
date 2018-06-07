using System.IO;

namespace Yu_Gi_Oh.File_Handling.Utility
{
    public class Dump_Settings
    {
        public Dump_Settings(string outputDirectory)
        {
            OutputDirectory = outputDirectory;

            if (!string.IsNullOrEmpty(OutputDirectory) && !Directory.Exists(OutputDirectory))
                Directory.CreateDirectory(OutputDirectory);

            Deep = true;
        }

        public string OutputDirectory { get; set; }
        public bool Deep { get; set; }
        public bool HumanReadable { get; set; }
    }
}