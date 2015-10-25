using Challenge.Core;

namespace LeftOrRight
{
    using System.Collections.Generic;
    using System.Linq;

    public static class ChallengeExtensionMethods
    {
        public static int ParseInteger(this string text)
        {
            return int.Parse(text);
        }

        public static int VerifyTimesCount(this int value)
        {
            // Bear in mind (0, 100]
            return value.VerifyRange(1, 100);
        }

        public static int VerifyQueryCount(this int value)
        {
            // Bear in mind [1, 100)
            return value.VerifyRange(0, 99);
        }

        public static int VerifyDestinationCount(this int value, Attractions attractions)
        {
            // Bear in mind (0, attractions.Count]
            return value.VerifyRange(1, attractions.Count);
        }

        public static int VerifyDestination(this int value, Attractions attractions)
        {
            // Bear in mind (0, attractions.Count]
            return value.VerifyRange(0, attractions.Count - 1);
        }

        /// <summary>
        /// Returns the direct line between the <paramref name="min"/> and <paramref name="max"/>
        /// items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetDirectLine<T>(this IEnumerable<T> values, int min, int max)
        {
            // For example:
            //   D     C
            // 0 1 2 3 4 5 6
            return values.Skip(min).Take(max - min);
        }

        /// <summary>
        /// Returns an indirect line spanning the boundary from the last item, circling around to
        /// the first item. Always presents the list in left to right order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetIndirectLine<T>(this IEnumerable<T> values, int min, int max)
        {
            // For example:
            //     C     D
            // 0 1 2 3 4 5 6

            // ReSharper disable once PossibleMultipleEnumeration
            var part = values.Take(min);

            // ReSharper disable once PossibleMultipleEnumeration
            return values.Skip(max).Concat(part);
        }
    }
}
