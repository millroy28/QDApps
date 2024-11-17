namespace QDApps.Models.WhereItAppModels.ViewModels
{
    public class ViewTag : Tag
    {
        public List<ViewItems> ViewItems { get; set; } = [];
        public string EditedTagName {  get; set; } = string.Empty;
    }
}
