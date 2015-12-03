using System;

namespace Simple.Monad
{
    public static partial class Maybe
    {
        public static Maybe<TResult> SelectMany<TSource, TResult>(this Maybe<TSource> source,
            Func<TSource, Maybe<TResult>> selector) => source.Bind(selector);

        public static Maybe<TResult> Select<TSource, TResult>(this Maybe<TSource> source,
            Func<TSource, TResult> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.Bind(x => Return(selector(x)));
        }

        public static Maybe<TResult> SelectMany<TSource, TInner, TResult>(
            this Maybe<TSource> source,
            Func<TSource, Maybe<TInner>> innerSelector,
            Func<TSource, TInner, TResult> resultSelector)
        {
            if (innerSelector == null) throw new ArgumentNullException(nameof(innerSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return source.Bind(x => innerSelector(x).Select(inner => resultSelector(x, inner)));
        }

        public static Maybe<TSource> Where<TSource>(this Maybe<TSource> source, Func<TSource, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return source.Bind(x => predicate(x) ? source : Nothing);
        }
    }
}
