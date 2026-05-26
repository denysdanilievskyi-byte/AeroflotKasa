using System.Text.Json;
using AeroflotKasa.Models;

namespace AeroflotKasa.Services;

public class FlightService
{
    private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "flights.json");
    private List<Flight> _flights = new();

    public bool HasUnsavedChanges { get; private set; } = false;
    public IReadOnlyList<Flight> GetAllFlights() => _flights;

    public void AddFlight(Flight flight)
    {
        _flights.Add(flight);
        HasUnsavedChanges = true;
    }

    public void UpdateFlight(Flight oldFlight, Flight newFlight)
    {
        var index = _flights.IndexOf(oldFlight);
        if (index >= 0)
        {
            _flights[index] = newFlight;
            HasUnsavedChanges = true;
        }
    }

    public void DeleteFlight(Flight flight)
    {
        _flights.Remove(flight);
        HasUnsavedChanges = true;
    }
}
