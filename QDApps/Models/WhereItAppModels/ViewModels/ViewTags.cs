namespace QDApps.Models.WhereItAppModels.ViewModels
{
    public class ViewTags
    {
        public int UserId {  get; set; }
        public int TagId { get; set; }
        public string TagName { get; set; } = null!;
        public int ItemCount { get; set; }
        public string? TagColor {  get; set; }
    }
}
