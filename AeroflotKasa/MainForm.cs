using AeroflotKasa.Services;
using AeroflotKasa.Models;

namespace AeroflotKasa;

public partial class MainForm : Form
{
    private DataGridView dgvFlights = new();
    private Button btnAdd = new();
    private Button btnEdit = new();
    private Button btnDelete = new();
    private Button btnBook = new();
    private Button btnHelp = new();

    private readonly FlightService _flightService;
    private TextBox txtSearch = new();
    private Button btnSearch = new();

    public MainForm()
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
        this.KeyDown += MainForm_KeyDown;
        btnHelp.Click += (s, e) => ShowHelp();
        this.FormClosing += MainForm_FormClosing;

        var pnlTop = new Panel { Dock = DockStyle.Top, Height = 50 };

        var lblSearch = new Label { Text = "Пункт призначення:", Location = new Point(10, 15), AutoSize = true };
        txtSearch.Location = new Point(140, 10);
        txtSearch.Size = new Size(200, 25);
        txtSearch.TabIndex = 0;

        btnSearch.Text = "Знайти рейси";
        btnSearch.Location = new Point(350, 10);
        btnSearch.Size = new Size(150, 25);
        btnSearch.TabIndex = 1;

        var btnClear = new Button { Text = "Скинути пошук", Location = new Point(510, 10), Size = new Size(120, 25) };
        btnClear.TabIndex = 2;

        pnlTop.Controls.Add(lblSearch);
        pnlTop.Controls.Add(txtSearch);
        pnlTop.Controls.Add(btnSearch);
        pnlTop.Controls.Add(btnClear);

        this.Controls.Add(pnlTop);

        var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 60 };

        btnAdd.Text = "Додати";
        btnAdd.Location = new Point(10, 10);
        btnAdd.TabIndex = 3;

        btnEdit.Text = "Редагувати";
        btnEdit.Location = new Point(90, 10);
        btnEdit.TabIndex = 4;

        btnDelete.Text = "Видалити";
        btnDelete.Location = new Point(170, 10);
        btnDelete.TabIndex = 5;

        btnBook.Text = "Оформити квиток";
        btnBook.Location = new Point(250, 10);
        btnBook.Size = new Size(120, 25);
        btnBook.TabIndex = 6;

        btnHelp.Text = "Довідка (F1)";
        btnHelp.Location = new Point(750, 10);
        btnHelp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnHelp.TabIndex = 7;

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
        pnlTop.BringToFront();
        this.Controls.Add(pnlBottom);
    }

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
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
                try { _flightService.SaveData(); }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Помилка збереження", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }
            }
            else if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }

    private void DgvFlights_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        foreach (DataGridViewRow row in dgvFlights.Rows)
        {
            row.HeaderCell.Value = (row.Index + 1).ToString();
        }
        dgvFlights.RowHeadersWidth = 65;
    }

    private void MainForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F1)
        {
            ShowHelp();
        }
    }

    private void ShowHelp()
    {
        MessageBox.Show(
            "Програма 'Каса аерофлоту'.\n\n" +
            "Гарячі клавіші:\n" +
            "F1 - Довідка\n" +
            "Enter - Підтвердити дію\n" +
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
}