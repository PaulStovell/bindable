using System.Linq;

namespace Bindable.Cms.Domain.Infrastructure.Services.Diagnostics
{
    public class DiagnosticService : IDiagnosticService
    {
        private readonly IDiagnosticTest[] _tests;

        public DiagnosticService(IDiagnosticTest[] tests)
        {
            _tests = tests;
        }

        public DiagnosticIssue[] Diagnose()
        {
            return _tests.SelectMany(test => test.Diagnose()).ToArray();
        }
    }
}