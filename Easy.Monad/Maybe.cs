using System;
using System.Collections.Generic;

namespace Easy.Monad
{
    public static class Maybe
    {
        public struct MaybeNothing
        { }

        public static readonly MaybeNothing Nothing = default(MaybeNothing);

        public static Maybe<T> Return<T>(T value)
        {
            return value != null
                ? new Maybe<T>(value)
                : Nothing;
        }

        public static Maybe<T> Return<T>(T? value)
            where T : struct
        {
            return value.HasValue
                ? new Maybe<T>(value.Value)
                : Nothing;
        }

        public static Maybe<T> Join<T>(Maybe<Maybe<T>> wrapped)
        {
            return wrapped.Bind(x => x);
        }
    }

    public struct Maybe<T> : IEquatable<Maybe<T>>, IComparable<Maybe<T>>
    {
        public Maybe(T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            HasValue = true;
            Value = value;
        }

        public static implicit operator Maybe<T>(Maybe.MaybeNothing dummy)
        {
            return default(Maybe<T>);
        }

        public bool HasValue { get; }
        private T Value { get; }

        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> func)
        {
            return HasValue
                ? func(Value)
                : default(Maybe<TResult>);
        }

        #region Comparison

        public int CompareTo(Maybe<T> other)
        {
            return HasValue
                ? other.HasValue
                    ? Comparer<T>.Default.Compare(Value, other.Value)
                    : 1
                : other.HasValue
                    ? -1
                    : 0;
        }

        public static bool operator >(Maybe<T> left, Maybe<T> right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <(Maybe<T> left, Maybe<T> right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >=(Maybe<T> left, Maybe<T> right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <=(Maybe<T> left, Maybe<T> right)
        {
            return left.CompareTo(right) <= 0;
        }

        #endregion

        #region Equality

        public bool Equals(Maybe<T> other)
        {
            return HasValue
                ? other.HasValue && EqualityComparer<T>.Default.Equals(Value, other.Value)
                : !other.HasValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Maybe<T> && Equals((Maybe<T>)obj);
        }

        public override int GetHashCode()
        {
            return HasValue
                ? EqualityComparer<T>.Default.GetHashCode(Value)
                : 0;
        }

        public static bool operator ==(Maybe<T> left, Maybe<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Maybe<T> left, Maybe<T> right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}
