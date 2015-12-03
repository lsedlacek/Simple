using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Simple.Misc
{
    public static class EnumExtensions
    {
        public static bool HasFlags<TEnum>(this TEnum value, TEnum flags) where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            return Impl<TEnum>.HasFlagsDelegate(value, flags);
        }

        [SuppressMessage("ReSharper", "RedundantCaseLabel", Justification = "Communicates intent.")]
        [SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases", Justification = "Other TypeCodes will not make it here.")]
        private static Type GetUnderlyingIntegralType(Type type)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                    return typeof(byte);
                case TypeCode.SByte:
                    return typeof(sbyte);
                case TypeCode.UInt16:
                    return typeof(ushort);
                case TypeCode.Int16:
                    return typeof(short);
                case TypeCode.UInt32:
                    return typeof(uint);
                case TypeCode.Int32:
                    return typeof(int);
                case TypeCode.UInt64:
                    return typeof(ulong);
                case TypeCode.Int64:
                default:
                    return typeof(long);
            }
        }

        private static class Impl<TEnum> where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            public static Func<TEnum, TEnum, bool> HasFlagsDelegate { get; } = CreateHasFlagDelegate();

            private static Func<TEnum, TEnum, bool> CreateHasFlagDelegate()
            {
                var enumType = typeof(TEnum);

                if (!enumType.IsEnum)
                {
                    throw new ArgumentException($"{enumType} is not an enum.", nameof(TEnum));
                }

                var value = Expression.Parameter(enumType);
                var flag = Expression.Parameter(enumType);
                var convertedFlag = Expression.Variable(GetUnderlyingIntegralType(enumType));
                var lambda = Expression.Lambda<Func<TEnum, TEnum, bool>>(
                    Expression.Block(
                        new[] { convertedFlag },
                        Expression.Assign(
                            convertedFlag,
                            Expression.Convert(
                                flag,
                                convertedFlag.Type
                            )
                        ),
                        Expression.Equal(
                            Expression.And(
                                Expression.Convert(
                                    value,
                                    convertedFlag.Type
                                ),
                                convertedFlag
                            ),
                            convertedFlag
                        )
                    ),
                    value,
                    flag
                );
                return lambda.Compile();
            }
        }
    }
}
