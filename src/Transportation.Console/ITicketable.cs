namespace Transportation
{
    public interface ITicketable
    {
        int Direction { get; }

        int DepartureTimeMinutes { get; }
    }
}
