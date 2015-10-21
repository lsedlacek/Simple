using System;
using System.Collections.Generic;

namespace Simple.Monad
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

        public static Maybe<T> Join<T>(this Maybe<Maybe<T>> wrapped)
        {
            return wrapped.Bind(x => x);
        }

        public static T OrElse<T>(this Maybe<T> maybe, T value)
        {
            return maybe.HasValue
                ? maybe.UnsafeValue
                : value;
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

        public T UnsafeValue
        {
            get
            {
                if (HasValue) return Value;
                throw new InvalidOperationException("Nothing has no value.");
            }
        }

        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> func)
        {
            return HasValue
                ? func(Value)
                : default(Maybe<TResult>);
        }

        #region Comparison

        public int CompareTo(Maybe<T> other)
        {
            var flag = (HasValue ? 1 : 0)
                | (other.HasValue ? 2 : 0);

            switch (flag)
            {
                case 0: // both nothing
                    return 0;
                case 1: // first something
                    return 1;
                case 2: // second something
                    return -1;
                default: // case 3: // both something
                    return Comparer<T>.Default.Compare(Value, other.Value);
            }
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
            var flag = (HasValue ? 1 : 0)
                | (other.HasValue ? 2 : 0);

            switch (flag)
            {
                case 0: // both nothing
                    return true;
                case 1: // first something
                case 2: // second something
                    return false;
                default: // case 3: // both something
                    return EqualityComparer<T>.Default.Equals(Value, other.Value);
            }
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
