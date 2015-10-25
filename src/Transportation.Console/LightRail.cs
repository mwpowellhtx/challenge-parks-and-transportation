using System.IO;
using Challenge.Core;

namespace Transportation
{
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
        public const int MinutesPerDay = 24*MinutesPerHour;
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
