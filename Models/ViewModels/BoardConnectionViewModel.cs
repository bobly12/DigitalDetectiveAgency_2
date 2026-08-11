// Models/ViewModels/BoardConnectionViewModel.cs
namespace DigitalDetectiveAgency.Models.ViewModels;

public class BoardConnectionViewModel
{
    public int Id { get; set; }
    public string FromType { get; set; } = string.Empty;
    public int FromId { get; set; }
    public string ToType { get; set; } = string.Empty;
    public int ToId { get; set; }
}

// NEW — a pair the player has already tried and got rejected.
// No Id needed since these aren't real connections, just a memory of attempts.
public class BoardTriedPairViewModel
{
    public string FromType { get; set; } = string.Empty;
    public int FromId { get; set; }
    public string ToType { get; set; } = string.Empty;
    public int ToId { get; set; }

    // NEW - tries system
    public int WrongTryCount { get; set; }
    public bool IsLocked { get; set; } // true once WrongTryCount hits the cap
}