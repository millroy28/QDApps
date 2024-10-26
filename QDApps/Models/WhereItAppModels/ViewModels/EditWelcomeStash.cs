namespace QDApps.Models.WhereItAppModels.ViewModels
{
    public class EditWelcomeStash
    {
        public required string Stash { get; set; }
        public required string Item { get; set; }
        public required string Tag { get; set; }
        public int UserId { get; set; }
    }
}
