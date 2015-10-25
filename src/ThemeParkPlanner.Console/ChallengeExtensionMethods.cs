using System.Collections.Generic;
using System.Linq;
using Challenge.Core;

namespace ThemeParkPlanner
{
    public static class ChallengeExtensionMethods
    {
        public static int VerifyAttractionCount(this int value)
        {
            // Bear in mind (0, 1000)
            return value.VerifyRange(1, 999);
        }

        public static int VerifyParkHours(this int value)
        {
            // Bear in mind [1, 23]
            return value.VerifyRange(1, 23);
        }

        public static int VerifyQueryCount(this int value)
        {
            // Bear in mind [0, 100)
            return value.VerifyRange(0, 99);
        }

        public static int VerifyDesiredAttractions(this int value)
        {
            // Bear in mind [0, 20]
            return value.VerifyRange(0, 20);
        }

        private static class Permutation<T>
        {
            public static readonly IEnumerable<IEnumerable<T>> DefaultPermutation = new List<IEnumerable<T>>();
        }

        public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> values)
        {
            // ReSharper disable PossibleMultipleEnumeration
            if (!values.Any())
                return Permutation<T>.DefaultPermutation;

            if (values.Count() == 1)
                return new List<IEnumerable<T>> {values};

            var results = new List<IEnumerable<T>>();

            for (var i = 0; i < values.Count(); i++)
            {
                var x = new[] {values.ElementAt(i)};

                var remaining = values.Take(i).Concat(values.Skip(i + 1));

                results.AddRange(from p in remaining.Permute() select x.Concat(p));
            }

            return results;
        }
    }
}
