using System;
using System.Collections.Generic;
using System.Linq;
using Challenge.Core;

namespace ThemeParkPlanner
{
    public class Visit
    {
        private readonly Guest _guest;

        private readonly IReadOnlyThemePark _themePark;

        private readonly List<int> _desired;

        public IEnumerable<int> Desired
        {
            get { return _desired; }
        }

        public Visit(Guest guest, IReadOnlyThemePark themePark, IEnumerable<int> desired)
        {
            _guest = guest;
            _themePark = themePark;
            _desired = desired.ToList();
        }

        public int TimeInPark
        {
            get
            {
                var timeMinutes = TimeMinutes;

                return timeMinutes > _themePark.MaxMinutesPerDay
                    ? Impossible
                    : (timeMinutes - _guest.EntryTimeMinutes);
            }
        }

        private int? _timeMinutes;

        private int TimeMinutes
        {
            get { return (_timeMinutes ?? (_timeMinutes = CalculateTime())).Value; }
        }

        private int CalculateTime()
        {
            const int minutesPerHour = Constants.MinutesPerHour;
            const int waitTime = Constants.WaitTime;

            var trimmed = Desired.TrimRight(waitTime).ToArray();

            var currentTime = _guest.EntryTimeMinutes;

            for (var i = 0; i < trimmed.Length; i++)
            {
                var d = trimmed[i];

                if (d == waitTime)
                {
                    var ceilingTime = (i + 1)*minutesPerHour;

                    currentTime = Math.Max(ceilingTime, currentTime);

                    continue;
                }

                // When the decision is not to wait, then attend the attractions as frequently as time permits.
                var a = _themePark.ReadOnlyAttractions.ElementAt(d);

                //var baseTime = entryTime > 0 ? (i*minutesPerHour + entryTime) : ((i + 1)*minutesPerHour);

                int queueTime;

                if (a.TryGetQueueTime(currentTime, out queueTime))
                    currentTime += queueTime;
            }

            return currentTime;
        }

        private const int Impossible = int.MaxValue;

        public bool IsImpossible()
        {
            return TimeInPark == Impossible;
        }
    }
}
