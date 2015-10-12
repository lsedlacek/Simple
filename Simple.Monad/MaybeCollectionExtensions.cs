using System;
using System.Collections.Generic;

namespace Simple.Monad
{
    public static class MaybeCollectionExtensions
    {
        public static Maybe<T> SingleOrNothing<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext()) return Maybe.Nothing;

                var value = enumerator.Current;
                if (!enumerator.MoveNext())
                {
                    return Maybe.Return(value);
                }

                throw new InvalidOperationException("More than one element in sequence.");
            }
        }

        public static Maybe<T> FirstOrNothing<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator())
            {
                return enumerator.MoveNext()
                    ? Maybe.Return(enumerator.Current)
                    : Maybe.Nothing;
            }
        }

        public static Maybe<TValue> GetOrNothing<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            return dictionary.ContainsKey(key)
                ? Maybe.Return(dictionary[key])
                : Maybe.Nothing;
        }
    }
}
