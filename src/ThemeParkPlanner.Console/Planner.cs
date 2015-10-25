using System;
using System.IO;
using Challenge.Core;

namespace ThemeParkPlanner
{
    public class Planner : ChallengeBase
    {
        private ThemePark _themePark;

        public Planner(TextReader reader, TextWriter writer)
            : base(reader, writer)
        {
        }

        protected override void Read(TextReader reader)
        {
            _themePark = ThemePark.Read(reader);
        }

        protected override void Run()
        {
            Action<Guest> tryVisit = g => g.TryVisit();
            _themePark.Guests.ForEach(tryVisit);
        }

        protected override void Report(TextWriter writer)
        {
            Action<Guest> report = g => g.Report(writer);
            _themePark.Guests.ForEach(report);
        }
    }
}
