using AeroflotKasa.Models;

namespace AeroflotKasa.Forms;

/// <summary>
/// Форма для оформлення квитків на рейс та узгодження з пасажиром.
/// </summary>
public class BookingForm : Form
{
    private TextBox txtFullName = new();
    private TextBox txtPassportData = new();
    private NumericUpDown numTickets = new();
    private Button btnConfirm = new();
    private Button btnCancel = new();

    public int TicketCount => (int)numTickets.Value;
    public Passenger PassengerInfo { get; private set; } = new Passenger();
}