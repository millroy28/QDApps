namespace QDApps.Models.WhereItAppModels.ViewModels
{
    public class Status
    {
        public bool IsSuccess { get; set; } = false;
        public int RecordId { get; set; } = -1;
        public string? StatusMessage { get; set; }
    }
}
