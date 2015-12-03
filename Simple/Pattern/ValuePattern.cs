using System;
using System.Collections.Generic;
using Simple.Monad;

namespace Simple.Pattern
{
    public class ValuePattern<T, TResult>
    {
        private static TResult DefaultDefault(T value)
        {
            throw new InvalidOperationException($"Pattern was not matched by value {value}.");
        } 

        private Dictionary<T, Func<T, TResult>> FuncByValue { get; }
        private Func<T, TResult> DefaultFunc { get; } 

        public ValuePattern()
            : this(new Dictionary<T, Func<T, TResult>>(), DefaultDefault)
        { }

        private ValuePattern(Dictionary<T, Func<T, TResult>> funcByValue, Func<T, TResult> defaultFunc)
        {
            FuncByValue = funcByValue;
            DefaultFunc = defaultFunc;
        }

        public ValuePattern<T, TResult> If(T toMatch, Func<T, TResult> func)
        {
            var newFuncByValue = new Dictionary<T, Func<T, TResult>>(FuncByValue) {{toMatch, func}};
            return new ValuePattern<T, TResult>(newFuncByValue, DefaultFunc);
        }

        public TResult Match(T value)
        {
            var maybeFunc = FuncByValue.GetOrNothing(value);
            var func = maybeFunc.OrElse(DefaultFunc);

            return func(value);
        }

        public ValuePattern<T, TResult> Default(Func<T, TResult> func)
        {
            var funcByValue = new Dictionary<T, Func<T, TResult>>(FuncByValue);
            return new ValuePattern<T, TResult>(funcByValue, func);
        }
    }
}
