using System;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using PaulStovell.Common;
using PaulStovell.Database.Management.ScriptProviders;

namespace PaulStovell.Database.Management.Execution
{
    /// <summary>
    /// A standard implementation of the IScriptExecutor interface that executes against a SQL Server 
    /// database using SQL Server SMO.
    /// </summary>
    public sealed class SqlScriptExecutor : IScriptExecutor
    {
        /// <summary>
        /// Executes the specified script against a database at a given connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="script">The script.</param>
        public void Execute(string connectionString, IScript script)
        {
            TraceHelper.TraceInformation("Executing SQL Server script '{0}'", script.Name);
            var connection = new SqlConnection(connectionString);

            Server server = new Server(new ServerConnection(connection));
            
            try
            {
                server.ConnectionContext.ExecuteNonQuery(script.Contents);
            }
            catch (ExecutionFailureException ex)
            {
                TraceHelper.TraceInformation("SQL exception has occured. Transaction rolled back for script: '{0}'", script.Name);
                var sqlException = ex.InnerException as SqlException;
                if (sqlException != null)
                {
                    TraceHelper.TraceError("Batch line number: {0}  Message: {3}", sqlException.LineNumber, sqlException.Procedure, sqlException.Number, sqlException.Message);
                }
                else
                {
                    TraceHelper.TraceError(ex);
                }
                TraceHelper.TraceError(ex.Message);
                throw;
            }
            catch (SqlException ex)
            {
                TraceHelper.TraceInformation("SQL exception has occured. Transaction rolled back for script: '{0}'", script.Name);
                TraceHelper.TraceError("Line: {0}  Error: {1}  Message: {2}", ex.LineNumber, ex.Number, ex.Message);
                TraceHelper.TraceError(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                TraceHelper.TraceInformation("Exception has occured. Transaction rolled back for script: '{0}'", script.Name);
                TraceHelper.TraceError(ex);
                throw;
            }
        }
    }
}