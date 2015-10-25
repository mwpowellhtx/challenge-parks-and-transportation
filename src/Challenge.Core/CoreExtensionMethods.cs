using System;
using System.Collections.Generic;
using System.Linq;

namespace Challenge.Core
{
    public static class CoreExtensionMethods
    {
        /// <summary>
        /// Returns a parsed integer given <paramref name="text"/>.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int ParseInteger(this string text)
        {
            return int.Parse(text);
        }

        public static int VerifyValue(this int value, int expected)
        {
            // ReSharper disable once InvertIf
            if (value != expected)
            {
                var message = string.Format("value {0} is not expected {1}", value, expected);
                throw new ArgumentException(message, "value");
            }
            return value;
        }

        /// <summary>
        /// Verifies the range of <paramref name="value"/> is greater than or equal to
        /// <paramref name="min"/> and less than or equal to <paramref name="max"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int VerifyRange(this int value, int min, int max)
        {
            if (value >= min && value <= max) return value;
            var message = string.Format("value {0} must be in the range ({1}, {2}]", value, min, max);
            throw new ArgumentException(message, "value");
        }

        public static IEnumerable<T> TrimRight<T>(this IEnumerable<T> values, T value)
            where T : IComparable
        {
            var result = new List<T>();

            foreach (var x in from reversed in values.Reverse()
                where reversed.CompareTo(value) != 0 || result.Any()
                select reversed)
            {
                result.Insert(0, x);
            }

            return result;
        }
    }
}
