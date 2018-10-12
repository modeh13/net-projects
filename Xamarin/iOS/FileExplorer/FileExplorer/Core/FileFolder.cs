using System;

namespace FileExplorer.Core
{
    public enum Type
    {
        File = 0,
        Folder = 1
    }

    public class FileFolder
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        //public string ParentFolder { get; set; }
        public int FilesCount { get; set; }
        public int FolderCount { get; set; }
        public long FileSize { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }

        public Type Type { get; set; }
    }
}