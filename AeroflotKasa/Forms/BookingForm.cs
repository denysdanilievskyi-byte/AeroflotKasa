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

        var lblFullName = new Label { Text = "ПІБ Пасажира:", Location = new Point(10, 75), AutoSize = true };
        txtFullName.Location = new Point(130, 70);
        txtFullName.Size = new Size(180, 25);
        txtFullName.TabIndex = 0;

        var lblPassport = new Label { Text = "Дані паспорта:", Location = new Point(10, 105), AutoSize = true };
        txtPassportData.Location = new Point(130, 100);
        txtPassportData.Size = new Size(180, 25);
        txtPassportData.TabIndex = 1;

        var lblTickets = new Label { Text = "Кількість квитків:", Location = new Point(10, 135), AutoSize = true };
        numTickets.Location = new Point(130, 130);
        numTickets.Size = new Size(180, 25);
        numTickets.Minimum = 1;
        numTickets.Maximum = flight.AvailableSeats > 0 ? flight.AvailableSeats : 1;
        numTickets.TabIndex = 2;

        this.Controls.Add(lblFullName);
        this.Controls.Add(txtFullName);
        this.Controls.Add(lblPassport);
        this.Controls.Add(txtPassportData);
        this.Controls.Add(lblTickets);
        this.Controls.Add(numTickets);
    }
}