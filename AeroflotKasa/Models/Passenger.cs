namespace AeroflotKasa.Models;

/// <summary>
/// Представляє пасажира, який оформлює квиток.
/// </summary>
public class Passenger
{
    /// <summary>
    /// Повне ім'я пасажира (ПІБ).
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Паспортні дані або дані іншого документа, що посвідчує особу.
    /// </summary>
    public string PassportData { get; set; } = string.Empty;
}