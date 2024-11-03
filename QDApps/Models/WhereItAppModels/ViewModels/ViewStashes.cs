namespace QDApps.Models.WhereItAppModels.ViewModels
{
    public class ViewStashes
    {
        public int UserId { get; set; }
        public string StashName { get; set; } = null!;
        public int StashId { get; set; }
        public int ItemCount { get; set; } = 0;
        public List<Tag> Tags { get; set; } = [];
    }
}
