using AeroflotKasa.Services;

namespace AeroflotKasa;

public partial class MainForm : Form
{
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

        this.Controls.Add(pnlBottom);
    }
}