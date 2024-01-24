using System;
using System.Collections.Generic;
using System.Linq;

namespace YoutubeDLSharp.Options
{
    public class MultiValue<T>(params T[] values)
    {
        private readonly List<T> values = [.. values];

        public List<T> Values => values;

        public static implicit operator MultiValue<T>(T value)
        {
            return new MultiValue<T>(value);
        }

        public static implicit operator MultiValue<T>(T[] values)
        {
            return new MultiValue<T>(values);
        }

        public static explicit operator T(MultiValue<T> value)
        {
            if (value.Values.Count == 1)
                return value.Values[0];
            else
                throw new InvalidCastException($"Cannot cast sequence of values to {typeof(T)}.");
        }

        public static explicit operator T[](MultiValue<T> value)
        {
            return [.. value.Values];
        }
    }
}
