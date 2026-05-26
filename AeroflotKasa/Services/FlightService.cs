using System.Text.Json;
using AeroflotKasa.Models;

namespace AeroflotKasa.Services;

public class FlightService
{
    private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "flights.json");
    private List<Flight> _flights = new();

    public bool HasUnsavedChanges { get; private set; } = false;
    public FlightService()
    {
        LoadData();
    }
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
    public List<Flight> FindNearestFlights(string destination)
    {
        return _flights
            .Where(f => f.AvailableSeats > 0 &&
                        (f.Route.Contains(destination, StringComparison.OrdinalIgnoreCase) ||
                         f.IntermediateStops.Contains(destination, StringComparison.OrdinalIgnoreCase)))
            .OrderBy(f => f.DepartureTime)
            .ToList();
    }
    public bool BookTickets(Flight flight, Passenger passenger, int ticketCount)
    {
        if (flight.AvailableSeats >= ticketCount)
        {
            flight.AvailableSeats -= ticketCount;

            for (int i = 0; i < ticketCount; i++)
            {
                flight.Passengers.Add(passenger);
            }

            HasUnsavedChanges = true;
            return true;
        }
        return false;
    }
    public void SaveData()
    {
        try
        {
            var json = JsonSerializer.Serialize(_flights, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            throw new Exception($"Не вдалося зберегти дані у файл {_filePath}. Деталі: {ex.Message}");
        }
    }
    private void LoadData()
    {
        if (File.Exists(_filePath))
        {
            try
            {
                var json = File.ReadAllText(_filePath);
                var data = JsonSerializer.Deserialize<List<Flight>>(json);
                if (data != null)
                {
                    _flights = data;
                }
            }
            catch
            {
                _flights = new List<Flight>();
            }
        }
    }

}
