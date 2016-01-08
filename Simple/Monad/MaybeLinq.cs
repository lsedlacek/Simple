using System;

namespace Simple.Monad
{
    public static partial class Maybe
    {
        public static Maybe<TResult> SelectMany<TSource, TResult>(this Maybe<TSource> source,
            Func<TSource, Maybe<TResult>> selector)
        {
            return source.HasValue
                ? selector(source.UnsafeValue)
                : Nothing<TResult>();
        }

        public static Maybe<TResult> Select<TSource, TResult>(this Maybe<TSource> source,
            Func<TSource, TResult> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.HasValue
                ? Return(selector(source.UnsafeValue))
                : Nothing<TResult>();
        }

        public static Maybe<TResult> SelectMany<TSource, TInner, TResult>(
            this Maybe<TSource> source,
            Func<TSource, Maybe<TInner>> innerSelector,
            Func<TSource, TInner, TResult> resultSelector)
        {
            if (innerSelector == null) throw new ArgumentNullException(nameof(innerSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return source.SelectMany(x => innerSelector(x).Select(y => resultSelector(x, y)));
        }

        public static Maybe<TSource> Where<TSource>(this Maybe<TSource> source, Func<TSource, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return source.HasValue && predicate(source.UnsafeValue)
                ? source
                : Nothing<TSource>();
        }

        public static Maybe<TSource> Concat<TSource>(this Maybe<TSource> source, Maybe<TSource> next)
        {
            return source.HasValue
                ? source
                : next;
        }
    }
}
