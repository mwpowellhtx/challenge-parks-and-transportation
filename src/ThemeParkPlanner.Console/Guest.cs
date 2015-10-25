using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Challenge.Core;

namespace ThemeParkPlanner
{
    public class Guest
    {
        private readonly IReadOnlyThemePark _themePark;

        public int EntryTimeMinutes { get; private set; }

        private List<int> _desired;

        public List<int> Desired
        {
            get { return _desired; }
            private set
            {
                _desired = value;

                /* Remember that if there are gaps with the hours then we have some choices
                 * which ones to prefer, and which spaces to meet otherwise. */

                while (_desired.Count < _themePark.MaxHoursPerDay)
                    _desired.Add(Constants.WaitTime);
            }
        }

        private static IEnumerable<int> VerifyDistinct(IEnumerable<int> values)
        {
            // ReSharper disable PossibleMultipleEnumeration

            var distinct = values.Distinct();

            if (distinct.Count() == values.Count())
                return distinct;

            throw new ArgumentException("values are not all distinct", "values")
            {
                Data = {{"values", values}}
            };
        }

        public Guest(int entryTimeMinutes, IReadOnlyThemePark themePark, IEnumerable<int> desired)
        {
            _themePark = themePark;
            EntryTimeMinutes = entryTimeMinutes;
            Desired = VerifyDistinct(desired).ToList();
        }

        public Visit BestPossibleVisit { get; private set; }

        /// <summary>
        /// Facilitates the Guest trying to visit the <see cref="_themePark"/>
        /// Returns whether this is possible.
        /// </summary>
        /// <returns></returns>
        public bool TryVisit()
        {
            var possible = Desired.Permute();

            BestPossibleVisit = (from x in possible
                let v = new Visit(this, _themePark, x)
                orderby v.TimeInPark
                select v).First();

            return false;
        }

        private static void VerifyWithinParkHours(int entryTimeMinutes, int maxHoursPerDay)
        {
            /* For anything more involved than this, start looking into
             * a proper dimensional analysis, units of measure solution. */

            if (entryTimeMinutes < maxHoursPerDay*Constants.MinutesPerHour)
                return;

            throw new ArgumentException("Cannot enter park after hours")
            {
                Data =
                {
                    // Hello C# 6.0 nameof ! or in this case, Mono 4
                    {"entryTimeMinutes", entryTimeMinutes},
                    {"maxHoursPerDay", maxHoursPerDay}
                }
            };
        }

        private static Guest ReadOne(TextReader reader, IReadOnlyThemePark themePark)
        {
            var lines = new[]
            {
                reader.ReadLineAsync().Result,
                reader.ReadLineAsync().Result,
                reader.ReadLineAsync().Result,
            };

            // TODO: may check hours are not after park closed
            var entryTimeMinutes = lines[0].ParseInteger();

            VerifyWithinParkHours(entryTimeMinutes, themePark.MaxHoursPerDay);

            var visitCount = lines[1].ParseInteger().VerifyDesiredAttractions();

            var visits = from x in lines[2].Split(' ')
                select x.ParseInteger().VerifyRange(0, themePark.ReadOnlyAttractions.Count - 1);

            // ReSharper disable once PossibleMultipleEnumeration
            visits.Count().VerifyValue(visitCount);

            // ReSharper disable once PossibleMultipleEnumeration
            return new Guest(entryTimeMinutes, themePark, visits);
        }

        public static IEnumerable<Guest> ReadAll(TextReader reader, int count, IReadOnlyThemePark themePark)
        {
            while (count-- > 0)
                yield return ReadOne(reader, themePark);
        }

        public void Report(TextWriter writer)
        {
            if (BestPossibleVisit.IsImpossible())
                writer.WriteLine("IMPOSSIBLE");
            else
                writer.WriteLine(BestPossibleVisit.TimeInPark);
        }
    }
}
