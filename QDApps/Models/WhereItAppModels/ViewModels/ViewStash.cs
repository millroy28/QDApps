namespace QDApps.Models.WhereItAppModels.ViewModels
{
    public class ViewStash : Stash
    {
        public List<ViewItems> ViewItems { get; set; } = [];
        public List<Stash> AvailableStashes { get; set; } = [];
        public List<Tag> AvailableTags { get; set; } = [];
        public int DestinationStashId {  get; set; }
        public int AddTagId { get; set; }
        public string EditedStashName {  get; set; } = string.Empty;
    }
}
