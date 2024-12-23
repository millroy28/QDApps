namespace QDApps.Models.WhereItAppModels
{
    public class ItemTagNames
    {
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string StashName { get; set; } = string.Empty;
        public int TagId { get; set; }
        public string TagName { get; set; } = string.Empty;
        public string? TagColor {  get; set; } = string.Empty;
        public string? TagDescription { get; set; } = string.Empty;
    }
}
