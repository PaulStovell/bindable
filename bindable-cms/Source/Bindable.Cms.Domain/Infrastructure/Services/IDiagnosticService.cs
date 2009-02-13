using Bindable.Cms.Domain.Infrastructure.Services.Diagnostics;

namespace Bindable.Cms.Domain.Infrastructure.Services
{
    public interface IDiagnosticService
    {
        DiagnosticIssue[] Diagnose();
    }
}
