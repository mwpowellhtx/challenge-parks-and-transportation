namespace LeftOrRight
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

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
}
