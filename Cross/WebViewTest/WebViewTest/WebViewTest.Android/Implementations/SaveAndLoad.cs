using WebViewTest.Interfaces;
using System.IO;
using WebViewTest.Droid.Implementations;
using Xamarin.Forms;
using Android.Content.Res;
using System.Threading.Tasks;
using System;
using System.IO.Compression;
using WebViewTest.Droid.Util;
using System.Net;
using Android.OS;
using System.Collections.Generic;
using System.Linq;

[assembly: Dependency(typeof(SaveAndLoad))]
namespace WebViewTest.Droid.Implementations
{
    public class SaveAndLoad : ISaveAndLoad
    {
        private string AssetPath = "file:///android_asset/";
        //private string AppPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        private string AppPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "MyMintApp");
        //private string AppPath = Path.Combine(Android.OS.Environment.DirectoryDocuments, "MyMintApp");
        //private string AppPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyMintApp");

        /// <summary>
        /// Get the Base URL
        /// </summary>
        /// <returns></returns>
        public string GetBaseUrl()
        {
            return AssetPath;
        }

        /// <summary>
        /// Get the Base URL to APP.
        /// </summary>
        /// <returns></returns>
        public string GetAppBaseUrl()
        {
            return $"file://{AppPath}/";
        }

        /// <summary>
        /// Create file on the Base URL App
        /// </summary>
        /// <param name="filename">File name with Path</param>
        /// <param name="text">Content file text</param>
        public void SaveText(string fileName, string text)
        {
            string directoryPath = Path.Combine(AppPath, Path.GetDirectoryName(fileName));

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
                        
            string filePath = Path.Combine(AppPath, fileName);
            File.WriteAllText(filePath, text);
        }

