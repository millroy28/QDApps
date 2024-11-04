namespace QDApps.Models.WhereItAppModels.ViewModels
{
    public class ViewItem : Item
    {
        public List<Stash> AvailableStashes { get; set; } = [];
        public List<Tag> AvailableTags { get; set; } = [];
        public List<Tag> Tags { get; set; } = [];
    }
}
