using System;

using Foundation;
using UIKit;

namespace FileExplorer.Core
{
    public partial class FileFolderCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("FileFolderCell");
        public static readonly UINib Nib;

        static FileFolderCell()
        {
            Nib = UINib.FromName("FileFolderCell", NSBundle.MainBundle);
        }

        protected FileFolderCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void UpdateCell(FileFolder fileFolder)
        {
            nameLabel.Text = fileFolder.Name;
            creationDateLabel.Text = fileFolder.CreationTime.ToString("dd-MM-yyyy hh:mm:ss tt");
            lastWriteDateLabel.Text = fileFolder.CreationTime.ToString("dd-MM-yyyy hh:mm:ss tt");

            switch (fileFolder.Type)
            {
                case Type.File:
                    descriptionLabel.Text = GetFileSize(fileFolder.FileSize);
                    break;
                case Type.Folder:
                    descriptionLabel.Text = $"{fileFolder.FolderCount} carpetas y {fileFolder.FilesCount} archivos";
                    break;
                default:
                    break;
            }
        }

        private string GetFileSize(long size) {
            string description = string.Empty;
            float fileSize = 0;

            if (size < (1024 * 1024))
            {
                fileSize = size / 1024F;
                description = $"Tamaño {fileSize} Kb";
            }
            else {
                fileSize = size / 1024F / 1024F;
                description = $"Tamaño {fileSize} Mb";
            }

            return description;
        }
    }
}