        /// <summary>
        /// Load the content of file.
        /// </summary>
        /// <param name="fileName">File name with Path</param>
        /// <param name="isResource">True: If File is Resource APP</param>
        /// <returns></returns>
        public string LoadText(string fileName, bool isResource)
        {
            string fileText = string.Empty;

            if (isResource)
            {
                using (StreamReader sr = new StreamReader(Android.App.Application.Context.Assets.Open(fileName)))
                {
                    fileText = sr.ReadToEnd();
                }
            }
            else {
                string filePath = Path.Combine(AppPath, fileName);

                if (File.Exists(filePath)) {
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
        /// Check if Folder exists
        /// </summary>
        /// <param name="folderPath">Folder Path</param>
        /// <returns></returns>
        public bool ExistsFolder(string folderPath)
        {   
            string fullPath = Path.Combine(AppPath, folderPath);
            return Directory.Exists(fullPath);
        }

        /// <summary>
        /// Check if File exists
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <returns></returns>
        public bool ExistsFile(string fileName)
        {
            string fullPath = Path.Combine(AppPath, fileName);
            return File.Exists(fileName);
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

        public string[] GetDirectories()
        {
            return Directory.GetDirectories(AppPath);
        }

        public async Task DownloadFile(string url, string fileName)
        {
            using (WebClient client = new WebClient())
            {                
                string filePath = Path.Combine(AppPath, fileName);
                string unZipFolderName = Path.GetFileNameWithoutExtension(fileName);
                string directoryPath = Path.Combine(AppPath, Path.GetDirectoryName(fileName));
                //string realFileName = Android.Webkit.URLUtil.GuessFileName(url, null, null);  

                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                
                client.DownloadFileCompleted += (sender, e) =>
                {
                    float freeSpace = FreeMemory();

                    if (e.Error != null) {
                        System.Diagnostics.Debug.WriteLine("Error : " + e.Error.Message);
                    }

                    if (!e.Cancelled)
                    {
                        try
                        {
                            if (File.Exists(filePath))
                            {
                                FileInfo file = new FileInfo(filePath);

                                if (Path.GetExtension(filePath) == ".zip")
                                {
                                    string unZipFile = Path.Combine(AppPath, unZipFolderName);

                                    if (!Directory.Exists(unZipFile)) {
                                        Directory.CreateDirectory(unZipFile);
                                    }

                                    Decompress decompress = new Decompress(filePath, unZipFile);
                                    decompress.UnZip();
                                    File.Delete(filePath);
                                }                                
                            }
                        }
                        catch (Exception ex) {
                            System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
                        }
                    }
                };

                try
                {
                    await client.DownloadFileTaskAsync(url, filePath);
                    //System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                    ////client.
                    //client.DownloadDataAsync(new Uri(url));
                    //client.DownloadDataCompleted += (sen, e) =>
                    //{
                    //    if (e.Error == null)
                    //    {
                    //        byte[] bytes = e.Result;
                    //        float sizeMb = bytes.Length / 1024F / 1024F;
                    //        float freeMemoryMb = FreeMemory();

                    //        if (freeMemoryMb > (2 * sizeMb))
                    //        {
                    //            using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write))
                    //            {
                    //                fs.Write(bytes, 0, bytes.Length);
                    //            }

                    //            if (File.Exists(filePath))
                    //            {
                    //                if (Path.GetExtension(filePath) == ".zip")
                    //                {
                    //                    freeMemoryMb = FreeMemory();

                    //                    if (freeMemoryMb > (5 * sizeMb))
                    //                    {
                    //                        //string unZipFile = Path.Combine(AppPath);
                    //                        string unZipFile = Path.Combine(AppPath, "12.1");

                    //                        if (!Directory.Exists(unZipFile)) {
                    //                            Directory.CreateDirectory(unZipFile);
                    //                        }

                    //                        Decompress decompress = new Decompress(filePath, unZipFile);
                    //                        decompress.UnZip();
                    //                        File.Delete(filePath);
                    //                    }
                    //                    else {
                    //                        System.Diagnostics.Debug.WriteLine("Espacio no disponible para descomprimir el archivo");
                    //                    }
                    //                }
                    //            }
                    //        }
                    //        else {
                    //            System.Diagnostics.Debug.WriteLine("Espacio no disponible para la descarga.");
                    //        }                            
                    //    }
                    //};
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
                }
            }            
        }


        public float FreeMemory()
        {
            Android.OS.StatFs statFs = new Android.OS.StatFs(Android.OS.Environment.RootDirectory.AbsolutePath);
            var storage = GetStorageInformation(Android.OS.Environment.RootDirectory.AbsolutePath);
            long Free = (statFs.AvailableBlocksLong * statFs.BlockSizeLong);

            //Android.OS.Environment.DataDirectory.AbsolutePath
            //Android.OS.Environment.ExternalStorageDirectory.Path
            Android.OS.StatFs stat = new Android.OS.StatFs(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));
            long blockSize = stat.BlockSizeLong;
            long availableBlocks = stat.AvailableBlocksLong;
            long Free2 = (stat.AvailableBlocksLong * stat.BlockSizeLong);

            return Free2 / 1024F / 1024F;
        }

        protected StorageInfo GetStorageInformation(string path)
        {
            StorageInfo storageInfo = new StorageInfo();

            Android.OS.StatFs stat = new Android.OS.StatFs(path); //"/storage/sdcard1"
            long totalSpaceBytes = 0;
            long freeSpaceBytes = 0;
            long availableSpaceBytes = 0;

            /*
              We have to do the check for the Android version, because the OS calls being made have been deprecated for older versions. 
              The ‘old style’, pre Android level 18 didn’t use the Long suffixes, so if you try and call use those on 
              anything below Android 4.3, it’ll crash on you, telling you that that those methods are unavailable. 
              http://blog.wislon.io/posts/2014/09/28/xamarin-and-android-how-to-use-your-external-removable-sd-card/
             */
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBeanMr2)
            {
                long blockSize = stat.BlockSizeLong;
                totalSpaceBytes = stat.BlockCountLong * stat.BlockSizeLong;
                availableSpaceBytes = stat.AvailableBlocksLong * stat.BlockSizeLong;
                freeSpaceBytes = stat.FreeBlocksLong * stat.BlockSizeLong;
            }
            else
            {

                totalSpaceBytes = (long)stat.BlockCount * (long)stat.BlockSize;
                availableSpaceBytes = (long)stat.AvailableBlocks * (long)stat.BlockSize;
                freeSpaceBytes = (long)stat.FreeBlocks * (long)stat.BlockSize;
            }

            storageInfo.TotalSpace = totalSpaceBytes;
            storageInfo.AvailableSpace = availableSpaceBytes;
            storageInfo.FreeSpace = freeSpaceBytes;
            return storageInfo;

        }
    }

    public class StorageInfo
    {
        public long TotalSpace { get; set; }
        public long AvailableSpace { get; set; }

        public long FreeSpace { get; set; }
    }
}