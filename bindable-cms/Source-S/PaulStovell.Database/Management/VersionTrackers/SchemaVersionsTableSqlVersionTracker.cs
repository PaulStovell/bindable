using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using PaulStovell.Common;
using PaulStovell.Database.Management.ScriptProviders;

namespace PaulStovell.Database.Management.VersionTrackers
{
    /// <summary>
    /// An implementation of the IVersionTacker interface which tracks version numbers for a SQL Server database
    /// using a table called dbo.SchemaVersions.
    /// </summary>
    public sealed class SchemaVersionsTableSqlVersionTracker : IVersionTracker
    {
        /// <summary>
        /// Recalls the version number of a database specified in a given connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public int RecallVersionNumber(string connectionString)
        {
            TraceHelper.TraceInformation("Detecting the current database version.");
            using (TraceHelper.Indent())
            {
                // Check whether the SchemaVersions table even exists. If not, return it.
                using (var connection = new SqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "select count(*) from sys.objects where type='U' and name='SchemaVersions'";
                        command.CommandType = CommandType.Text;
                        connection.Open();

                        var result = 0;
                        int.TryParse(command.ExecuteScalar().ToString(), out result);

                        if (result == 0)
                        {
                            TraceHelper.TraceInformation("The SchemaVersions table could not be found. The database is assumed to be at version 0.");
                            return 0;
                        }
                    }
                }

                // Call the GetCurrentVersionNumber stored procedure
                using (var connection = new SqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "GetCurrentVersionNumber";
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        var versionResult = command.ExecuteScalar();
                        var result = 0;

                        if ((versionResult != null) && (versionResult.ToString() != string.Empty))
                        {
                            result = int.Parse(versionResult.ToString());
                        }

                        Trace.TraceInformation("Version {0} detected.", result);
                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// Records a database upgrade for a database specified in a given connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="script">The script.</param>
        public void StoreUpgrade(string connectionString, IScript script)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "RecordVersionUpgrade";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@versionNumber", script.VersionNumber);
                    command.Parameters.AddWithValue("@sourceIdentifier", script.SourceIdentifier);
                    command.Parameters.AddWithValue("@scriptName", script.Name);
                    connection.Open();
                    command.ExecuteNonQuery();
                    Trace.TraceInformation("Upgrade to version {0} stored successfully.", script.VersionNumber);
                }
            }
        }
    }
}