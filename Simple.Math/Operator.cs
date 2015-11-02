using System;
using System.Linq.Expressions;

namespace Simple.Math
{
    public static class Operator
    {
        #region Arithmetic

        public static T Add<T>(T x, T y)
        {
            return AddImpl<T>.Func(x, y);
        }

        private static class AddImpl<T>
        {
            public static readonly Func<T, T, T> Func = BuildBinary<T, T>(Expression.Add);
        }

        public static T Subtract<T>(T x, T y)
        {
            return SubtractImpl<T>.Func(x, y);
        }

        private static class SubtractImpl<T>
        {
            public static readonly Func<T, T, T> Func = BuildBinary<T, T>(Expression.Subtract);
        }

        public static T Multiply<T>(T x, T y)
        {
            return MultiplyImpl<T>.Func(x, y);
        }

        private static class MultiplyImpl<T>
        {
            public static readonly Func<T, T, T> Func = BuildBinary<T, T>(Expression.Multiply);
        }

        public static T Divide<T>(T x, T y)
        {
            return DivideImpl<T>.Func(x, y);
        }

        private static class DivideImpl<T>
        {
            public static readonly Func<T, T, T> Func = BuildBinary<T, T>(Expression.Divide);
        }

        public static T Modulo<T>(T x, T y)
        {
            return ModuloImpl<T>.Func(x, y);
        }

        private static class ModuloImpl<T>
        {
            public static readonly Func<T, T, T> Func = BuildBinary<T, T>(Expression.Modulo);
        }

        public static T Not<T>(T x)
        {
            return NotImpl<T>.Func(x);
        }

        private static class NotImpl<T>
        {
            public static readonly Func<T, T> Func = BuildUnary<T>(Expression.Not);
        }

        #endregion Arithmetic

        #region Boolean

        public static bool AndAlso(bool x, bool y)
        {
            return x && y;
        }

        public static bool OrElse(bool x, bool y)
        {
            return x || y;
        }

        public static bool Nand(bool x, bool y)
        {
            return (!x || !y);
        }

        public static bool Nor(bool x, bool y)
        {
            return !x && !y;
        }

        #endregion Boolean

        #region Binary

        public static T BitwiseAnd<T>(T x, T y)
        {
            return BitwiseAndImpl<T>.Func(x, y);
        }

        private static class BitwiseAndImpl<T>
        {
            public static readonly Func<T, T, T> Func = BuildBinary<T, T>(Expression.And);
        }

        public static T BitwiseOr<T>(T x, T y)
        {
            return BitwiseOrImpl<T>.Func(x, y);
        }

        private static class BitwiseOrImpl<T>
        {
            public static readonly Func<T, T, T> Func = BuildBinary<T, T>(Expression.Or);
        }

        public static T Xor<T>(T x, T y)
        {
            return XorImpl<T>.Func(x, y);
        }

        private static class XorImpl<T>
        {
            public static readonly Func<T, T, T> Func = BuildBinary<T, T>(Expression.ExclusiveOr);
        }

        public static T BitwiseNot<T>(T x)
        {
            return BitwiseNotImpl<T>.Func(x);
        }

        private static class BitwiseNotImpl<T>
        {
            public static readonly Func<T, T> Func = BuildUnary<T>(Expression.OnesComplement);
        }

        #endregion Binary

        #region Comparison

        public static bool Equal<T>(T x, T y)
        {
            return EqualImpl<T>.Func(x, y);
        }

        private static class EqualImpl<T>
        {
            public static readonly Func<T, T, bool> Func = BuildBinary<T, bool>(Expression.Equal);
        }

        public static bool NotEqual<T>(T x, T y)
        {
            return NotEqualImpl<T>.Func(x, y);
        }

        private static class NotEqualImpl<T>
        {
            public static readonly Func<T, T, bool> Func = BuildBinary<T, bool>(Expression.NotEqual);
        }

        public static bool GreaterThanOrEqual<T>(T x, T y)
        {
            return GreaterThanOrEqualImpl<T>.Func(x, y);
        }

        private static class GreaterThanOrEqualImpl<T>
        {
            public static readonly Func<T, T, bool> Func = BuildBinary<T, bool>(Expression.GreaterThanOrEqual);
        }

        public static bool LessThanOrEqual<T>(T x, T y)
        {
            return LessThanOrEqualImpl<T>.Func(x, y);
        }

        private static class LessThanOrEqualImpl<T>
        {
            public static readonly Func<T, T, bool> Func = BuildBinary<T, bool>(Expression.LessThanOrEqual);
        }

        public static bool GreaterThan<T>(T x, T y)
        {
            return GreaterThanImpl<T>.Func(x, y);
        }

        private static class GreaterThanImpl<T>
        {
            public static readonly Func<T, T, bool> Func = BuildBinary<T, bool>(Expression.GreaterThan);
        }

        public static bool LessThan<T>(T x, T y)
        {
            return LessThanImpl<T>.Func(x, y);
        }

        private static class LessThanImpl<T>
        {
            public static readonly Func<T, T, bool> Func = BuildBinary<T, bool>(Expression.LessThan);
        }

        #endregion Comparison

        private static Func<T, T> BuildUnary<T>(Func<Expression, UnaryExpression> unaryExpressionFactory)
        {
            var x = Expression.Parameter(typeof(T), "x");
            var body = unaryExpressionFactory(x);
            var lambda = Expression.Lambda<Func<T, T>>(body, x);
            return lambda.Compile();
        } 

        private static Func<T, T, TResult> BuildBinary<T, TResult>(
            Func<Expression, Expression, BinaryExpression> binaryExpressionFactory)
        {
            var x = Expression.Parameter(typeof(T), "x");
            var y = Expression.Parameter(typeof(T), "y");
            var body = binaryExpressionFactory(x, y);
            var lambda = Expression.Lambda<Func<T, T, TResult>>(body, x, y);
            return lambda.Compile();
        }
    }
}
