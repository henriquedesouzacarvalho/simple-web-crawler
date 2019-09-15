using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace simple_web_crawler
{
    public class ProcessFile
    {
        private static string FileStringData { get; set; }
        public void GetFileDataString(string downloadedFilePath)
        {
            var content = new StreamReader(downloadedFilePath);
            string line, allFileLines = string.Empty;

            while ((line = content.ReadLine()) != null)
                if (!string.IsNullOrEmpty(line))
                    allFileLines += line;

            content.Close();

            FileStringData = allFileLines;
        }

        public void ShowAllOfEachLetterInFile()
        {
            var countedList = FileStringData.GroupBy(x => x)
                                            .Select(x => new { x.Key, Quantity = x.Count() }).ToList();

            countedList.Sort((x, y) => y.Quantity.CompareTo(x.Quantity));

            foreach (var l in countedList)
            {
                Console.WriteLine($"Letter: {l.Key} - Quantity: {l.Quantity}");
            }
        }

        public void CountAllCapitalizedLettersInFile()
        {
            var value = Regex
                        .Matches(FileStringData, "[A-Z]")
                        .OfType<Match>()
                        .Select(match => match.Value)
                        .Count();

            Console.WriteLine(value);
        }

        private IEnumerable<dynamic> GetWordList()
        {
            return (from i in Regex.Split(string.Concat(FileStringData.Select(c => char.IsUpper(c) ? " " + c : c.ToString())).TrimStart(' '), @"\W+")
                    group i by i into grp
                    orderby grp.Count() descending
                    select new { grp.Key, Count = grp.Count() }).Where(r => r.Count > 1);
        }

        public void ShowMostCommonWordInFile()
        {
            var word = GetWordList().First();
            Console.WriteLine($"Word: {word.Key} - Times Seen: {word.Count}");
        }

        public void ShowMostCommonPrefixInFile()
        {
            var wordsList = GetWordList();

            var mostCommonPrefixList = wordsList.Where(x => x.Key.Length > 2).Select(a => new { Key = a.Key.ToLower().Substring(0, 2), a.Count }).ToList();
            mostCommonPrefixList.Sort((x, y) => x.Key.CompareTo(y.Key));

            var mostCommonPrefixQuery = (from m in mostCommonPrefixList
                                         group m by m.Key into mcpl
                                         select new { Key = mcpl.Key, Count = mcpl.Sum(x => x.Count) }).ToList();

            mostCommonPrefixQuery.Sort((x, y) => y.Count.CompareTo(x.Count));
            var mostCommonPrefix = mostCommonPrefixQuery.FirstOrDefault();

            Console.WriteLine($"Prefix: {mostCommonPrefix.Key} - Number of Occurrences: {mostCommonPrefix.Count}{Environment.NewLine}");
        }
    }
}
