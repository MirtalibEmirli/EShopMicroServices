namespace Catalog.Api.Publisher
{
    public class EmailNotificationEvent
    {
        public string ToEmail { get; set; } = null!;
        public string? UserId { get; set; }
        public string? ProductName { get; set; }
        public string Subject { get; set; } = null!;
        public string Template { get; set; } = null!;
    }

}
