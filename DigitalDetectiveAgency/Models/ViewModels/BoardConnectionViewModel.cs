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