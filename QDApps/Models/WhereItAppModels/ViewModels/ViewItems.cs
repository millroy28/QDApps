namespace QDApps.Models.WhereItAppModels.ViewModels
{
    public class ViewItems
    {
        public bool Selected { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; } 
        public string ItemName { get; set; } = null!;
        public int StashId { get; set; }
        public string StashName { get; set; } = null!;
        public List<Tag> Tags { get; set; } = [];

    }
}
