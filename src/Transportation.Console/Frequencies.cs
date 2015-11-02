using System.Collections.Generic;
using System.IO;
using System.Linq;
using Challenge.Core;

namespace Transportation
{
    public class Frequencies
    {
        public const int Indefinite = int.MinValue;

        public IDictionary<int, int> Entries { get; private set; }

        public Frequencies(IDictionary<int, int> entries = null)
        {
            Entries = entries ?? new Dictionary<int, int>();
        }

        private int? AlignStartTime(int minute)
        {
            //TODO: may need to further align minute with the day-spanning clock
            var times = from k in Entries.Keys where k <= minute select k;
            // ReSharper disable PossibleMultipleEnumeration
            return !times.Any() ? (int?) null : times.Last();
        }

        public int GetWaitTimeMinutes(int startTimeMinutes)
        {
            var alignedTimeMinutes = AlignStartTime(startTimeMinutes);
            return alignedTimeMinutes == null ? Indefinite : Entries[alignedTimeMinutes.Value];
        }

        private static Frequencies ReadAll(TextReader reader, int count, Frequencies frequencies)
        {
            /* The order in which these are read can be peculiar. Watch for the roll over of the
             * clock to happen when the value is suddenly less than the former minimum. And even
             * this is not a great way of doing it without also incorporating some notion of the
             * date itself. */

            const int minutesPerDay = Constants.MinutesPerDay;

            int? previousTimeMinutes = null;

            while (count-- > 0)
            {
                var line = reader.ReadLineAsync().Result;

                var parts = line.Split(' ');

                var startingTimeMinutes = parts[0].ToMinutes();

                // Normalize when the previous was greater than this one.
                if (previousTimeMinutes > startingTimeMinutes)
                    startingTimeMinutes += minutesPerDay;

                var waitTimeMinutes = parts[1].ParseInteger();

                frequencies.Entries[startingTimeMinutes] = waitTimeMinutes;

                previousTimeMinutes = startingTimeMinutes;
            }

            return frequencies;
        }

        public static Frequencies Read(TextReader reader)
        {
            var line = reader.ReadLineAsync().Result;
            var count = line.ParseInteger();
            return ReadAll(reader, count, new Frequencies());
        }
    }
}
