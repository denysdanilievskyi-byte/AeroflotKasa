using System.Drawing.Printing;
using System.Text;
using AeroflotKasa.Forms;
using AeroflotKasa.Models;
using AeroflotKasa.Services;

namespace AeroflotKasa;

/// <summary>
/// Головна форма додатку "Каса аерофлоту".
/// Відображає розклад рейсів та надає доступ до основних операцій.
/// </summary>
public partial class Form1 : Form
{
    private readonly FlightService _flightService;
    private DataGridView dgvFlights = new();
    private TextBox txtSearch = new();
    private Button btnSearch = new();
    private Button btnAdd = new();
    private Button btnEdit = new();
    private Button btnDelete = new();
    private Button btnBook = new();
    private Button btnHelp = new();

    /// <summary>
    /// Ініціалізує головну форму додатку.
    /// </summary>
    public Form1()
    {
        _flightService = new FlightService();
        InitializeComponentCustom();
        RefreshGrid(_flightService.GetAllFlights());
    }

    private void InitializeComponentCustom()
    {
        this.Text = "Каса аерофлоту";
        this.Size = new Size(900, 500);
        this.MinimumSize = new Size(800, 400);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.KeyPreview = true;
        this.KeyDown += Form1_KeyDown;
        this.FormClosing += Form1_FormClosing;

        var pnlTop = new Panel { Dock = DockStyle.Top, Height = 50 };

        var lblSearch = new Label { Text = "Пункт призначення:", Location = new Point(10, 15), AutoSize = true };
        txtSearch.Location = new Point(140, 10);
        txtSearch.Size = new Size(200, 25);
        txtSearch.TabIndex = 0;

        btnSearch.Text = "Знайти рейси";
        btnSearch.Location = new Point(350, 10);
        btnSearch.Size = new Size(150, 25);
        btnSearch.TabIndex = 1;
        btnSearch.Click += BtnSearch_Click;

        var btnClear = new Button { Text = "Скинути пошук", Location = new Point(510, 10), Size = new Size(120, 25) };
        btnClear.TabIndex = 2;
        btnClear.Click += (s, e) => { txtSearch.Clear(); RefreshGrid(_flightService.GetAllFlights()); };

        pnlTop.Controls.Add(lblSearch);
        pnlTop.Controls.Add(txtSearch);
        pnlTop.Controls.Add(btnSearch);
        pnlTop.Controls.Add(btnClear);

        var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 60 };

        btnAdd.Text = "Додати";
        btnAdd.Location = new Point(10, 10);
        btnAdd.TabIndex = 3;
        btnAdd.Click += BtnAdd_Click;

        btnEdit.Text = "Редагувати";
        btnEdit.Location = new Point(90, 10);
        btnEdit.TabIndex = 4;
        btnEdit.Click += BtnEdit_Click;

        btnDelete.Text = "Видалити";
        btnDelete.Location = new Point(170, 10);
        btnDelete.TabIndex = 5;
        btnDelete.Click += BtnDelete_Click;

        btnBook.Text = "Оформити квиток";
        btnBook.Location = new Point(250, 10);
        btnBook.Size = new Size(120, 25);
        btnBook.TabIndex = 6;
        btnBook.Click += BtnBook_Click;

        btnHelp.Text = "Довідка (F1)";
        btnHelp.Location = new Point(750, 10);
        btnHelp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnHelp.TabIndex = 7;
        btnHelp.Click += (s, e) => ShowHelp();

        pnlBottom.Controls.Add(btnAdd);
        pnlBottom.Controls.Add(btnEdit);
        pnlBottom.Controls.Add(btnDelete);
        pnlBottom.Controls.Add(btnBook);
        pnlBottom.Controls.Add(btnHelp);

        dgvFlights.Dock = DockStyle.Fill;
        dgvFlights.AllowUserToAddRows = false;
        dgvFlights.AllowUserToDeleteRows = false;
        dgvFlights.ReadOnly = true;
        dgvFlights.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvFlights.MultiSelect = false;
        dgvFlights.AutoGenerateColumns = false;
        dgvFlights.RowHeadersVisible = true;
        dgvFlights.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
        dgvFlights.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvFlights.TabIndex = 8;

        dgvFlights.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        dgvFlights.RowTemplate.MinimumHeight = 35;
        dgvFlights.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvFlights.ColumnHeadersHeight = 40;

