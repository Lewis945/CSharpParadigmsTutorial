using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OOP
{
    public class DataParser<T>
        where T : struct
    {
        private readonly Func<string[], bool> _filter;
        private readonly Func<string[], Func<string, string>, T> _map;
        private static Regex _digitsOnly = new Regex(@"[^\d]");

        public DataParser(Func<string[], bool> filter, Func<string[], Func<string, string>, T> map)
        {
            _filter = filter;
            _map = map;
        }

        public IEnumerable<T> Parse(IEnumerable<string[]> data) =>
            data
                .Where(l => _filter(l))
                .Select(l => _map(l, item => _digitsOnly.Replace(item, string.Empty)));
    }
}
