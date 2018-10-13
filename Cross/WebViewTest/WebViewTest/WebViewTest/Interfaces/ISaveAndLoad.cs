using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace WebViewTest.Interfaces
{
    public interface ISaveAndLoad
    {
        string GetBaseUrl();
        string GetAppBaseUrl();
        void SaveText(string fileName, string text);
        string LoadText(string fileName, bool isResource = false);
        void DeleteFile(string filePath);
        void DeleteFolder(string folderPath);
        bool ExistsFile(string fileName);
        bool ExistsFolder(string folderPath);
        Task DownloadFile(string url, string fileName);
        string[] GetDirectories();
    }
}