namespace LeftOrRight
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Plan
    {
        private readonly Attractions _attractions;

        private List<int> _destinations;

        private void VerifyDestination(int value)
        {
            value.VerifyDestination(_attractions);
        }

        private List<int> Destinations
        {
            get { return _destinations; }
            set
            {
                _destinations = value;
                _destinations.ForEach(VerifyDestination);
            }
        }

        private List<Route> _routes;

        public Plan(Attractions attractions, IEnumerable<int> destinations)
        {
            _attractions = attractions;
            Destinations = destinations.ToList();
        }

        private static Plan ReadOne(TextReader reader, Attractions attractions)
        {
            var count = reader.ReadLineAsync().Result
                .ParseInteger().VerifyDestinationCount(attractions);

            var destinations
                = from a in reader.ReadLineAsync().Result.Split(' ')
                    select a.ParseInteger().VerifyDestination(attractions);

            // ReSharper disable once PossibleMultipleEnumeration
            destinations.Count().VerifyValue(count);

            // ReSharper disable once PossibleMultipleEnumeration
            return new Plan(attractions, destinations);
        }

        public static IEnumerable<Plan> ReadAll(TextReader reader, Attractions attractions)
        {
            // Read the attraction queries.
            var count = reader.ReadLineAsync().Result.ParseInteger().VerifyQueryCount();

            while (count-- > 0)
                yield return ReadOne(reader, attractions);
        }

        public void MapRoutes()
        {
            var d = Destinations;
            _routes = (from i in Enumerable.Range(0, d.Count - 1)
                select new Route(_attractions, d[i], d[i + 1])).ToList();
        }

        private int? _totalMinutes;

        public int TotalMinutes
        {
            get { return (_totalMinutes ?? (_totalMinutes = _routes.Sum(r => r.LeastMinutes))).Value; }
        }
    }
}
