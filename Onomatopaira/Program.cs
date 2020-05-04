﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Celtic_Guardian;

namespace Onomatopaira
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] Args)
        {
            var UseArgs = false;
            var TocFileLocation = "";

            if (Args.Length > 0)
            {
                TocFileLocation = Args.FirstOrDefault(File.Exists);
                if (TocFileLocation != null)
                    UseArgs = true;
                else
                    Utilities.Log("Coun't Find TOC File.", Utilities.Event.Warning);
            }

            Console.Title = "Onomatopaira";

            using (var FileDialog = new OpenFileDialog())
            {
                FileDialog.Title = "Open Yu-Gi-Oh TOC File...";
                FileDialog.Filter = "Yu-Gi-Oh! Wolf TOC File |*.toc";
                if (UseArgs == false)
                {
                    if (FileDialog.ShowDialog() != DialogResult.OK) return;
                    TocFileLocation = FileDialog.FileName;
                }

                try
                {
                    using (var reader = new StreamReader(TocFileLocation))
                    {
                        if (!File.Exists(TocFileLocation.Replace(".toc", ".dat")))
                            Utilities.Log("Can't Find DAT File.", Utilities.Event.Error, true, 1);
                        var datReader =
                            new BinaryReader(File.Open(TocFileLocation.Replace(".toc", ".dat"), FileMode.Open));
                        reader.ReadLine();

                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            if (line == null) continue;

                            line = line.TrimStart(' ');
                            line = Regex.Replace(line, @"  +", " ", RegexOptions.Compiled);
                            var Data = new FileInformation(line.Split(new char[] { ' ' }, 3));

                            Utilities.Log(
                                $"Extracting File: {new FileInfo(Data.FileName).Name} ({Data.FileSize} Bytes)",
                                Utilities.Event.Information);

                            new FileInfo("YGO_DATA/" + Data.FileName).Directory?.Create();

                            var ExtraBytes = Utilities.HexToDec(Data.FileSize);
                            if (Utilities.HexToDec(Data.FileSize) % 4 != 0)
                                while (ExtraBytes % 4 != 0)
                                    ExtraBytes = ExtraBytes + 1;

                            using (var FileWriter = new BinaryWriter(File.Open("YGO_DATA/" + Data.FileName,
                                FileMode.Create, FileAccess.Write)))
                            {
                                FileWriter.Write(datReader.ReadBytes(Utilities.HexToDec(Data.FileSize)));
                                FileWriter.Flush();
                            }

                            datReader.BaseStream.Position += ExtraBytes - Utilities.HexToDec(Data.FileSize);
                        }
                    }
                }
                catch (Exception Ex)
                {
                    Utilities.Log($"Exception Caught: {Ex.Message}", Utilities.Event.Error, true, 1);
                }
            }
        }

        private struct FileInformation
        {
            public FileInformation(IReadOnlyList<string> Data)
            {
                if (Data.Count != 3 || Data.Count > 3)
                    Utilities.Log("Can't Parse TOC File.", Utilities.Event.Error, true, 1);

                FileSize = Data[0];
                //FileNameLength = Data[1];
                FileName = Data[2];
            }

            public readonly string FileSize;

            //public string FileNameLength;
            public readonly string FileName;
        }
    }
}