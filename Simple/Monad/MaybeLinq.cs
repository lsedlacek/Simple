using System;

namespace Simple.Monad
{
    public static class MaybeLinq
    {
        public static Maybe<TResult> SelectMany<TSource, TResult>(this Maybe<TSource> source,
            Func<TSource, Maybe<TResult>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.HasValue
                ? selector(source.UnsafeValue)
                : Maybe<TResult>.Nothing;
        }

        public static Maybe<TResult> Select<TSource, TResult>(this Maybe<TSource> source,
            Func<TSource, TResult> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.HasValue
                ? Maybe.Return(selector(source.UnsafeValue))
                : Maybe<TResult>.Nothing;
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
                : Maybe<TSource>.Nothing;
        }
    }
}
