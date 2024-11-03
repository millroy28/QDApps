namespace QDApps.Models.WhereItAppModels.ViewModels
{
    public class ViewInventory
    {
        public List<ViewStashes> Stashes { get; set; } = [];
        public List<ViewItems> Items { get; set; } = [];
        public List<ViewTags> Tags { get; set; } = [];
        public string UserName { get; set; } = string.Empty;

    }
}
