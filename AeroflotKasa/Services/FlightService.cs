using System.Text.Json;
using AeroflotKasa.Models;

namespace AeroflotKasa.Services;

public class FlightService
{
    private List<Flight> _flights = new();

    public IReadOnlyList<Flight> GetAllFlights() => _flights;

    public void AddFlight(Flight flight)
    {
        _flights.Add(flight);
    }

    public void UpdateFlight(Flight oldFlight, Flight newFlight)
    {
        var index = _flights.IndexOf(oldFlight);
        if (index >= 0)
        {
            _flights[index] = newFlight;
        }
    }
}
