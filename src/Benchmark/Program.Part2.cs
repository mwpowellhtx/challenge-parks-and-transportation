using System;
using System.Collections.Generic;
using System.IO;
using Transportation;

#if PART2

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
    }
}

namespace Transportation
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Challenge.Core;

    public static class Constants
    {
        /// <summary>
        /// Added for increased clarity.
        /// </summary>
        public const int MinutesPerHour = 60;

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Refactored from MINUTES_IN_DAY. Additionally, this can be a constant.</remarks>
        public const int MinutesPerDay = 24 * MinutesPerHour;
    }

    public static class ChallengeExtensionMethods
    {
        /// <summary>
        /// Converts a string in the format hh:mm (of a 24-hour clock)
        /// to minutes past midnight.  For example, 01:30 becomes 90.
        /// </summary>
        /// <param name="hhmm">A string in the format hh:mm such as 23:41</param>
        /// <returns>How many minutes past midnight have elapsed</returns>
        /// <remarks>Reformatted comments for increased end-user clarity.</remarks>
        public static int ToMinutes(this string hhmm)
        {
            var parts = hhmm.Split(':');
            // Refactored for increased clarity.
            return parts[0].ParseInteger()
                   * Constants.MinutesPerHour
                   + parts[1].ParseInteger();
        }

        /// <summary>
        /// Converts a number of minutes past midnight into a String representation
        /// in the format hh:mm of a 24-hour clock.  For example, 90 becomes 01:30.
        /// </summary>
        /// <param name="minutes">Minutes time past midnight</param>
        /// <returns>The time in format h:mm or hh:mm, such as 23:41</returns>
        /// <remarks>Reformatted comments for increased end-user clarity.</remarks>
        public static string FormatMinutes(this int minutes)
        {
            var h = minutes / 60;
            var m = minutes % 60;
            // This reads a lot easier plus we are using built in formatters.
            return string.Format("{0:D2}:{1:D2}", h, m);
        }

        /// <summary>
        /// Delegate the callback requesting stop intervals, in minutes, given a
        /// <paramref name="patron"/>.
        /// </summary>
        /// <param name="patron"></param>
        /// <returns></returns>
        public delegate IEnumerable<int> GetStopIntervalsMinutesDelegate(Patron patron);

        /// <summary>
        /// Returns whether the <paramref name="trip"/> CanBeScheduled for the
        /// <paramref name="patron"/>, taking into consideration any <paramref name="deltas"/>
        /// incurred by boarding mid-line.
        /// </summary>
        /// <param name="trip"></param>
        /// <param name="patron"></param>
        /// <param name="deltas"></param>
        /// <returns></returns>
        public static bool CanBeScheduled(this Trip trip, Patron patron, GetStopIntervalsMinutesDelegate deltas)
        {
            // Deal with these couple of obvious criteria first.
            if (trip == null || trip.Direction != patron.Direction)
                return false;

            var totalDeltaMinutes = deltas(patron).Sum();

            /* Now we can deal with the next obvious condition. Do not normalize the Patron time
             * after all. Not only is this acceptable as-is, I truly don't believe there is a way
             * for us to know otherwise; unless we wanted to incorporate actual DateTime dates
             * into the mix. */

            return trip.DepartureTimeMinutes + totalDeltaMinutes >= patron.DepartureTimeMinutes;
        }

        /// <summary>
        /// Returns whether the <paramref name="trip"/> CannotBeScheduled for the
        /// <paramref name="patron"/>, taking into consideration any <paramref name="deltas"/>
        /// incurred by boarding mid-line.
        /// </summary>
        /// <param name="trip"></param>
        /// <param name="patron"></param>
        /// <param name="deltas"></param>
        /// <returns></returns>
        public static bool CannotBeScheduled(this Trip trip, Patron patron, GetStopIntervalsMinutesDelegate deltas)
        {
            return !trip.CanBeScheduled(patron, deltas);
        }
    }

    public interface IDirectional
    {
        bool IsNorthbound { get; }

        bool IsSouthbound { get; }
    }

    public interface ITicketable
    {
        int Direction { get; }

        int DepartureTimeMinutes { get; }
    }

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
            var times = from k in Entries.Keys where k < minute select k;
            // ReSharper disable PossibleMultipleEnumeration
            return !times.Any() ? (int?)null : times.Last();
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

    public class Trip : IDirectional, ITicketable
    {
        public TripConstraint Constraint { get; private set; }

        public int Direction
        {
            get { return Constraint.Direction; }
        }

        public int DepartureTimeMinutes { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="constraint"></param>
        /// <param name="departureTimeMinutes"></param>
        /// <remarks>Constraints are another area that I was dubious about. However, it makes
        /// sense for the trip to know about its constraint, especially so that subsequent
        /// calculations may be properly normalized and denormalized when performing calculations
        /// and making reports, respectively.</remarks>
        public Trip(TripConstraint constraint, int departureTimeMinutes)
        {
            Constraint = constraint;
            DepartureTimeMinutes = departureTimeMinutes;
        }

        public bool IsNorthbound
        {
            get { return Direction == TripConstraint.Northbound; }
        }

        public bool IsSouthbound
        {
            get { return Direction == TripConstraint.Southbound; }
        }

        public void Report(TextWriter writer, Conductor conductor, Patron patron)
        {
            var earliestDepartureTimeMinutes
                = DepartureTimeMinutes
                  + conductor.GetStopIntervalsMinutes(patron).Sum();

            writer.WriteLine(earliestDepartureTimeMinutes.FormatMinutes());
        }
    }

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

    /// <summary>
    /// Conductor is responsible for managing the line.
    /// </summary>
    public class Conductor
    {
        /// <summary>
        /// Represents the time between stops in minutes.
        /// The ith entry is the time between the ith stop and the (i+1)th stop.
        /// </summary>
        public List<int> StopIntervalMinutes { get; private set; }

        /// <summary>
        /// Represents a set of <see cref="IDirectional.IsNorthbound"/> and
        /// <see cref="IDirectional.IsSouthbound"/> constraints.
        /// </summary>
        public List<TripConstraint> Constraints { get; private set; }

        /// <summary>
        /// Represents a collection of ScheduledTrips.
        /// </summary>
        private readonly IList<Trip> _scheduledTrips;

        public Conductor(IEnumerable<int> stopIntervalMinutes,
            IEnumerable<TripConstraint> constraints)
        {
            StopIntervalMinutes = stopIntervalMinutes.ToList();
            Constraints = constraints.ToList();
            _scheduledTrips = new List<Trip>();
        }

        private List<Patron> _patrons;

        private IReadOnlyCollection<Patron> _readOnlyPatrons;

        public IReadOnlyCollection<Patron> ReadOnlyPatrons
        {
            get
            {
                return _readOnlyPatrons ?? (_readOnlyPatrons
                    = new ReadOnlyCollection<Patron>(_patrons));
            }
        }

        /// <summary>
        /// Returns a collection of stop intervals relevant from the <paramref name="patron"/>
        /// <see cref="Patron.StopNumber"/> in the appropriate <see cref="IDirectional.IsNorthbound"/>.
        /// This is a useful callback throughout the approach.
        /// </summary>
        /// <param name="patron"></param>
        /// <returns></returns>
        internal IEnumerable<int> GetStopIntervalsMinutes(Patron patron)
        {
            // No need to subtract one here just pass the StopNumber through in either case.
            return patron.IsNorthbound
                ? StopIntervalMinutes.Take(patron.StopNumber)
                : StopIntervalMinutes.Skip(patron.StopNumber);
        }

        // ReSharper disable SuggestBaseTypeForParameter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trip"></param>
        /// <param name="patron"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        /// <remarks>This was another impacted area where not only calculating the desired
        /// departure time, but also knowing about the associated constraint, was desirable.</remarks>
        private int? GetNextDepartureTimeMinutes(Trip trip, Patron patron, out TripConstraint constraint)
        {
            // Should work for both Northbound (true/false), inverse of which is Southbound.
            constraint = Constraints.Single(t => t.IsNorthbound == patron.IsNorthbound);

            if (trip == null)
                return constraint.FirstDepartureTimeMinutes;

            var dtm = trip.DepartureTimeMinutes;

            // Which closing the loop the refactor should be relatively painless after all.
            var result = dtm + constraint.Frequencies.GetWaitTimeMinutes(dtm);

            return result > constraint.MaxAllowableDepartureTimeMinutes
                ? (int?)null
                : result;
        }

        private void ScheduleTrip(Patron patron)
        {
            Trip last;

            /* Backfill the schedule until the Patron's request can be fulfilled,
             * if at all possible given known set of constraints. */

            while ((last = _scheduledTrips
                .Where(t => t.IsNorthbound == patron.IsNorthbound)
                .OrderBy(t => t.DepartureTimeMinutes).LastOrDefault())
                .CannotBeScheduled(patron, GetStopIntervalsMinutes))
            {
                TripConstraint constraint;

                var nextDepartureTimeMinutes = GetNextDepartureTimeMinutes(last, patron, out constraint);

                // It is not possible to fulfill the Patron's itinerary given the allowable constraints.
                if (nextDepartureTimeMinutes == null) break;

                var trip = new Trip(constraint, nextDepartureTimeMinutes.Value);

                _scheduledTrips.Add(trip);
            }

            // Because Patron may have jumped into the queue midstream.
            patron.Itinerary = _scheduledTrips.First(
                t => t.CanBeScheduled(patron, GetStopIntervalsMinutes));
        }

        public void ScheduleTrips()
        {
            _patrons.ForEach(ScheduleTrip);
        }

        private static IEnumerable<int> ReadStopIntervalMinutes(TextReader reader)
        {
            var line = reader.ReadLineAsync().Result;
            // TODO: may verify count with array length.
            var count = line.ParseInteger();
            line = reader.ReadLineAsync().Result;
            return from x in line.Split(' ') select x.ParseInteger();
        }

        public static Conductor Read(TextReader reader)
        {
            var stopIntervalMinutes = ReadStopIntervalMinutes(reader).ToArray();

            var constraints = TripConstraint.ReadAll(reader).ToList();

            /* Instead of Frequencies existing at a Conductor level, they must instead belong
             * to a TripConstraint, since their times must reflect a baseline of the earliest
             * scheduled departure time. */

            var frequencies = Frequencies.Read(reader);

            constraints.ForEach(c => c.Frequencies = frequencies);

            var patrons = Patron.ReadAll(reader).ToArray();

            return new Conductor(stopIntervalMinutes, constraints)
            {
                _patrons = patrons.ToList()
            };
        }
    }

    public class LightRail : ChallengeBase
    {
        public LightRail(TextReader reader, TextWriter writer)
            : base(reader, writer)
        {
        }

        private Conductor _conductor;

        protected override void Read(TextReader reader)
        {
            _conductor = Conductor.Read(reader);
        }

        protected override void Run()
        {
            _conductor.ScheduleTrips();
        }

        protected override void Report(TextWriter writer)
        {
            // In a round-about kind of way this is how the report may be conducted.
            foreach (var patron in _conductor.ReadOnlyPatrons)
                patron.Itinerary.Report(writer, _conductor, patron);
        }
    }
}

