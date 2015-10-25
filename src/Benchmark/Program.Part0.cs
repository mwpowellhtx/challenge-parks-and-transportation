#if PART0

using System;
using LeftOrRight;

namespace Challenge.Core
{
    using System;
    using System.IO;

    public class Disposable : IDisposable
    {
        protected Disposable()
        {
        }

        ~Disposable()
        {
            Dispose(false);
        }

        private bool _disposed;

        protected bool IsDisposed
        {
            get { return _disposed; }
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            Dispose(true);
            _disposed = true;
        }
    }

    public abstract class ChallengeBase : Disposable
    {
        private readonly TextWriter _writer;

        protected ChallengeBase(TextReader reader, TextWriter writer)
        {
            _writer = writer;
            Initialize(reader);
        }

        private void Initialize(TextReader reader)
        {
            Read(reader);
        }

        protected abstract void Read(TextReader reader);

        protected virtual string[] ReadLines(TextReader reader)
        {
            return reader.ReadToEndAsync().Result.Replace("\r\n", "\n").Split('\n');
        }

        protected abstract void Report(TextWriter writer);

        protected abstract void Run();

        protected override void Dispose(bool disposing)
        {
            if (!disposing || IsDisposed) return;

            Run();

            Report(_writer);

            base.Dispose(true);
        }
    }
}

namespace LeftOrRight
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Challenge.Core;
    
    public static class ChallengeExtensionMethods
    {
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
        /// Verifies the range of <paramref name="value"/> is greater than or equal to
        /// <paramref name="min"/> and less than or equal to <paramref name="max"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static int VerifyRange(this int value, int min, int max)
        {
            // ReSharper disable once InvertIf
            if (value < min || value > max)
            {
                var message = string.Format("value {0} must be in the range ({1}, {2}]", value, min, max);
                throw new ArgumentException(message, "value");
            }
            return value;
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

    public class Route
    {
        private readonly Attractions _attractions;

        private int _current;

        public int Current
        {
            get { return _current; }
            private set { _current = value.VerifyDestination(_attractions); }
        }

        private int _destination;

        public int Destination
        {
            get { return _destination; }
            private set { _destination = value.VerifyDestination(_attractions); }
        }

        private int? _leastMinutes;

        /// <summary>
        /// Gets the LeastMinutes it should take to reach <see cref="Destination"/> starting
        /// from <see cref="Current"/>.
        /// </summary>
        public int LeastMinutes
        {
            get { return (_leastMinutes ?? (_leastMinutes = CalculateLeastMinutes())).Value; }
        }

        private int CalculateLeastMinutes()
        {
            return Math.Min(_attractions.GetLeftRouteItem(this), _attractions.GetRightRouteItem(this));
        }

        public Route(Attractions attractions, int current, int destination)
        {
            _attractions = attractions;
            Current = current;
            Destination = destination;
        }

        public bool IsDestinationLeft
        {
            get { return Destination < Current; }
        }

        public bool IsDestinationRight
        {
            get { return Destination > Current; }
        }
    }

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

    public class Attractions
    {
        private List<int> _minutesBetween;

        private static void VerifyTimeMinutes(int value)
        {
            if (value > 0) return;
            var message = string.Format("value {0} must be positive", value);
            throw new ArgumentException(message, "value");
        }

        /// <summary>
        /// Gets or sets the minutes between attractions. The i-th integer represents the time,
        /// in minutes, it takes for the average guest to walk from the i-th attraction to the
        /// (i + 1)-th attraction.
        /// </summary>
        private List<int> MinutesBetween
        {
            get { return _minutesBetween; }
            set
            {
                _minutesBetween = value;
                _minutesBetween.ForEach(VerifyTimeMinutes);
            }
        }

        public int Count
        {
            get { return MinutesBetween.Count; }
        }

        public Attractions(IEnumerable<int> minutesBetween)
        {
            MinutesBetween = minutesBetween.ToList();
        }

        public int GetLeftRouteItem(Route route)
        {
            return route.IsDestinationRight
                ? int.MaxValue
                : GetRouteItems(MinutesBetween, route.Destination, route.Current).Min();
        }

        public int GetRightRouteItem(Route route)
        {
            return route.IsDestinationLeft
                ? int.MaxValue
                : GetRouteItems(MinutesBetween, route.Current, route.Destination).Min();
        }

        public static IEnumerable<int> GetRouteItems(IEnumerable<int> values, int min, int max)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            yield return values.GetDirectLine(min, max).Sum();

            // ReSharper disable once PossibleMultipleEnumeration
            yield return values.GetIndirectLine(min, max).Sum();
        }

        public static Attractions Read(TextReader reader)
        {
            // ReadOne the number of attractions followed by the time between attractions.
            reader.ReadLineAsync().Result.ParseInteger().VerifyTimesCount();

            var times = from t in reader.ReadLineAsync().Result.Split(' ') select t.ParseInteger();

            return new Attractions(times);
        }
    }

        public class LeftOrRightChallenge : ChallengeBase
    {
        private Attractions _attractions;

        private List<Plan> _plans;

        public LeftOrRightChallenge(TextReader reader, TextWriter writer)
            : base(reader, writer)
        {
        }

        protected override void Read(TextReader reader)
        {
            _attractions = Attractions.Read(reader);
            _plans = Plan.ReadAll(reader, _attractions).ToList();
        }

        protected override void Run()
        {
            Action<Plan> mapRoutes = p => p.MapRoutes();
            _plans.ForEach(mapRoutes);
        }

        protected override void Report(TextWriter writer)
        {
            Action<Plan> reportRoutes = p => writer.WriteLine(p.TotalMinutes);
            _plans.ForEach(reportRoutes);
        }
    }
}

// ReSharper disable once CheckNamespace
public class Test
{
    public static void Main()
    {
        using (new LeftOrRightChallenge(Console.In, Console.Out))
        {
        }
    }
}

#endif
