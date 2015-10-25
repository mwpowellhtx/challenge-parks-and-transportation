using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Challenge.Core;

namespace Transportation
{
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
            return patron.IsNorthbound
                ? StopIntervalMinutes.Take(patron.StopNumber)
                : StopIntervalMinutes.Skip(patron.StopNumber - 1);
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
                ? (int?) null
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
}
