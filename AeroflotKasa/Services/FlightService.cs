using System.Collections.Generic;
using AeroflotKasa.Models;

namespace AeroflotKasa.Services;

public class FlightService
{
    private List<Flight> _flights = new();

    public bool HasUnsavedChanges { get; private set; } = false;

    public IReadOnlyList<Flight> GetAllFlights() => _flights;
}