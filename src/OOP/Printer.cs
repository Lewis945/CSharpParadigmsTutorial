using System;
using System.Linq;

namespace OOP
{
    public class Printer<T>
        where T : struct
    {
        private readonly string _filePath;
        private readonly FileReader _reader;
        private readonly DataParser<T> _parser;
        private readonly DataProcessor<T> _processor;
        private readonly Action<T> _printItem;
        private readonly Action<T> _printMinSpreadItem;

        public Printer(string path, FileReader reader, DataParser<T> parser, DataProcessor<T> processor, Action<T> printItem, Action<T> printMinSpreadItem)
        {
            _filePath = path;
            _reader = reader;
            _parser = parser;
            _processor = processor;
            _printItem = printItem;
            _printMinSpreadItem = printMinSpreadItem;
        }

        public void Print()
        {
            var data = _reader.Read(_filePath);
            var parsedData = _parser.Parse(data).ToList();

            Console.WriteLine("File data:");

            foreach (var item in parsedData)
            {
                _printItem(item);
            }

            _printMinSpreadItem(_processor.GetMinSpreadItem(parsedData));
        }
    }
}
