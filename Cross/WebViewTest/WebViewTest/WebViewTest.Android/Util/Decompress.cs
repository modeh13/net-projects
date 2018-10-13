using System;
using System.IO;
using Java.Util.Zip;

namespace WebViewTest.Droid.Util
{
    public class Decompress
    {
        string _zipFile;
        string _location;

        public Decompress(string zipFile, string location)
        {
            _zipFile = zipFile;
            _location = location;
            DirChecker("");
        }

        void DirChecker(string dir)
        {
            var file = new Java.IO.File(_location + dir);

            if (!file.IsDirectory)
            {
                file.Mkdirs();
            }
        }

        public void UnZip()
        {
            using (ZipInputStream s = new ZipInputStream(System.IO.File.OpenRead(_zipFile)))
            {
                ZipEntry theEntry;
                while ((theEntry = s.NextEntry) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    try
                    {                        
                        directoryName = Path.Combine(_location, directoryName);
                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(directoryName);
                        }
                        if (fileName != String.Empty)
                        {
                            using (FileStream streamWriter = System.IO.File.Create(Path.Combine(_location, theEntry.Name)))
                            {
                                int size = 2048;
                                byte[] data = new byte[size];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex) {
                        System.Diagnostics.Debug.WriteLine($"Error descomprimiendo el archivo: {fileName}. Ex: {ex.Message}");
                    }                    
                }
            }
        }
    }
}