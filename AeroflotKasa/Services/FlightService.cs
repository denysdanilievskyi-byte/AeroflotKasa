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

        if (!_flights.Any())
        {
            GenerateMockData();
        }
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
    private void GenerateMockData()
    {
        var random = new Random();

        var airlines = new[] { "PS", "LO", "LH", "AF", "BA", "TK", "UA", "AA", "DL", "EK", "QR", "SQ" };
        var cities = new[]
        {
            "Київ", "Варшава", "Лондон", "Париж", "Берлін", "Рим", "Мадрид", "Амстердам",
            "Нью-Йорк", "Лос-Анджелес", "Чикаго", "Маямі", "Торонто",
            "Токіо", "Пекін", "Сеул", "Бангкок", "Сінгапур", "Дубай", "Доха"
        };
        var daysOptions = new[] { "Пн, Ср, Пт", "Вт, Чт, Сб", "Щодня", "Сб, Нд", "Пн, Чт", "Ср, Нд" };

        var generatedFlights = new List<Flight>();

        for (int i = 0; i < 300; i++)
        {
            var airline = airlines[random.Next(airlines.Length)];
            var flightNumber = $"{airline}-{random.Next(100, 9999)}-{i}";

            var from = cities[random.Next(cities.Length)];
            string to;
            do { to = cities[random.Next(cities.Length)]; } while (from == to);

            var route = $"{from} - {to}";

            var intermediateStops = string.Empty;
            if (random.NextDouble() < 0.4)
            {
                string stop;
                do { stop = cities[random.Next(cities.Length)]; } while (stop == from || stop == to);
                intermediateStops = stop;
            }

            var daysOffset = random.Next(1, 60);
            var hours = random.Next(0, 24);
            var minutes = random.Next(0, 12) * 5;
            var departureTime = DateTime.Today.AddDays(daysOffset).AddHours(hours).AddMinutes(minutes);

            var days = daysOptions[random.Next(daysOptions.Length)];
            var seats = random.Next(0, 350);

            generatedFlights.Add(new Flight
            {
                FlightNumber = flightNumber,
                Route = route,
                IntermediateStops = intermediateStops,
                DepartureTime = departureTime,
                FlightDays = days,
                AvailableSeats = seats
            });
        }

        _flights = generatedFlights.OrderBy(f => f.DepartureTime).ToList();
        SaveData();
        HasUnsavedChanges = false;
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