namespace Benchmark
{
    class Program
    {

#if DEVELOP

        private static IEnumerable<string> TestCases
        {
            get
            {
                // Expected output:
                // 06:00
                // 05:04
                // 05:04
                // 05:19
                // 07:14
                // 07:22
                // 07:25
                // 23:19
                // 23:56
                yield return @"9
4 8 3 12 8 2 2 3
05:04 23:30
06:00 23:55
6
05:00 15
06:00 6
08:30 10
15:00 6
18:30 10
21:00 15
9
05:55 8 S
05:00 0 N
05:04 0 N
05:05 0 N
07:09 1 N
07:17 2 N
07:20 3 N
23:10 0 N
23:47 6 N";

                // Expected output:
                // 06:00
                // 05:04
                // 05:04
                // 05:19
                // 07:14
                // 07:22
                // 07:25
                // 23:19
                // 23:56
                yield return @"9
4 8 3 12 8 2 2 3
05:04 23:30
06:00 23:55
6
05:00 15
06:00 6
08:30 10
15:00 6
18:30 10
21:00 15
11
05:55 9 S
05:00 0 N
05:04 0 N
05:05 0 N
07:09 1 N
07:17 2 N
07:20 3 N
23:10 0 N
23:47 6 N";

                // TODO: 23:59 was a typo from their web site
                // Expected output:
                // 05:04
                // 05:04
                // 23:49
                yield return @"8
4 8 3 12 8 2 3
05:04 02:00
06:00 02:50
6
05:00 15
06:00 6
08:30 10
15:00 6
21:00 15
01:00 20
3
05:00 0 N
02:05 0 N
23:47 0 N";
            }
        }

#endif

        static void Main(string[] args)
        {

#if DEVELOP

            foreach (var testCase in TestCases)
            {
                using (var reader = new StringReader(testCase))
                {
                    using (new LightRail(reader, Console.Out))
                    {
                        Console.WriteLine("=========================");
                    }
                }
            }

#else

            using (new LightRail(Console.In, Console.Out))
            {
            }

#endif

        }
    }
}

#if !DEVELOP

class Test
{
    static void Main()
    {
        using (new LightRail(Console.In, Console.Out))
        {
        }
    }
}

#endif

#endif
