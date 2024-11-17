namespace QDApps.Models.WhereItAppModels.ViewModels
{
    public class ViewItem : Item
    {
        public string EditedItemName { get; set; } = string.Empty;
        public int? DestinationStashId { get; set; } 
        public List<Stash> AvailableStashes { get; set; } = [];
        public List<Tag> AvailableTags { get; set; } = [];
        public List<Tag> Tags { get; set; } = [];
    }
}
