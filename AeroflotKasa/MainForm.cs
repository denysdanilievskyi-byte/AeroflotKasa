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

        this.Controls.Add(dgvFlights);
        pnlTop.BringToFront();

        this.Controls.Add(pnlBottom);
    }
    private void RefreshGrid(IEnumerable<Flight> flights)
    {
        dgvFlights.DataSource = null;
        dgvFlights.DataSource = flights.ToList();
    }
}