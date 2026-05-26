using AeroflotKasa.Models;

namespace AeroflotKasa.Forms;

/// <summary>
/// Форма для додавання та редагування інформації про рейс.
/// </summary>
public class FlightForm : Form
{
    public Flight? Flight { get; private set; }
}