        dgvFlights.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FlightNumber", HeaderText = "Номер рейсу", FillWeight = 15, MinimumWidth = 80 });
        dgvFlights.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Route", HeaderText = "Маршрут", FillWeight = 25, MinimumWidth = 150 });
        dgvFlights.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "IntermediateStops", HeaderText = "Проміжні посадки", FillWeight = 20, MinimumWidth = 120 });
        dgvFlights.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DepartureTime", HeaderText = "Час відправлення", FillWeight = 15, MinimumWidth = 110, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd.MM.yyyy HH:mm" } });
        dgvFlights.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FlightDays", HeaderText = "Дні польоту", FillWeight = 15, MinimumWidth = 100 });
        dgvFlights.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "AvailableSeats", HeaderText = "Вільні місця", FillWeight = 10, MinimumWidth = 80 });

        dgvFlights.DataBindingComplete += DgvFlights_DataBindingComplete;

        this.Controls.Add(dgvFlights);
        this.Controls.Add(pnlTop);
        this.Controls.Add(pnlBottom);

        this.AcceptButton = btnSearch;
    }

    private void DgvFlights_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        foreach (DataGridViewRow row in dgvFlights.Rows)
        {
            row.HeaderCell.Value = (row.Index + 1).ToString();
        }

        dgvFlights.RowHeadersWidth = 65;
    }

    private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (_flightService.HasUnsavedChanges)
        {
            var result = MessageBox.Show(
                "Зберегти зміни перед виходом?",
                "Підтвердження виходу",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _flightService.SaveData();
                }
                catch (Exception)
                {
                    MessageBox.Show("Під час збереження виникла помилка. Перевірте доступ до файлової системи.", "Помилка збереження", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }
            }
            else if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }

    private void Form1_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F1)
        {
            ShowHelp();
        }
        else if (e.KeyCode == Keys.Escape)
        {
            this.Close();
        }
    }

    private void ShowHelp()
    {
        MessageBox.Show(
            "Програма 'Каса аерофлоту'.\n\n" +
            "Гарячі клавіші:\n" +
            "F1 - Довідка\n" +
            "Enter - Підтвердити дію / Пошук\n" +
            "Esc - Скасувати/Закрити вікно\n" +
            "Tab - Наступне поле\n" +
            "Shift+Tab - Попереднє поле\n\n" +
            "Використовуйте кнопки знизу для керування рейсами.",
            "Довідка", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void RefreshGrid(IEnumerable<Flight> flights)
    {
        dgvFlights.DataSource = null;
        dgvFlights.DataSource = flights.ToList();
    }

    private void BtnSearch_Click(object? sender, EventArgs e)
    {
        var destination = txtSearch.Text.Trim();
        if (string.IsNullOrWhiteSpace(destination))
        {
            MessageBox.Show("Будь ласка, введіть пункт призначення для пошуку.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var nearestFlights = _flightService.FindNearestFlights(destination);
        if (nearestFlights.Any())
        {
            RefreshGrid(nearestFlights);
            MessageBox.Show($"Знайдено {nearestFlights.Count} рейсів.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show("На жаль, не знайдено рейсів із вільними місцями до вказаного пункту.", "Результат пошуку", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshGrid(new List<Flight>());
        }
    }

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        using var form = new FlightForm();
        if (form.ShowDialog() == DialogResult.OK && form.Flight != null)
        {
            try
            {
                _flightService.AddFlight(form.Flight);
                RefreshGrid(_flightService.GetAllFlights());
            }
            catch (Exception)
            {
                MessageBox.Show("Під час виконання операції виникла помилка. Перевірте доступ до файлової системи.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void BtnEdit_Click(object? sender, EventArgs e)
    {
        if (dgvFlights.CurrentRow?.DataBoundItem is Flight selectedFlight)
        {
            using var form = new FlightForm(selectedFlight);
            if (form.ShowDialog() == DialogResult.OK && form.Flight != null)
            {
                try
                {
                    _flightService.UpdateFlight(selectedFlight, form.Flight);
                    RefreshGrid(_flightService.GetAllFlights());
                }
                catch (Exception)
                {
                    MessageBox.Show("Під час виконання операції виникла помилка. Перевірте доступ до файлової системи.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        else
        {
            MessageBox.Show("Будь ласка, оберіть рейс для редагування.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (dgvFlights.CurrentRow?.DataBoundItem is Flight selectedFlight)
        {
            var result = MessageBox.Show($"Ви дійсно бажаєте видалити рейс {selectedFlight.FlightNumber}?", "Підтвердження видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    _flightService.DeleteFlight(selectedFlight);
                    RefreshGrid(_flightService.GetAllFlights());
                }
                catch (Exception)
                {
                    MessageBox.Show("Під час виконання операції виникла помилка. Перевірте доступ до файлової системи.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        else
        {
            MessageBox.Show("Будь ласка, оберіть рейс для видалення.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnBook_Click(object? sender, EventArgs e)
    {
        if (dgvFlights.CurrentRow?.DataBoundItem is Flight selectedFlight)
        {
            if (selectedFlight.AvailableSeats <= 0)
            {
                MessageBox.Show("На обраному рейсі немає вільних місць.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var form = new BookingForm(selectedFlight);
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (_flightService.BookTickets(selectedFlight, form.PassengerInfo, form.TicketCount))
                    {
                        GenerateBoardingPassPdf(selectedFlight, form.PassengerInfo, form.TicketCount);
                        RefreshGrid(_flightService.GetAllFlights());
                    }
                    else
                    {
                        MessageBox.Show("Не вдалося оформити квитки. Перевірте кількість вільних місць.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Під час виконання операції виникла помилка. Перевірте доступ до файлової системи.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        else
        {
            MessageBox.Show("Будь ласка, оберіть рейс для оформлення квитка.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void GenerateBoardingPassPdf(Flight flight, Passenger passenger, int ticketCount)
    {
        var sb = new StringBuilder();
        sb.AppendLine("=================================================");
        sb.AppendLine("               ПОСАДКОВА ВІДОМІСТЬ               ");
        sb.AppendLine("=================================================");
        sb.AppendLine($"Дата та час оформлення: {DateTime.Now:dd.MM.yyyy HH:mm}");
        sb.AppendLine("-------------------------------------------------");
        sb.AppendLine($"Пасажир:           {passenger.FullName}");
        sb.AppendLine($"Документ (паспорт): {passenger.PassportData}");
        sb.AppendLine("-------------------------------------------------");
        sb.AppendLine($"Рейс:              {flight.FlightNumber}");
        sb.AppendLine($"Маршрут:           {flight.Route}");
        if (!string.IsNullOrWhiteSpace(flight.IntermediateStops))
        {
            sb.AppendLine($"Проміжні посадки:  {flight.IntermediateStops}");
        }
        sb.AppendLine($"Час відправлення:  {flight.DepartureTime:dd.MM.yyyy HH:mm}");
        sb.AppendLine("-------------------------------------------------");
        sb.AppendLine($"Кількість квитків: {ticketCount}");
        sb.AppendLine("=================================================");
        sb.AppendLine("      Бажаємо приємного польоту! Аерофлот        ");
        sb.AppendLine("=================================================");

        string safeName = passenger.FullName.Replace(" ", "_");

        using var saveFileDialog = new SaveFileDialog
        {
            Filter = "PDF файл (*.pdf)|*.pdf|Текстовий файл (*.txt)|*.txt",
            DefaultExt = "pdf",
            FileName = $"BoardingPass_{flight.FlightNumber}_{safeName}",
            Title = "Зберегти посадкову відомість"
        };

        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            string filePath = saveFileDialog.FileName;

            try
            {
                if (filePath.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    PrintDocument printDocument = new PrintDocument();
                    printDocument.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                    printDocument.PrinterSettings.PrintToFile = true;
                    printDocument.PrinterSettings.PrintFileName = filePath;

                    printDocument.PrintPage += (sender, e) =>
                    {
                        using var font = new Font("Courier New", 12);
                        using var brush = new SolidBrush(Color.Black);
                        e.Graphics?.DrawString(sb.ToString(), font, brush, new PointF(50, 50));
                    };

                    printDocument.Print();
                }
                else
                {
                    File.WriteAllText(filePath, sb.ToString());
                }

                MessageBox.Show(
                    $"Квитки успішно оформлено!\nПосадкову відомість збережено у файл:\n{filePath}",
                    "Успіх",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                string fallbackPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Fallback_BoardingPass_{DateTime.Now.Ticks}.txt");
                File.WriteAllText(fallbackPath, sb.ToString());

                MessageBox.Show(
                    $"Сталася помилка при збереженні у вибраний файл. Можливо, до нього немає доступу.\n\nВідомість збережено у резервний текстовий файл:\n{fallbackPath}",
                    "Інформація",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
    }
}