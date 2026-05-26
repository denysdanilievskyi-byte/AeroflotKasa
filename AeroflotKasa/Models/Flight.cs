namespace AeroflotKasa.Models;

/// <summary>
/// Представляє рейс у касі аерофлоту.
/// Містить інформацію про маршрут, час та доступні місця.
/// </summary>
public class Flight
{
    /// <summary>
    /// Унікальний номер рейсу.
    /// </summary>
    public string FlightNumber { get; set; } = string.Empty;

    /// <summary>
    /// Маршрут рейсу у форматі 'Пункт А - Пункт Б'.
    /// </summary>
    public string Route { get; set; } = string.Empty;

    /// <summary>
    /// Пункти проміжної посадки, якщо такі є.
    /// </summary>
    public string IntermediateStops { get; set; } = string.Empty;

    /// <summary>
    /// Дата та час відправлення рейсу.
    /// </summary>
    public DateTime DepartureTime { get; set; }

    /// <summary>
    /// Дні польоту рейсу протягом тижня.
    /// </summary>
    public string FlightDays { get; set; } = string.Empty;

    /// <summary>
    /// Кількість вільних місць на рейсі, доступних для бронювання.
    /// </summary>
    public int AvailableSeats { get; set; }

    /// <summary>
    /// Список пасажирів, які придбали квитки на цей рейс. 
    /// Реалізує зв'язок 1-до-багатьох для виконання вимог курсової роботи щодо двох сутностей.
    /// </summary>
    public List<Passenger> Passengers { get; set; } = new();
}