using System;
using System.Collections.Generic;
using System.IO;
using Celtic_Guardian.Miscellaneous_Files;

namespace Celtic_Guardian.LOTD_Files
{
    public class LOTD_Directory
    {
        private string _name;
        private LOTD_Directory _parent;
        public LOTD_Archive Archive { get; set; }
        public Dictionary<string, LOTD_File> Files { get; private set; }
        public Dictionary<string, LOTD_Directory> Directories { get; private set; }
        public bool IsRoot { get; set; }

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

        public LOTD_Directory()
        {
            Files = new Dictionary<string, LOTD_File>();
            Directories = new Dictionary<string, LOTD_Directory>();
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
            return path.Split(new[] {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar},
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
    }
    }
}