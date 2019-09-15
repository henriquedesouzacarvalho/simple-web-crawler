using System;

namespace simple_web_crawler
{
    class Program
    {
        private static string FileUrl { get; set; }
        private static DownloadFile DownloadFile;
        private static ProcessFile ProcessFile;

        static void Main(string[] args)
        {
            Process();
        }

        private static void Process()
        {
            DownloadFile = new DownloadFile();
            ProcessFile = new ProcessFile();

            Console.WriteLine("Enter the URL of the file to be downloaded:");
            FileUrl = Console.ReadLine();

            while (!(string.IsNullOrEmpty(FileUrl) && DownloadFile.ValidateURI(FileUrl)))
            {
                if (FileUrl.ToLower() == ":q!")
                {
                    Environment.Exit(0);
                }
                else if (string.IsNullOrEmpty(FileUrl) || !DownloadFile.ValidateURI(FileUrl))
                {
                    Console.Clear();
                    Console.WriteLine("Please enter a valid URL for the file download:");
                    Console.WriteLine("If you want to terminate the program press':q!'.");
                }
                else
                {
                    break;
                }

                FileUrl = Console.ReadLine();
            }

            var downloadedFilePath = DownloadFile.DownloadFileFrom(FileUrl);

            ProcessFile.GetFileDataString(downloadedFilePath);

            Console.WriteLine($"{Environment.NewLine}How many of each letter are in the file");
            ProcessFile.ShowAllOfEachLetterInFile();

            Console.WriteLine($"{Environment.NewLine}How many letters are capitalized in the file.");
            ProcessFile.CountAllCapitalizedLettersInFile();

            Console.WriteLine($"{Environment.NewLine}The most common word and the number of times it has been seen.");
            ProcessFile.ShowMostCommonWordInFile();

            Console.WriteLine($"{Environment.NewLine}The most common 2 character prefix and the number of occurrences in the text file.");
            ProcessFile.ShowMostCommonPrefixInFile();
        }
    }
}
