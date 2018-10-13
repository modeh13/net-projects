using System.IO;

using Foundation;
using Xamarin.Forms;
using WebViewTest.iOS.Implementations;
using WebViewTest.Interfaces;
using System.Threading.Tasks;
using System.Net;
using System;
using MiniZip.ZipArchive;

[assembly: Dependency(typeof(SaveAndLoad))]
namespace WebViewTest.iOS.Implementations
{
    public class SaveAndLoad : ISaveAndLoad
    {
        private string BundlePath = NSBundle.MainBundle.BundlePath;
        private string AppPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        /// <summary>
        /// Get the Base URL
        /// </summary>
        /// <returns></returns>
        public string GetBaseUrl()
        {
            return BundlePath;
        }

        /// <summary>
        /// Get the Base URL to APP.
        /// </summary>
        /// <returns></returns>
        public string GetAppBaseUrl()
        {
            return AppPath;
        }

        /// <summary>
        /// Create file on the Base URL App
        /// </summary>
        /// <param name="fileName">File name with Path</param>
        /// <param name="text">Content file text</param>
        public void SaveText(string fileName, string text)
        {
            //string directoryPath = Path.Combine(AppPath, "..", Path.GetDirectoryName(fileName));
            string directoryPath = Path.Combine(AppPath, Path.GetDirectoryName(fileName));

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, Path.GetFileName(fileName));
            File.WriteAllText(filePath, text);
        }

        /// <summary>
        /// Load the content of file.
        /// </summary>
        /// <param name="filename">File name with Path</param>
        /// <param name="isResource">True: If File is Resource APP</param>
        /// <returns></returns>
        public string LoadText(string fileName, bool isResource)
        {
            string fileText = string.Empty;

            if (isResource)
            {
                using (StreamReader sr = new StreamReader(Path.Combine(BundlePath, fileName)))
                {
                    fileText = sr.ReadToEnd();
                }
            }
            else {                
                string filePath = Path.Combine(AppPath, fileName);

                if (File.Exists(filePath))
                {
                    fileText = File.ReadAllText(filePath);
                }
            }

            return fileText;            
        }

        /// <summary>
        /// Delete File
        /// </summary>
        /// <param name="filePath">File Path</param>
        public void DeleteFile(string filePath)
        {
            filePath = Path.Combine(AppPath, filePath);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// Delete Folder
        /// </summary>
        /// <param name="folderPath">Folder Path</param>
        public void DeleteFolder(string folderPath)
        {
            string fullPath = Path.Combine(AppPath, folderPath);
            if (ExistsFolder(fullPath))
            {
                Directory.Delete(fullPath, true);
            }
        }

        /// <summary>
        /// Check if File exists
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        public bool ExistsFile(string fileName)
        {
            string fullPath = Path.Combine(AppPath, fileName);
            return File.Exists(fileName);
        }

        /// <summary>
        /// Check if Folder exists
        /// </summary>
        /// <param name="folderPath">Folder path</param>
        /// <returns></returns>
        public bool ExistsFolder(string folderPath)
        {
            string fullPath = Path.Combine(AppPath, folderPath);
            return Directory.Exists(fullPath);
        }

        public string[] GetDirectories()
        {
            return Directory.GetDirectories(AppPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task DownloadFile(string url, string fileName)
        {
            using (WebClient client = new WebClient())
            {
                string filePath = Path.Combine(AppPath, fileName);
                string directoryPath = Path.Combine(AppPath, Path.GetDirectoryName(fileName));
                //string realFileName = Android.Webkit.URLUtil.GuessFileName(url, null, null);

                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                client.DownloadFileCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Error : " + e.Error.Message);
                    }

                    if (!e.Cancelled)
                    {
                        try
                        {
                            if (File.Exists(filePath))
                            {
                                string unZipFile = Path.Combine(AppPath);
                                var zip = new ZipArchive();
                                zip.UnzipOpenFile(filePath);
                                zip.UnzipFileTo(unZipFile, true);

                                //zip.OnError += (sender, args) => {
                                //    Console.WriteLine("Error while unzipping: {0}", args);
                                //};

                                zip.UnzipCloseFile();
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
                        }
                    }
                };

                try
                {
                    //await client.DownloadFileTaskAsync(url, filePath);
                    
                    client.DownloadDataAsync(new Uri(url));
                    client.DownloadDataCompleted += (sen, e) =>
                    {
                        if (e.Error == null)
                        {
                            byte[] bytes = e.Result;
                            using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write))
                            {
                                fs.Write(bytes, 0, bytes.Length);
                            }

                            if (File.Exists(filePath))
                            {
                                string unZipFile = Path.Combine(AppPath);
                                var zip = new ZipArchive();
                                zip.UnzipOpenFile(filePath);
                                zip.UnzipFileTo(unZipFile, true);

                                //zip.OnError += (sender, args) => {
                                //    Console.WriteLine("Error while unzipping: {0}", args);
                                //};

                                zip.UnzipCloseFile();
                            }
                        }
                    };                    
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
                }
            }
        }

        private float GetFreeSize() {
            return NSFileManager.DefaultManager.GetFileSystemAttributes(AppPath).FreeSize / 1024F / 1024F;
        }
    }
}