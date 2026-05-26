using AeroflotKasa.Models;

namespace AeroflotKasa.Forms;

/// <summary>
/// Форма для додавання та редагування інформації про рейс.
/// </summary>
public class FlightForm : Form
{
    private TextBox txtFlightNumber = new();
    private TextBox txtRoute = new();
    private TextBox txtStops = new();
    private TextBox txtDays = new();
    private DateTimePicker dtpDeparture = new();
    private NumericUpDown numSeats = new();
    private Button btnSave = new();
    private Button btnCancel = new();


    public Flight? Flight { get; private set; }
}
