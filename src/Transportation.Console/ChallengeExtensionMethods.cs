using System.Collections.Generic;
using System.Linq;
using Challenge.Core;

namespace Transportation
{
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
                   *Constants.MinutesPerHour
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
            var h = minutes/60;
            var m = minutes%60;
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
}
