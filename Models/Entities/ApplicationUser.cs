using Microsoft.AspNetCore.Identity;

namespace DigitalDetectiveAgency.Models.Entities;

public class ApplicationUser : IdentityUser
{
    public string DetectiveName { get; set; } = string.Empty;
    public bool HasCompletedTutorial { get; set; }
}