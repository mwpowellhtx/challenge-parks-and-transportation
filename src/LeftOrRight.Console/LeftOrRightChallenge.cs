namespace LeftOrRight
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Challenge.Core;

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
