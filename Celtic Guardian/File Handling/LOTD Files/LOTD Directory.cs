using Celtic_Guardian.Miscellaneous_Files;
using System;
using System.Collections.Generic;
using System.IO;
using Celtic_Guardian.Utility;

namespace Celtic_Guardian.LOTD_Files
{
    public class LOTD_Directory
    {
        //*******
        //Globals
        //*******
        private string _name;
        private LOTD_Directory _parent;

        //**********
        //Properties
        //**********
        public LOTD_Archive Archive { get; set; }
        public Dictionary<string, LOTD_File> Files { get; private set; }
        public Dictionary<string, LOTD_Directory> Directories { get; private set; }
        public bool IsRoot { get; set; }

        //*************************
        //Expanded Getters, Setters
        //*************************
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                if (Parent != null) Parent.Directories.Remove(_name);

                _name = value;

                if (Parent != null) Parent.Directories.Add(_name, this);
            }
        }
        public string FullName
        {
            get
            {
                var name = Name == null ? string.Empty : Name;
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

        //***********
        //Constructor
        //***********
        public LOTD_Directory()
        {
            Files = new Dictionary<string, LOTD_File>(StringComparer.OrdinalIgnoreCase);
            Directories = new Dictionary<string, LOTD_Directory>(StringComparer.OrdinalIgnoreCase);
        }

        //*****************
        //Functions
        //*****************
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

        private LOTD_Directory ResolveDirectory(string[] path, bool isFilePath, bool create)
        {
            var directory = this;

            var pathCount = path.Length + (isFilePath ? -1 : 0);
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
                        subDir = new LOTD_Directory();
                        subDir.Name = path[i];
                        subDir.Parent = directory;
                        directory = subDir;
                    }
                }

            return directory;
        }

        private string[] SplitPath(string path)
        {
            return path.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar },
                StringSplitOptions.RemoveEmptyEntries);
        }

        private bool IsSameOrSubDirectory(string BasePath, string Path)
        {
            var dirInfoPath = new DirectoryInfo(System.IO.Path.GetFullPath(Path).TrimEnd('\\', '/'));
            var DirInfoBasePath = new DirectoryInfo(System.IO.Path.GetFullPath(BasePath).TrimEnd('\\', '/'));

            var SubDirectory = "";
            while (dirInfoPath != null)
            {
                if (dirInfoPath.FullName.Equals(DirInfoBasePath.FullName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                SubDirectory = string.IsNullOrEmpty(SubDirectory) ? dirInfoPath.Name : System.IO.Path.Combine(dirInfoPath.Name, SubDirectory);
                dirInfoPath = dirInfoPath.Parent;
            }
            return false;
        }

        public LOTD_File[] AddFilesOnDisk(string directory, string rootDir, bool recursive)
        {
            List<LOTD_File> files = new List<LOTD_File>();
            if (Directory.Exists(directory))
            {
                SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                foreach (string filePath in Directory.GetFiles(directory, "*.*", searchOption))
                {
                    LOTD_File file = AddFileOnDisk(filePath, rootDir);
                    if (file != null)
                    {
                        files.Add(file);
                    }
                }
            }
            return files.ToArray();
        }

        public LOTD_File AddFileOnDisk(string filePath, string rootDir)
        {
            if (File.Exists(filePath))
            {
                string relativePath = GetRelativeFilePathOnDisk(filePath, rootDir);

                LOTD_File existingFile = FindFile(relativePath);
                if (existingFile != null)
                {
                    if (!existingFile.IsFileOnDisk)
                    {
                        // Already exists as an archive file or a placeholder
                        // Set the file path (this will favor loading from the file rather than the archive)
                        existingFile.FilePathOnDisk = filePath;
                    }
                    return existingFile;
                }

                string[] splitted = SplitPath(relativePath);
                if (splitted.Length > 0)
                {
                    LOTD_Directory directory = ResolveDirectory(splitted, true, true);
                    if (directory != null)
                    {
                        LOTD_File file = new LOTD_File();
                        file.Name = splitted[splitted.Length - 1];
                        file.Directory = directory;
                        file.FilePathOnDisk = filePath;
                        return file;
                    }
                }
            }
            return null;
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
            if ((isFile && !File.Exists(path)) || (!isFile && !Directory.Exists(path)))
            {
                return null;
            }

            path = Path.GetFullPath(path);
            rootDir = Path.GetFullPath(rootDir);

            string dir = isFile ? Path.GetDirectoryName(path) : path;
            if (!IsSameOrSubDirectory(rootDir, dir))
            {
                throw new Exception("Path Needs To Be A Sub-Directory Of RootDir! PLEASE CHECK CODE!");
            }

            Uri pathUri = new Uri(path);
            Uri referenceUri = new Uri(rootDir);
            string relativePath = referenceUri.MakeRelativeUri(pathUri).ToString();
            int firstPathIndex = relativePath.IndexOfAny(new char[] { Path.PathSeparator, Path.AltDirectorySeparatorChar });
            if (firstPathIndex >= 0)
            {
                relativePath = relativePath.Substring(firstPathIndex + 1);
            }

            return relativePath;
        }

        public LOTD_File AddFile(string path, long offset, long length)
        {
            string[] splitted = SplitPath(path);
            if (splitted.Length > 0)
            {
                LOTD_Directory directory = ResolveDirectory(splitted, true, true);
                if (directory != null)
                {
                    LOTD_File file = new LOTD_File();
                    file.Name = splitted[splitted.Length - 1];
                    file.Directory = directory;
                    file.ArchiveOffset = offset;
                    file.ArchiveLength = length;
                    return file;
                }
            }
            return null;
        }

        public void RemoveFile(string path)
        {
            LOTD_File file = FindFile(path);
            if (file != null)
            {
                file.Directory = null;
            }
        }

        public void RemoveFile(LOTD_File file)
        {
            if (file.Directory == this)
            {
                file.Directory = null;
            }
        }

        private bool IsSameOrSubDirectory(string basePath, string path, out string subDirectory)
        {
            DirectoryInfo di = new DirectoryInfo(Path.GetFullPath(path).TrimEnd('\\', '/'));
            DirectoryInfo diBase = new DirectoryInfo(Path.GetFullPath(basePath).TrimEnd('\\', '/'));

            subDirectory = null;
            while (di != null)
            {
                if (di.FullName.Equals(diBase.FullName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else
                {
                    if (string.IsNullOrEmpty(subDirectory))
                    {
                        subDirectory = di.Name;
                    }
                    else
                    {
                        subDirectory = Path.Combine(di.Name, subDirectory);
                    }
                    di = di.Parent;
                }
            }
            return false;
        }
    }
}