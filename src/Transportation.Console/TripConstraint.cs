using System.Collections.Generic;
using System.IO;

namespace Transportation
{
    public class TripConstraint : IDirectional
    {
        public static readonly IDictionary<char, int> Directions;

        static TripConstraint()
        {
            Directions = new Dictionary<char, int>
            {
                {'N', Northbound},
                {'S', Southbound}
            };
        }

        /// <summary>
        /// By definition if Train is Northbound this means that TripConstraint origination
        /// is Southmost Stationn.
        /// </summary>
        public const int Northbound = int.MaxValue;

        /// <summary>
        /// By definition if Train is Southbound this means that TripConstraint origination
        /// is Northmost Stationn.
        /// </summary>
        public const int Southbound = int.MinValue;

        public int Direction { get; private set; }

        public int FirstDepartureTimeMinutes { get; private set; }

        private int _maxAllowableDepartureTimeMinutes;

        /// <summary>
        /// Gets the normalized MaxAllowableDepartureTimeMinutes adjusted for times spanning
        /// midnight hours of operation.
        /// </summary>
        public int MaxAllowableDepartureTimeMinutes
        {
            get { return _maxAllowableDepartureTimeMinutes; }
            private set { _maxAllowableDepartureTimeMinutes = Normalize(value); }
        }

        public Frequencies Frequencies { get; set; }

        public TripConstraint(int direction, int firstDepartureTimeMinutes, int maxAllowableDepartureTimeMinutes)
        {
            Direction = direction;
            FirstDepartureTimeMinutes = firstDepartureTimeMinutes;
            MaxAllowableDepartureTimeMinutes = maxAllowableDepartureTimeMinutes;
        }

        /// <summary>
        /// Returns the <paramref name="desiredTimeMinutes"/> adjusted for spanning midnight
        /// hours of operation.
        /// </summary>
        /// <param name="desiredTimeMinutes"></param>
        /// <returns></returns>
        public int Normalize(int desiredTimeMinutes)
        {
            var first = FirstDepartureTimeMinutes;

            var offset = desiredTimeMinutes >= first
                ? 0
                : Constants.MinutesPerDay;

            return desiredTimeMinutes + offset;
        }

        public bool IsNorthbound
        {
            get { return Direction == Northbound; }
        }

        public bool IsSouthbound
        {
            get { return Direction == Southbound; }
        }

        private static TripConstraint ReadOne(TextReader reader, int destination)
        {
            var line = reader.ReadLineAsync().Result;
            var parts = line.Split(' ');
            var departureTimeMinutes = parts[0].ToMinutes();
            var maxAllowableDepartureTimeMinutes = parts[1].ToMinutes();
            return new TripConstraint(destination, departureTimeMinutes, maxAllowableDepartureTimeMinutes);
        }

        public static IEnumerable<TripConstraint> ReadAll(TextReader reader)
        {
            /* Avoid yield return in this instance as it was causing issues requiring a ToArray or
             * something to force evaluation. Instead do it this way in order to ensure that the
             * read really does happen in the prescribed order. */

            return new[]
            {
                ReadOne(reader, Northbound),
                ReadOne(reader, Southbound)
            };
        }
    }
}
