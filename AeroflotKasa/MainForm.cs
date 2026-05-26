using AeroflotKasa.Services;

namespace AeroflotKasa;

public partial class MainForm : Form
{
    private readonly FlightService _flightService;

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
    }
}