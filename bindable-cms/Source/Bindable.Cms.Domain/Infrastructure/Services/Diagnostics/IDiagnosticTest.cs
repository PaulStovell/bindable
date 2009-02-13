using System.Collections.Generic;
namespace Bindable.Cms.Domain.Infrastructure.Services.Diagnostics
{
    public interface IDiagnosticTest
    {
        IEnumerable<DiagnosticIssue> Diagnose();
        void AttemptToFix(int issueId);
    }
}