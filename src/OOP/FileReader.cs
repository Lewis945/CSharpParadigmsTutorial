using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OOP
{
    public class FileReader
    {
        private readonly char[] _separators;
        private readonly bool _ignoreHeaders;

        public FileReader(char[] separators, bool ignoreHeaders = false)
        {
            _separators = separators;
            _ignoreHeaders = ignoreHeaders;
        }

        public IEnumerable<string[]> Read(string path) =>
            File.ReadLines(path)
                .Where((item, index) => !_ignoreHeaders || index != 0)
                .Select(l => l.Split(_separators, StringSplitOptions.RemoveEmptyEntries))
                .Where(item => item.Length > 1);
    }
}
