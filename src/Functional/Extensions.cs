using System;
using System.Collections.Generic;

namespace Functional
{
    public static class Extensions
    {
        public static IEnumerable<TSource> Filter<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            int i = 0;
            foreach (TSource item in source)
            {
                if (predicate(item, i))
                {
                    yield return item;
                }
                i++;
            }
        }

        public static IEnumerable<TMap> Map<TSource, TMap>(
            this IEnumerable<TSource> source,
            Func<TSource, int, TMap> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            int i = 0;
            foreach (TSource item in source)
            {
                yield return selector(item, i);
                i++;
            }
        }

        public static TAggregate Reduce<TSource, TAggregate>(
            this IEnumerable<TSource> source,
            Func<TAggregate, TSource, int, TAggregate> aggregator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            TAggregate aggregate = default(TAggregate);
            int index = 0;
            foreach (var item in source)
            {
                aggregate = aggregator(aggregate, item, index);
            }

            return aggregate;
        }

        //public static int Count<TSource>(this IEnumerable<TSource> source)
        //{
        //    if (source == null)
        //        throw new ArgumentNullException(nameof(source));

        //    int index = 0;
        //    foreach (var item in source)
        //    {
        //        index++;
        //    }
        //    return index++;
        //}

        //public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source, int count)
        //{
        //    if (source == null)
        //        throw new ArgumentNullException(nameof(source));
        //    if (count < 0)
        //        throw new ArgumentOutOfRangeException(nameof(count));

        //    var array = new TSource[count];
        //    int i = 0;
        //    foreach (var item in source)
        //    {
        //        array[i++] = item;
        //    }
        //    return array;
        //}
    }
}
