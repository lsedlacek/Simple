using System;
using System.Collections.Generic;

namespace Simple.Monad
{
    public static class Result
    {
        private sealed class Success<TValue, TError> : IResult<TValue, TError>, IEquatable<Success<TValue, TError>>
        {
            public Success(TValue value)
            {
                Value = value;
            }

            public TResult Match<TResult>(Func<TValue, TResult> fValue, Func<TError, TResult> fError)
            {
                if (fValue == null) throw new ArgumentNullException(nameof(fValue));
                if (fError == null) throw new ArgumentNullException(nameof(fError));

                return fValue(Value);
            }

            private TValue Value { get; }

            public bool Equals(Success<TValue, TError> other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return EqualityComparer<TValue>.Default.Equals(Value, other.Value);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj is Success<TValue, TError>
                    && Equals((Success<TValue, TError>)obj);
            }

            public override int GetHashCode()
            {
                return EqualityComparer<TValue>.Default.GetHashCode(Value);
            }
        }

        private sealed class Failure<TValue, TError> : IResult<TValue, TError>, IEquatable<Failure<TValue, TError>>
        {
            public Failure(TError error)
            {
                Error = error;
            }

            public TResult Match<TResult>(Func<TValue, TResult> fValue, Func<TError, TResult> fError)
            {
                if (fValue == null) throw new ArgumentNullException(nameof(fValue));
                if (fError == null) throw new ArgumentNullException(nameof(fError));

                return fError(Error);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj is Failure<TValue, TError>
                    && Equals((Failure<TValue, TError>)obj);
            }

            public override int GetHashCode()
            {
                return EqualityComparer<TError>.Default.GetHashCode(Error);
            }

            private TError Error { get; }

            public bool Equals(Failure<TValue, TError> other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return EqualityComparer<TError>.Default.Equals(Error, other.Error);
            }
        }

        public static IResult<TValue, TError> Return<TValue, TError>(TValue value)
            => new Success<TValue, TError>(value);

        public static IResult<TValue, TError> Fail<TValue, TError>(TError error)
            => new Failure<TValue, TError>(error);

        public static IResult<TValue, TError> Append<TValue, TError>(
            this IResult<TValue, TError> left, IResult<TValue, TError> right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            return left.Match(
                val => left,
                err => right);
        }

        public static IResult<TValue, TError> Where<TValue, TError>(this IResult<TValue, TError> source,
            Func<TValue, bool> predicate, TError ifFalse)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return source.SelectMany(
                value => predicate(value)
                    ? source
                    : Fail<TValue, TError>(ifFalse));
        }

        public static IResult<TValue, TError> Where<TValue, TError>(this IResult<TValue, TError> source,
            Func<TValue, bool> predicate, Func<TValue, TError> ifFalse)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return source.SelectMany(
                value => predicate(value)
                    ? source
                    : Fail<TValue, TError>(ifFalse(value)));
        }

        public static IResult<TValue, TError> OrFail<TValue, TError>(this Maybe<TValue> maybe, TError error)
        {
            return maybe.HasValue
                ? Return<TValue, TError>(maybe.UnsafeValue)
                : Fail<TValue, TError>(error);
        }

        public static IResult<TValue, TError> OrFail<TValue, TError>(this Maybe<TValue> maybe, Func<TError> error)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));

            return maybe.HasValue
                ? Return<TValue, TError>(maybe.UnsafeValue)
                : Fail<TValue, TError>(error());
        }

        public static IResult<TValue, TResultError> MapError<TValue, TError, TResultError>(
            this IResult<TValue, TError> source,
            Func<TError, TResultError> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.MapBoth(
                x => x,
                selector);
        }

        public static IResult<TResultValue, TResultError> MapBoth<TValue, TError, TResultValue, TResultError>(
            this IResult<TValue, TError> source,
            Func<TValue, TResultValue> valueSelector,
            Func<TError, TResultError> errorSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (valueSelector == null) throw new ArgumentNullException(nameof(valueSelector));
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));

            return source.Match(
                value => Return<TResultValue, TResultError>(valueSelector(value)),
                error => Fail<TResultValue, TResultError>(errorSelector(error)));
        }
    }

    public interface IResult<out TValue, out TError>
    {
        TResult Match<TResult>(Func<TValue, TResult> fValue, Func<TError, TResult> fError);
    }
}
