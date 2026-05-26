using AeroflotKasa.Services;

namespace AeroflotKasa;

public partial class MainForm : Form
{
    private readonly FlightService _flightService;
    private TextBox txtSearch = new();
    private Button btnSearch = new();

    public MainForm()
    {
        _flightService = new FlightService();
        InitializeComponentCustom();
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
    }
}