using System.Collections.Generic;
using System.IO;
using Challenge.Core;

namespace Transportation
{
    public class Patron : IDirectional, ITicketable
    {
        private readonly int _timeOfDayMinutes;

        public int DepartureTimeMinutes
        {
            get { return _timeOfDayMinutes; }
        }

        public int StopNumber { get; private set; }

        public int Direction { get; private set; }

        public Trip Itinerary { get; internal set; }

        public Patron(int timeOfDayMinutes, int stopNumber, int direction)
        {
            _timeOfDayMinutes = timeOfDayMinutes;
            StopNumber = stopNumber;
            Direction = direction;
        }

        public bool IsNorthbound
        {
            get { return Direction == TripConstraint.Northbound; }
        }

        public bool IsSouthbound
        {
            get { return Direction == TripConstraint.Southbound; }
        }

        private static Patron ReadOne(TextReader reader)
        {
            var line = reader.ReadLineAsync().Result;
            // Accounting for discrepancies in the input file where Count > the actual Count(Patron)
            if (string.IsNullOrEmpty(line)) return null;
            var parts = line.Split(' ');
            var timeOfDayMinutes = parts[0].ToMinutes();
            var stopNumber = parts[1].ParseInteger();
            var direction = TripConstraint.Directions[parts[2][0]];
            return new Patron(timeOfDayMinutes, stopNumber, direction);
        }

        public static IEnumerable<Patron> ReadAll(TextReader reader)
        {
            var line = reader.ReadLineAsync().Result;
            var count = line.ParseInteger();
            var result = new List<Patron>();
            while (count-- > 0)
            {
                var patron = ReadOne(reader);
                // Accounting for an error in the input file.
                if (patron == null) break;
                result.Add(patron);
            }
            return result;
        }
    }
}
