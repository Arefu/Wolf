using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.File_Handling.LOTD_Files
{
    public class LOTD_Directory
    {
        private string _name;
        private LOTD_Directory _parent;

        public LOTD_Directory()
        {
            Files = new Dictionary<string, LOTD_File>(StringComparer.OrdinalIgnoreCase);
            Directories = new Dictionary<string, LOTD_Directory>(StringComparer.OrdinalIgnoreCase);
        }

        public LOTD_Archive Archive { get; set; }
        public Dictionary<string, LOTD_File> Files { get; }
        public Dictionary<string, LOTD_Directory> Directories { get; }
        public bool IsRoot { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                Parent?.Directories.Remove(_name);

                _name = value;

                Parent?.Directories.Add(_name, this);
            }
        }

        public string FullName
        {
            get
            {
                var name = Name ?? string.Empty;
                string parentName = null;
                if (Parent != null && !Parent.IsRoot) parentName = Parent.FullName;
                return parentName == null ? name : Path.Combine(parentName, name);
            }
        }

        public LOTD_Directory Parent
        {
            get => _parent;
            set
            {
                if (_parent == value) return;
                _parent?.Directories.Remove(Name);

                _parent = value;

                if (_parent != null)
                {
                    _parent.Directories.Add(Name, this);
                    Archive = _parent.Archive;
                }
                else
                {
                    Archive = null;
                }
            }
        }

        public void Dump(string outputDirectory)
        {
            Dump(new Dump_Settings(outputDirectory));
        }

        public void Dump(Dump_Settings Settings)
        {
            foreach (var Folder in Directories)
                Folder.Value.Dump(Settings);
            foreach (var File in Files)
                File.Value.Dump(Settings);
        }

        public void CreateDirectory(string Path)
        {
            var Directory = ResolveDirectory(Path, false, true);
            if (Directory == null)
                throw new IOException("Path Not Found! CHECK CODE PLEASE!");
        }

        public bool DirectoryExists(string Path)
        {
            return FindDirectory(Path, false) != null;
        }

        public LOTD_Directory FindDirectory(string Path)
        {
            return FindDirectory(Path, false);
        }

        public LOTD_Directory FindDirectory(string Path, bool isFilePath)
        {
            return ResolveDirectory(Path, false, false);
        }

        public bool FileExists(string Path)
        {
            return FindFile(Path) != null;
        }

        public LOTD_File FindFile(string path)
        {
            var splitted = SplitPath(path);
            if (splitted.Length <= 0) return null;
            var directory = ResolveDirectory(splitted, true, false);
            if (directory != null && directory.Files.TryGetValue(splitted[splitted.Length - 1], out var file))
                return file;

            return null;
        }

        public List<LOTD_File> GetAllFiles()
        {
            var Files = new List<LOTD_File>();
            GetAllFiles(Files);
            return Files;
        }

        private void GetAllFiles(ICollection<LOTD_File> files)
        {
            foreach (var file in Files.Values)
                files.Add(file);
            foreach (var directory in Directories.Values)
                directory.GetAllFiles(files);
        }

        public LOTD_Directory ResolveDirectory(string path, bool isFilePath, bool create)
        {
            return ResolveDirectory(SplitPath(path), isFilePath, create);
        }

        private LOTD_Directory ResolveDirectory(IReadOnlyList<string> path, bool isFilePath, bool create)
        {
            var directory = this;

            var pathCount = path.Count + (isFilePath ? -1 : 0);
            for (var i = 0; i < pathCount; i++)
                if (path[i] == "..")
                {
                    directory = directory.Parent;
                    if (directory == null) return null;
                }
                else
                {
                    if (directory.Directories.TryGetValue(path[i], out var subDir))
                    {
                        directory = subDir;
                    }
                    else
                    {
                        subDir = new LOTD_Directory
                        {
                            Name = path[i],
                            Parent = directory
                        };
                        directory = subDir;
                    }
                }

            return directory;
        }

        private static string[] SplitPath(string path)
        {
            return path.Split(new[] {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar},
                StringSplitOptions.RemoveEmptyEntries);
        }

        private static bool IsSameOrSubDirectory(string BasePath, string Path)
        {
            var dirInfoPath = new DirectoryInfo(System.IO.Path.GetFullPath(Path).TrimEnd('\\', '/'));
            var DirInfoBasePath = new DirectoryInfo(System.IO.Path.GetFullPath(BasePath).TrimEnd('\\', '/'));

            var SubDirectory = "";
            while (dirInfoPath != null)
            {
                if (dirInfoPath.FullName.Equals(DirInfoBasePath.FullName, StringComparison.OrdinalIgnoreCase))
                    return true;

                SubDirectory = string.IsNullOrEmpty(SubDirectory)
                    ? dirInfoPath.Name
                    : System.IO.Path.Combine(dirInfoPath.Name, SubDirectory);
                dirInfoPath = dirInfoPath.Parent;
            }

            return false;
        }

        public LOTD_File[] AddFilesOnDisk(string directory, string rootDir, bool recursive)
        {
            var files = new List<LOTD_File>();
            if (!Directory.Exists(directory)) return files.ToArray();

            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            files.AddRange(Directory.GetFiles(directory, "*.*", searchOption)
                .Select(filePath => AddFileOnDisk(filePath, rootDir)).Where(file => file != null));

            return files.ToArray();
        }

        public LOTD_File AddFileOnDisk(string filePath, string rootDir)
        {
            if (!File.Exists(filePath)) return null;

            var relativePath = GetRelativeFilePathOnDisk(filePath, rootDir);

            var existingFile = FindFile(relativePath);
            if (existingFile != null)
            {
                if (!existingFile.IsFileOnDisk) existingFile.FilePathOnDisk = filePath;
                return existingFile;
            }

            var splitted = SplitPath(relativePath);
            if (splitted.Length <= 0) return null;
            var directory = ResolveDirectory(splitted, true, true);
            if (directory == null) return null;
            var file = new LOTD_File
            {
                Name = splitted[splitted.Length - 1],
                Directory = directory,
                FilePathOnDisk = filePath
            };
            return file;
        }

        public string GetRelativeFilePathOnDisk(string filePath, string rootDir)
        {
            return GetRelativePathOnDisk(filePath, rootDir, true);
        }

        public string GetRelativeDirectoryOnDisk(string directory, string rootDir)
        {
            return GetRelativePathOnDisk(directory, rootDir, false);
        }

        public string GetRelativePathOnDisk(string path, string rootDir, bool isFile)
        {
            if (isFile && !File.Exists(path) || !isFile && !Directory.Exists(path)) return null;

            path = Path.GetFullPath(path);
            rootDir = Path.GetFullPath(rootDir);

            var dir = isFile ? Path.GetDirectoryName(path) : path;
            if (!IsSameOrSubDirectory(rootDir, dir))
                throw new Exception("Path Needs To Be A Sub-Directory Of RootDir! PLEASE CHECK CODE!");

            var pathUri = new Uri(path);
            var referenceUri = new Uri(rootDir);
            var relativePath = referenceUri.MakeRelativeUri(pathUri).ToString();
            var firstPathIndex = relativePath.IndexOfAny(new[] {Path.PathSeparator, Path.AltDirectorySeparatorChar});
            if (firstPathIndex >= 0) relativePath = relativePath.Substring(firstPathIndex + 1);

            return relativePath;
        }

        public LOTD_File AddFile(string path, long offset, long length)
        {
            var splitted = SplitPath(path);
            if (splitted.Length <= 0) return null;
            var directory = ResolveDirectory(splitted, true, true);
            if (directory == null) return null;
            var file = new LOTD_File
            {
                Name = splitted[splitted.Length - 1],
                Directory = directory,
                ArchiveOffset = offset,
                ArchiveLength = length
            };
            return file;
        }

        public void RemoveFile(string path)
        {
            var file = FindFile(path);
            if (file != null) file.Directory = null;
        }

        public void RemoveFile(LOTD_File file)
        {
            if (file.Directory == this) file.Directory = null;
        }

        private bool IsSameOrSubDirectory(string basePath, string path, out string subDirectory)
        {
            var di = new DirectoryInfo(Path.GetFullPath(path).TrimEnd('\\', '/'));
            var diBase = new DirectoryInfo(Path.GetFullPath(basePath).TrimEnd('\\', '/'));

            subDirectory = null;
            while (di != null)
                if (di.FullName.Equals(diBase.FullName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else
                {
                    subDirectory = string.IsNullOrEmpty(subDirectory) ? di.Name : Path.Combine(di.Name, subDirectory);
                    di = di.Parent;
                }

            return false;
        }
    }
}