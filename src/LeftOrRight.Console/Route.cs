namespace LeftOrRight
{
    using System;

    public class Route
    {
        private readonly Attractions _attractions;

        private int _current;

        public int Current
        {
            get { return _current; }
            private set { _current = value.VerifyDestination(_attractions); }
        }

        private int _destination;

        public int Destination
        {
            get { return _destination; }
            private set { _destination = value.VerifyDestination(_attractions); }
        }

        private int? _leastMinutes;

        /// <summary>
        /// Gets the LeastMinutes it should take to reach <see cref="Destination"/> starting
        /// from <see cref="Current"/>.
        /// </summary>
        public int LeastMinutes
        {
            get { return (_leastMinutes ?? (_leastMinutes = CalculateLeastMinutes())).Value; }
        }

        private int CalculateLeastMinutes()
        {
            return Math.Min(_attractions.GetLeftRouteItem(this), _attractions.GetRightRouteItem(this));
        }

        public Route(Attractions attractions, int current, int destination)
        {
            _attractions = attractions;
            Current = current;
            Destination = destination;
        }

        public bool IsDestinationLeft
        {
            get { return Destination < Current; }
        }

        public bool IsDestinationRight
        {
            get { return Destination > Current; }
        }
    }
}
