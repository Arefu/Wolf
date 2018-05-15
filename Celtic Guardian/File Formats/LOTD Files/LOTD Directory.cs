using System;
using System.Collections.Generic;
using System.IO;

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

        public LOTD_File FindFile(string path)
        {
            var splitted = SplitPath(path);
            if (splitted.Length > 0)
            {
                var directory = ResolveDirectory(splitted, true, false);
                LOTD_File file;
                if (directory != null && directory.Files.TryGetValue(splitted[splitted.Length - 1], out file))
                    return file;
            }

            return null;
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
                    LOTD_Directory subDir;
                    if (directory.Directories.TryGetValue(path[i], out subDir))
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
    }
}