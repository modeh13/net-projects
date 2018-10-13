using System;
using System.Collections.Generic;
using System.IO;

namespace FileExplorer.Core
{
    public static class Util
    {
        public readonly static string FolderPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        
        //private readonly static string FolderPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public static string GetFolderPath()
        {
            var s = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var s1 = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
            var s2 = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms);
            var s3 = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var s4 = System.Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            var s5 = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var s6 = System.Environment.GetFolderPath(Environment.SpecialFolder.Programs);
            var s7 = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);

            //return Path.Combine(FolderPath, "..", "Library");   
            return "/Users/compilacion/Library/Developer/CoreSimulator/Devices/FA2D5747-6D4D-4EF8-A8DE-3F8E4CDDCDAB/data/Containers/Data/Application/";
        }

        public static IEnumerable<FileFolder> GetFileFolderList(string folderPath)
        {
            List<FileFolder> list = new List<FileFolder>();

            if (Directory.Exists(folderPath))
            {
                DirectoryInfo mainDirectory = new DirectoryInfo(folderPath);

                foreach (DirectoryInfo directory in mainDirectory.GetDirectories())
                {
                    //directory.LastWriteTime
                    list.Add(new FileFolder()
                    {
                        Name = directory.Name,
                        FullName = directory.FullName,
                        CreationTime = directory.CreationTime,
                        LastWriteTime = directory.LastWriteTime,
                        //ParentFolder = folderPath,
                        FilesCount = directory.GetFiles().Length,
                        FolderCount = directory.GetDirectories().Length,
                        Type = Core.Type.Folder
                    });
                }

                foreach (FileInfo file in mainDirectory.GetFiles())
                {
                    list.Add(new FileFolder()
                    {
                        Name = file.Name,
                        FullName = file.FullName,
                        CreationTime = file.CreationTime,
                        LastWriteTime = file.LastWriteTime,
                        //ParentFolder = folderPath,
                        FileSize = file.Length,
                        Type = Core.Type.File
                    });
                }

            }
            return list;
        }
    }
}