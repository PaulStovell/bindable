using System;
using System.Collections.Generic;
using System.Web;
using Bindable.Cms.Database;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Bindable.Cms.Domain.Infrastructure.Services.Diagnostics.Tests
{
    public class CanConnectToDatabaseTest : IDiagnosticTest
    {
        private readonly IApplicationDatabase _database;

        public CanConnectToDatabaseTest(IApplicationDatabase database)
        {
            _database = database;
        }

        public IEnumerable<DiagnosticIssue> Diagnose()
        {
            if (!_database.DoesDatabaseExist())
            {
                var description = new StringBuilder();
                description.AppendFormat("Could not connect to database '{0}' on server '{1}'. ", _database.DatabaseName, _database.ServerName);
                description.AppendFormat("This could occur because either the database or server do not exist, are not online, are incorrect, or the user '{0}' does not have access. ", "NT Authority\\NetworkService");

                var instructions = new StringBuilder();
                instructions.AppendLine("  - Ensure that the server exists");
                instructions.AppendLine("  - Ensure that the database exists");
                instructions.AppendLine("  - Connect to the server using SQL Server Management Studio");
                instructions.AppendLine("  - Ensure the user is a member of the db_owner role");
                
                yield return new DiagnosticIssue()
                                 {
                                     IssueId = 1,
                                     CanFix = false,
                                     Description = description.ToString(),
                                     Instructions = instructions.ToString(),
                                 };
            }
        }

        public void AttemptToFix(int issueId)
        {
            throw new NotImplementedException();
        }
    }
}
