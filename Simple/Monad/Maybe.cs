using System;
using System.Collections.Generic;

namespace Simple.Monad
{
    public static class Maybe
    {
        public static Maybe<T> Return<T>(T value)
            => new Maybe<T>(value);

        public static Maybe<T> OfNullable<T>(T nullable)
            where T : class
            => nullable != null
                ? Return(nullable)
                : Maybe<T>.Nothing;

        public static Maybe<T> OfNullable<T>(T? nullable)
            where T : struct
            => nullable.HasValue
                ? Return(nullable.Value)
                : Maybe<T>.Nothing;

        public static TResult Match<TSource, TResult>(this Maybe<TSource> maybe, Func<TSource, TResult> fValue,
            Func<TResult> fNothing)
        {
            if (fValue == null) throw new ArgumentNullException(nameof(fValue));
            if (fNothing == null) throw new ArgumentNullException(nameof(fNothing));


            return maybe.HasValue
                ? fValue(maybe.UnsafeValue)
                : fNothing();
        }

        public static Maybe<T> Join<T>(this Maybe<Maybe<T>> wrapped)
            => wrapped.SelectMany(x => x);

        public static T OrElse<T>(this Maybe<T> maybe, T value)
            => maybe.HasValue
                ? maybe.UnsafeValue
                : value;

        public static T OrElse<T>(this Maybe<T> maybe, Func<T> factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            return maybe.HasValue
                ? maybe.UnsafeValue
                : factory();
        }

        public static Maybe<TSource> Append<TSource>(this Maybe<TSource> source, Maybe<TSource> next)
            => source.HasValue
                ? source
                : next;

        public static IEnumerable<T> AsEnumerable<T>(this Maybe<T> maybe)
        {
            if (maybe.HasValue)
            {
                yield return maybe.UnsafeValue;
            }
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

        public static Maybe<T> Nothing => default(Maybe<T>);

        public bool HasValue { get; }
        private T Value { get; }

        internal T UnsafeValue
        {
            get
            {
                if (HasValue) return Value;
                throw new InvalidOperationException("Nothing has no value.");
            }
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

        public static bool operator>(Maybe<T> left, Maybe<T> right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator<(Maybe<T> left, Maybe<T> right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator>=(Maybe<T> left, Maybe<T> right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator<=(Maybe<T> left, Maybe<T> right)
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

        public static bool operator==(Maybe<T> left, Maybe<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator!=(Maybe<T> left, Maybe<T> right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}
