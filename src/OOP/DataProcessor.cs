using System;
using System.Collections.Generic;
using System.Linq;

namespace OOP
{
    public class DataProcessor<T>
        where T : struct
    {
        private readonly Func<T, T, T> _getter;

        public DataProcessor(Func<T, T, T> getter)
        {
            _getter = getter;
        }

        public T GetMinSpreadItem(IEnumerable<T> data) =>
            data.Aggregate((ac, item) => _getter(ac, item));
    }
}
