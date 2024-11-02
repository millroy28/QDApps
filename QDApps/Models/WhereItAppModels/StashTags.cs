namespace QDApps.Models.WhereItAppModels
{
    public class StashTags
    {
        public int UserId { get; set; }
        public int StashId { get; set; }
        public string TagName { get; set; } = string.Empty;
        public int TagId { get; set; }  
    }
}
