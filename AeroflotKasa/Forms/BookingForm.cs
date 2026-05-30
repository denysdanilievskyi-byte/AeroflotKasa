using AeroflotKasa.Models;

namespace AeroflotKasa.Forms;

/// <summary>
/// Форма для оформлення квитків на рейс та узгодження з пасажиром.
/// </summary>
public class BookingForm : Form
{
    private TextBox txtFullName = new();
    private TextBox txtPassportData = new();
    private TextBox txtTickets = new();
    private Button btnConfirm = new();
    private Button btnCancel = new();

    private readonly Flight _currentFlight;
    public int TicketCount { get; private set; }

    /// <summary>
    /// Дані пасажира, на якого оформлюються квитки.
    /// </summary>
    public Passenger PassengerInfo { get; private set; } = new Passenger();

    /// <summary>
    /// Ініціалізує новий екземпляр форми бронювання для обраного рейсу.
    /// </summary>
    /// <param name="flight">Об'єкт рейсу, на який оформлюються квитки.</param>
    public BookingForm(Flight flight)
    {
        _currentFlight = flight;
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

        var lblInfo = new Label
        {
            Text = $"Рейс: {flight.FlightNumber}\nМаршрут: {flight.Route}\nВільних місць: {flight.AvailableSeats}",
            Location = new Point(10, 10),
            AutoSize = true
        };

        var lblFullName = new Label { Text = "ПІБ Пасажира:", Location = new Point(10, 75), AutoSize = true };
        txtFullName.Location = new Point(130, 70);
        txtFullName.Size = new Size(180, 25);
        txtFullName.TabIndex = 0;

        var lblPassport = new Label { Text = "Дані паспорта:", Location = new Point(10, 105), AutoSize = true };
        txtPassportData.Location = new Point(130, 100);
        txtPassportData.Size = new Size(180, 25);
        txtPassportData.TabIndex = 1;

        var lblTickets = new Label { Text = "Кількість квитків:", Location = new Point(10, 135), AutoSize = true };
        txtTickets.Location = new Point(130, 130);
        txtTickets.Size = new Size(180, 25);
        txtTickets.Text = "1";
        txtTickets.TabIndex = 2;

        btnConfirm.Text = "Узгодити та Підтвердити";
        btnConfirm.Location = new Point(80, 190);
        btnConfirm.Size = new Size(180, 25);
        btnConfirm.TabIndex = 3;
        btnConfirm.Click += BtnConfirm_Click;

        btnCancel.Text = "Скасувати";
        btnCancel.Location = new Point(120, 220);
        btnCancel.Size = new Size(100, 25);
        btnCancel.TabIndex = 4;
        btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

        this.Controls.Add(lblInfo);
        this.Controls.Add(lblFullName);
        this.Controls.Add(txtFullName);
        this.Controls.Add(lblPassport);
        this.Controls.Add(txtPassportData);
        this.Controls.Add(lblTickets);
        this.Controls.Add(txtTickets);
        this.Controls.Add(btnConfirm);
        this.Controls.Add(btnCancel);

        this.AcceptButton = btnConfirm;
        this.CancelButton = btnCancel;
        this.KeyDown += BookingForm_KeyDown;
    }

    private void BookingForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F1)
        {
            MessageBox.Show("Заповніть ПІБ, паспортні дані, оберіть кількість квитків та натисніть Enter.", "Довідка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void BtnConfirm_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(txtPassportData.Text))
        {
            MessageBox.Show("Будь ласка, заповніть ПІБ та паспортні дані пасажира.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!int.TryParse(txtTickets.Text, out int parsedTickets))
        {
            MessageBox.Show("Будь ласка, введіть коректне число для кількості квитків.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (parsedTickets <= 0)
        {
            MessageBox.Show("Кількість квитків повинна бути більше нуля.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (parsedTickets > _currentFlight.AvailableSeats)
        {
            MessageBox.Show($"Неможливо оформити {parsedTickets} квитків. На рейсі залишилося лише {_currentFlight.AvailableSeats} вільних місць.", "Перевищення ліміту", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        TicketCount = parsedTickets;
        PassengerInfo.FullName = txtFullName.Text.Trim();
        PassengerInfo.PassportData = txtPassportData.Text.Trim();

        var confirmResult = MessageBox.Show(
            $"Пасажир: {PassengerInfo.FullName}\n" +
            $"Документ: {PassengerInfo.PassportData}\n" +
            $"Кількість квитків: {TicketCount}\n\n" +
            $"Ви погоджуєте оформлення та списування місць?",
            "Узгодження з пасажиром",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmResult == DialogResult.Yes)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}