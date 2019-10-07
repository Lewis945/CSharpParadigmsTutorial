using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Imperative
{
    public class Program
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
            var fileData = ReadFile(path, new char[] { ' ', '\t' }, true);
            var data = ParseData(fileData, columns);
            var itemWithMinSpread = GetDataItemWithMinSpread(data);

            Console.WriteLine("File data:");

            for (int i = 0; i < data.Length; i++)
            {
                var item = data[i];
                Console.WriteLine($"{item[0]} - {item[1]} - {item[2]}; Spread = {item[3]}");
            }

            Console.WriteLine($"{message}: {itemWithMinSpread[0]}");
        }

        private static string[][] ReadFile(string filePath, char[] separator, bool ignoreHeaders = false)
        {
            var lines = File.ReadAllLines(filePath);

            int startIndex = ignoreHeaders ? 1 : 0;
            int j = 0;

            var data = new string[lines.Length][];

            for (int i = startIndex; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var splitLine = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                if (splitLine.Length == 1)
                    continue;

                data[j] = splitLine;
                j++;
            }

            return TrimArray<string>(data);
        }

        private static object[][] ParseData(string[][] data, int[] columnIndecies)
        {
            if (columnIndecies.Length != 3)
                throw new ArgumentException("Length is not equal to 3", nameof(columnIndecies));

            var parsedData = new object[data.Length][];

            for (int i = 0; i < data.Length; i++)
            {
                var item = data[i];
                parsedData[i] = new object[4]
                {
                    item[columnIndecies[0]],
                    decimal.Parse(_digitsOnly.Replace(item[columnIndecies[1]], string.Empty)),
                    decimal.Parse(_digitsOnly.Replace(item[columnIndecies[2]], string.Empty)),
                    0
                };

                var diff = Math.Abs((decimal)parsedData[i][1] - (decimal)parsedData[i][2]);
                parsedData[i][3] = diff;
            }

            return parsedData;
        }

        private static object[] GetDataItemWithMinSpread(object[][] data)
        {
            decimal minValue = decimal.MaxValue;
            int minIndex = 0;
            for (int i = 0; i < data.Length; i++)
            {
                var item = data[i];
                var spread = (decimal)item[3];

                if (spread < minValue)
                {
                    minValue = spread;
                    minIndex = i;
                }
            }

            return data[minIndex];
        }

        private static T[][] TrimArray<T>(T[][] data)
        {
            var length = 0;
            for (int i = 0; i < data.Length; i++)
            {
                var item = data[i];
                if (item == null)
                {
                    length = i;
                    break;
                }
            }

            var newData = new T[length][];
            for (int i = 0; i < length; i++)
            {
                newData[i] = data[i];
            }

            return newData;
        }
    }
}
