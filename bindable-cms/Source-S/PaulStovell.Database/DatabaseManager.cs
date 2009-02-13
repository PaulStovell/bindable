using System.Reflection;
using PaulStovell.Database.Management;
using PaulStovell.Database.Management.Execution;
using PaulStovell.Database.Management.ScriptProviders;
using PaulStovell.Database.Management.VersionTrackers;

namespace PaulStovell.Database
{
    public class DatabaseManager : IDatabaseManager
    {
        private readonly string _connectionString;
        private readonly IScriptExecutor _scriptExecutor;
        private readonly IVersionTracker _versionTracker;
        private readonly IScriptProvider _scriptProvider;
        private readonly DatabaseUpgrader _upgrader;

        public DatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
            _scriptExecutor = new SqlScriptExecutor();
            _versionTracker = new SchemaVersionsTableSqlVersionTracker();
            _scriptProvider = new EmbeddedSqlScriptProvider(
                Assembly.GetExecutingAssembly(),
                versionNumber => string.Format(
                                     "PaulStovell.Database.Scripts.Sequential.Script{0}.sql",
                                     versionNumber.ToString().PadLeft(4, '0')));
            _upgrader = new DatabaseUpgrader(connectionString, _scriptProvider, _versionTracker, _scriptExecutor);
        }

        public bool DoesDatabaseExist()
        {
            return SqlDatabaseHelper.Exists(_connectionString);
        }

        public void CreateDatabase()
        {
            SqlDatabaseHelper.CreateOrContinue(_connectionString);
        }

        public void DestroyDatabase()
        {
            SqlDatabaseHelper.DestroyOrContinue(_connectionString);
        }

        public int GetCurrentVersion()
        {
            return _versionTracker.RecallVersionNumber(_connectionString);
        }

        public int GetApplicationVersion()
        {
            return _scriptProvider.GetHighestScriptVersion();
        }

        public DatabaseUpgradeResult PerformUpgrade()
        {
            return _upgrader.PerformUpgrade();
        }
    }
}
