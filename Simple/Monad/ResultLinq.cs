using System;

namespace Simple.Monad
{
    public static class ResultLinq
    {
        public static IResult<TResultValue, TError> Select<TValue, TError, TResultValue>(
            this IResult<TValue, TError> source,
            Func<TValue, TResultValue> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.MapBoth(
                selector,
                x => x);
        }

        public static IResult<TResultValue, TError> SelectMany<TValue, TError, TResultValue>(
            this IResult<TValue, TError> source,
            Func<TValue, IResult<TResultValue, TError>> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.Match(
                selector,
                Result.Fail<TResultValue, TError>);
        }

        public static IResult<TResultValue, TError> SelectMany<TValue, TError, TIntermediateValue, TResultValue>(
            this IResult<TValue, TError> source,
            Func<TValue, IResult<TIntermediateValue, TError>> innerSelector,
            Func<TValue, TIntermediateValue, TResultValue> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (innerSelector == null) throw new ArgumentNullException(nameof(innerSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return source.SelectMany(x => innerSelector(x).Select(inner => resultSelector(x, inner)));
        }
    }
}
