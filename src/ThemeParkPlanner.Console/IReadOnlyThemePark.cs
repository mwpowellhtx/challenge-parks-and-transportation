using System.Collections.Generic;

namespace ThemeParkPlanner
{
    public interface IReadOnlyThemePark
    {
        int MaxHoursPerDay { get; }

        int MaxMinutesPerDay { get; }

        IReadOnlyCollection<Attraction> ReadOnlyAttractions { get; }
    }
}
