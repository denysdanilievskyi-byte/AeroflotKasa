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

    public BookingForm(Flight flight)
    {
        InitializeComponent(flight);
    }

    private void InitializeComponent(Flight flight)
    {
        this.Text = "Оформлення квитків";
        this.Size = new Size(350, 300);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.KeyPreview = true;
        this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) this.DialogResult = DialogResult.Cancel; };

        var lblInfo = new Label
        {
            Text = $"Рейс: {flight.FlightNumber}\nМаршрут: {flight.Route}\nВільних місць: {flight.AvailableSeats}",
            Location = new Point(10, 10),
            AutoSize = true
        };
        this.Controls.Add(lblInfo);
    }
}