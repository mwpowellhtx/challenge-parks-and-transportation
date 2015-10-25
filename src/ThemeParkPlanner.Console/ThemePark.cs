using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Challenge.Core;

namespace ThemeParkPlanner
{
    public class ThemePark : IReadOnlyThemePark
    {
        public int MaxHoursPerDay { get; private set; }

        public int MaxMinutesPerDay
        {
            get { return MaxHoursPerDay*Constants.MinutesPerHour; }
        }

        public List<Attraction> Attractions { get; private set; }

        private IReadOnlyCollection<Attraction> _readOnlyAttractions;

        public IReadOnlyCollection<Attraction> ReadOnlyAttractions
        {
            get
            {
                return _readOnlyAttractions ?? (_readOnlyAttractions
                    = new ReadOnlyCollection<Attraction>(Attractions));
            }
        }

        public ThemePark(IEnumerable<Attraction> attractions, int maxHoursPerDay)
        {
            Attractions = attractions.ToList();
            MaxHoursPerDay = maxHoursPerDay;
        }

        public List<Guest> Guests { get; private set; }

        public static ThemePark Read(TextReader reader)
        {
            var values = from x in reader.ReadLineAsync().Result.Split(' ')
                select x.ParseInteger();

            // ReSharper disable once PossibleMultipleEnumeration
            var attractionCount = values.ElementAt(0);

            // ReSharper disable once PossibleMultipleEnumeration
            var maxHoursPerDay = values.ElementAt(1);

            var attractions = Attraction.ReadAll(reader, attractionCount).ToList();

            var result = new ThemePark(attractions, maxHoursPerDay);

            var queryCount = reader.ReadLineAsync().Result.ParseInteger();

            result.Guests = Guest.ReadAll(reader, queryCount, result).ToList();

            return result;
        }
    }
}
