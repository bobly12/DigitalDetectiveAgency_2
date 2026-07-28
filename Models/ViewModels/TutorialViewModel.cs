namespace DigitalDetectiveAgency.Models.ViewModels
{
    public class TutorialViewModel
    {
        // Detective speaking
        public string Speaker { get; set; } = "Chief Investigator";

        // Large title
        public string Title { get; set; } = "Detective Training";

        // Main dialogue
        public List<string> Dialogue { get; set; } = new();

        // Whether this is the player's first time
        public bool IsFirstTime { get; set; } = true;

        // First playable case
        public int FirstCaseId { get; set; }

        // Skip button visibility
        public bool AllowSkip { get; set; } = true;

        // Redirect after tutorial
        public string ContinueAction { get; set; } = "Board";
        public string ContinueController { get; set; } = "Board";
    }
}