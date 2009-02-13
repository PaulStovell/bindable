namespace Bindable.Cms.Domain.Infrastructure.Services.Diagnostics
{
    public class DiagnosticIssue
    {
        public int IssueId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public bool CanFix { get; set; }
    }
}