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
    private DateTimePicker dtpDeparture = new();
    private TextBox txtDays = new();
    private NumericUpDown numSeats = new();
    private Button btnSave = new();
    private Button btnCancel = new();

    private Flight? _originalFlight;

    /// <summary>
    /// Об'єкт рейсу, який було створено або відредаговано у формі.
    /// </summary>
    public Flight? Flight { get; private set; }

    /// <summary>
    /// Ініціалізує новий екземпляр форми для додавання або редагування рейсу.
    /// </summary>
    /// <param name="flight">Об'єкт рейсу для редагування. Якщо null, створюється новий рейс.</param>
    public FlightForm(Flight? flight = null)
    {
        InitializeComponent();
        _originalFlight = flight;

        if (flight != null)
        {
            txtFlightNumber.Text = flight.FlightNumber;
            txtRoute.Text = flight.Route;
            txtStops.Text = flight.IntermediateStops;
            dtpDeparture.Value = flight.DepartureTime < dtpDeparture.MinDate ? DateTime.Now : flight.DepartureTime;
            txtDays.Text = flight.FlightDays;
            numSeats.Value = flight.AvailableSeats;
        }
    }

    private void InitializeComponent()
    {
        this.Text = "Інформація про рейс";
        this.Size = new Size(350, 350);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.KeyPreview = true;

        var lblFlightNumber = new Label { Text = "Номер рейсу:", Location = new Point(10, 15), AutoSize = true };
        txtFlightNumber.Location = new Point(120, 10);
        txtFlightNumber.Size = new Size(200, 25);
        txtFlightNumber.TabIndex = 0;

        var lblRoute = new Label { Text = "Маршрут:", Location = new Point(10, 45), AutoSize = true };
        txtRoute.Location = new Point(120, 40);
        txtRoute.Size = new Size(200, 25);
        txtRoute.TabIndex = 1;

        var lblStops = new Label { Text = "Проміжні посадки:", Location = new Point(10, 75), AutoSize = true };
        txtStops.Location = new Point(120, 70);
        txtStops.Size = new Size(200, 25);
        txtStops.TabIndex = 2;

        var lblDeparture = new Label { Text = "Час відправлення:", Location = new Point(10, 105), AutoSize = true };
        dtpDeparture.Location = new Point(120, 100);
        dtpDeparture.Size = new Size(200, 25);
        dtpDeparture.Format = DateTimePickerFormat.Custom;
        dtpDeparture.CustomFormat = "dd.MM.yyyy HH:mm";
        dtpDeparture.TabIndex = 3;

        var lblDays = new Label { Text = "Дні польоту:", Location = new Point(10, 135), AutoSize = true };
        txtDays.Location = new Point(120, 130);
        txtDays.Size = new Size(200, 25);
        txtDays.TabIndex = 4;

        var lblSeats = new Label { Text = "Вільні місця:", Location = new Point(10, 165), AutoSize = true };
        numSeats.Location = new Point(120, 160);
        numSeats.Size = new Size(200, 25);
        numSeats.Maximum = 1000;
        numSeats.TabIndex = 5;

        btnSave.Text = "Зберегти";
        btnSave.Location = new Point(120, 220);
        btnSave.TabIndex = 6;
        btnSave.Click += BtnSave_Click;

        btnCancel.Text = "Скасувати";
        btnCancel.Location = new Point(220, 220);
        btnCancel.TabIndex = 7;
        btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

        this.Controls.Add(lblFlightNumber);
        this.Controls.Add(txtFlightNumber);
        this.Controls.Add(lblRoute);
        this.Controls.Add(txtRoute);
        this.Controls.Add(lblStops);
        this.Controls.Add(txtStops);
        this.Controls.Add(lblDeparture);
        this.Controls.Add(dtpDeparture);
        this.Controls.Add(lblDays);
        this.Controls.Add(txtDays);
        this.Controls.Add(lblSeats);
        this.Controls.Add(numSeats);
        this.Controls.Add(btnSave);
        this.Controls.Add(btnCancel);

        this.AcceptButton = btnSave;
        this.CancelButton = btnCancel;
        this.KeyDown += FlightForm_KeyDown;
    }

    private void FlightForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F1)
        {
            MessageBox.Show("Введіть коректні дані для рейсу та натисніть 'Зберегти' або клавішу Enter.", "Довідка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtFlightNumber.Text) || string.IsNullOrWhiteSpace(txtRoute.Text))
        {
            MessageBox.Show("Будь ласка, заповніть номер рейсу та маршрут.", "Помилка перевірки", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Flight = new Flight
        {
            FlightNumber = txtFlightNumber.Text.Trim(),
            Route = txtRoute.Text.Trim(),
            IntermediateStops = txtStops.Text.Trim(),
            DepartureTime = dtpDeparture.Value,
            FlightDays = txtDays.Text.Trim(),
            AvailableSeats = (int)numSeats.Value,
            Passengers = _originalFlight != null ? _originalFlight.Passengers : new List<Passenger>()
        };

        this.DialogResult = DialogResult.OK;
    }
}