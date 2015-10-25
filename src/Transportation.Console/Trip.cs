using System.IO;
using System.Linq;

namespace Transportation
{
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
            //TODO: also putting a cap on the minutes: 23:59? see example ...
            var earliestDepartureTimeMinutes
                = DepartureTimeMinutes
                  + conductor.GetStopIntervalsMinutes(patron).Sum();

            writer.WriteLine(earliestDepartureTimeMinutes.FormatMinutes());
        }
    }
}
