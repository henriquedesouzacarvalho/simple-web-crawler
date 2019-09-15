using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace simple_web_crawler
{
    public class DownloadFile
    {
        private readonly string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public string DownloadDirectory { get; set; }

        public void GetDownloadDirectory()
        {
            var parentDirName = (new FileInfo(baseDirectory).Directory.Parent.Parent.Parent).FullName;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                parentDirName += @"\Download\";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                parentDirName += @"/Download/";
            }
            DownloadDirectory = parentDirName;
        }

        public string DownloadFileFrom(string fileUrl)
        {
            Console.WriteLine($"Getting the file from: {fileUrl}");

            GetDownloadDirectory();
            var downloadPath = DownloadDirectory;
            var webClient = new WebClient();
            var fileContent = webClient.DownloadData(fileUrl);
            var completeFilePath = downloadPath + "Downloaded_File_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss.txt");

            if (!Directory.Exists(downloadPath))
                Directory.CreateDirectory(downloadPath);

            File.WriteAllBytes(completeFilePath, fileContent);

            return completeFilePath;
        }

        public bool ValidateURI(string uri) => Uri.TryCreate(uri, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);      
    }
}
