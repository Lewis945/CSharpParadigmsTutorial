using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using DataItem = System.Tuple<string, decimal, decimal, decimal>;

namespace Functional
{
    public static class Program
    {
        private static Regex _digitsOnly = new Regex(@"[^\d]");

        public static void Main(string[] args)
        {
            Run(@"Data\weather.dat", new int[] { 0, 1, 2 }, "Day with min spread");

            Console.WriteLine($"-----------------------");

            Run(@"Data\football.dat", new int[] { 1, 6, 8 }, "Team with min spread");

            Console.ReadKey();
        }

        private static void Run(string path, int[] columns, string message)
        {
            var print = PrintData(message, GetMinSpreadItem);

            var data = File.ReadLines(path)
                .ParseFile(new char[] { ' ', '\t' }, true)
                .ParseData(columns);

            print(data);
        }

        private static IEnumerable<string[]> ParseFile(this IEnumerable<string> data, char[] separator, bool ignoreHeaders = false)
        {
            return data
                .Filter((item, index) => !ignoreHeaders || index != 0)
                .Map((item, index) => item.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                .Filter((item, index) => item.Length > 1);
        }

        private static IEnumerable<DataItem> ParseData(this IEnumerable<string[]> data, int[] columnIndecies)
        {
            if (columnIndecies.Length != 3)
                throw new ArgumentException("Length is not equal to 3", nameof(columnIndecies));

            return data
                .Map((item, index) => new DataItem(
                    item[columnIndecies[0]],
                    decimal.Parse(_digitsOnly.Replace(item[columnIndecies[1]], string.Empty)),
                    decimal.Parse(_digitsOnly.Replace(item[columnIndecies[2]], string.Empty)),
                    0)
                )
                .Map((item, index) =>
                {
                    var diff = Math.Abs(item.Item2 - item.Item3);
                    return new DataItem(item.Item1, item.Item2, item.Item3, diff);
                });
        }

        private static DataItem GetMinSpreadItem(this IEnumerable<DataItem> data)
        {
            return data.Reduce<DataItem, DataItem>((aggregate, item, index) =>
            {
                if (item.Item4 < (aggregate?.Item4 ?? int.MaxValue))
                    return item;
                return aggregate;
            });
        }

        private static Action<IEnumerable<DataItem>> PrintData(string message, Func<IEnumerable<DataItem>, DataItem> getItemWithMinSpread)
        {
            return (data) =>
            {
                Console.WriteLine("File data:");

                foreach (var item in data)
                {
                    Console.WriteLine($"{item.Item1} - {item.Item2} - {item.Item3}; Spread = {item.Item4}");
                }

                Console.WriteLine($"{message}: {getItemWithMinSpread(data).Item1}");
            };
        }
    }
}
