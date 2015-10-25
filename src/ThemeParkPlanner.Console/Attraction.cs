using System.Collections.Generic;
using System.IO;
using System.Linq;
using Challenge.Core;

namespace ThemeParkPlanner
{
    public class Attraction
    {
        /// <summary>
        /// Gets the QueueTimes indexed by the hour in which the time occurs.
        /// </summary>
        public IReadOnlyDictionary<int, int> QueueTimes { get; private set; }

        public Attraction(IEnumerable<int> queueTimes)
        {
            // ReSharper disable PossibleMultipleEnumeration
            QueueTimes = Enumerable.Range(0, queueTimes.Count())
                .ToDictionary(i => i, queueTimes.ElementAt);
        }

        public bool TryGetQueueTime(int timeMinutes, out int queueTime)
        {
            var hour = timeMinutes/Constants.MinutesPerHour;
            return QueueTimes.TryGetValue(hour, out queueTime);
        }

        private static Attraction ReadOne(TextReader reader)
        {
            var queueTimes = from x in reader.ReadLineAsync().Result.Split(' ')
                select x.ParseInteger();
            return new Attraction(queueTimes);
        }

        public static IEnumerable<Attraction> ReadAll(TextReader reader, int count)
        {
            while (count-- > 0)
                yield return ReadOne(reader);
        }
    }
}
