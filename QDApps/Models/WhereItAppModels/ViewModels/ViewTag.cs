namespace QDApps.Models.WhereItAppModels.ViewModels
{
    public class ViewTag : Tag
    {
        public List<ViewItems> ViewItems { get; set; } = [];
        public string EditedTagName { get; set; } = string.Empty;
        public string? EditedTagColor { get; set; } = string.Empty;
        public string? EditedTagDescription { get; set; } = string.Empty;
        public Dictionary<string, string> AvailableTagColors { get; set; } = [];
    }
}
