using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Celtic_Guardian.LOTD_Files
{
    public class LOTD_Archive
    {
        public string InstallDirectory { get; private set; }
        public BinaryReader Reader { get; private set; }
        public LOTD_Directory Root { get; private set; }
        public bool WriteAccess { get; set; }

        public LOTD_Archive()
        {
            InstallDirectory = Utilities.GetInstallDir();
        }

        public LOTD_Archive(string installDirectory)
        {
            InstallDirectory = installDirectory;
        }

        public void Load()
        {
            if (string.IsNullOrEmpty(InstallDirectory))
                throw new DirectoryNotFoundException("Install Folder Not Found! CHECK CODE PLEASE!");

            if (!File.Exists(Path.Combine(InstallDirectory, "YGO_DATA.toc")) || !File.Exists(Path.Combine(InstallDirectory, "YGO_DATA.dat")))
                throw new FileNotFoundException("TOC/DAT Files Not Found! CHECK CODE PLEASE!");

            if (Reader != null)
            {
                Reader.Close();
                Reader = null;
            }

            Root = new LOTD_Directory
            {
                Archive = this,
                IsRoot = true
            };

            var filePaths = new List<string>();

            try
            {
                var Offset = 0L;
                var Lines = File.ReadAllLines(Path.Combine(InstallDirectory, "YGO_DATA.toc"));
                for (var Counter = 0; Counter < Lines.Length; Counter++)
                {
                    var CurrentLine = Lines[Counter];
                    if(CurrentLine.StartsWith("UT")) continue;

                    CurrentLine = CurrentLine.TrimStart(' ');
                    CurrentLine = Regex.Replace(CurrentLine, @"  +", " ", RegexOptions.Compiled);
                    //Root.AddFile
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}