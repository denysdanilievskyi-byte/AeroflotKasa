namespace AeroflotKasa.Models;

public class Flight
{
    public string FlightNumber { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public string IntermediateStops { get; set; } = string.Empty;
    public DateTime DepartureTime { get; set; }
    public string FlightDays { get; set; } = string.Empty;
    public int AvailableSeats { get; set; }

    public List<Passenger> Passengers { get; set; } = new();
}