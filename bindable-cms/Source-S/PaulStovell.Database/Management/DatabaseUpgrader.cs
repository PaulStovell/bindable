using System;
using System.Collections.Generic;
using System.Diagnostics;
using PaulStovell.Common;
using PaulStovell.Database.Management;
using PaulStovell.Database.Management.Execution;
using PaulStovell.Database.Management.ScriptProviders;
using PaulStovell.Database.Management.VersionTrackers;

namespace PaulStovell.Database.Management
{
    /// <summary>
    /// This class orchestrates the database upgrade process.
    /// </summary>
    public class DatabaseUpgrader
    {
        private readonly string _connectionString;
        private readonly IScriptProvider _scriptProvider;
        private readonly IVersionTracker _versionTracker;
        private readonly IScriptExecutor _scriptExecutor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseUpgrader"/> class.
        /// </summary>
        public DatabaseUpgrader(string connectionString, IScriptProvider scriptProvider, IVersionTracker versionTracker, IScriptExecutor scriptExecutor)
        {
            _connectionString = connectionString;
            _scriptExecutor = scriptExecutor;
            _versionTracker = versionTracker;
            _scriptProvider = scriptProvider;
        }

        /// <summary>
        /// Performs the database upgrade.
        /// </summary>
        public DatabaseUpgradeResult PerformUpgrade()
        {
            var originalVersion = 0;
            var currentVersion = 0;
            var maximumVersion = 0;
            var scripts = new List<IScript>();
            try
            {
                TraceHelper.TraceInformation("Beginning database upgrade. Connection string is: '{0}'", _connectionString);

                originalVersion = _versionTracker.RecallVersionNumber(_connectionString);
                maximumVersion = _scriptProvider.GetHighestScriptVersion();

                currentVersion = originalVersion;
                while (currentVersion < maximumVersion)
                {
                    currentVersion++;
                    TraceHelper.TraceInformation("Upgrading to version: '{0}'", currentVersion);
                    using (TraceHelper.Indent())
                    {
                        IScript script = _scriptProvider.GetScript(currentVersion);
                        _scriptExecutor.Execute(_connectionString, script);
                        _versionTracker.StoreUpgrade(_connectionString, script);
                        scripts.Add(script);
                    }
                }
                TraceHelper.TraceInformation("Upgrade successful");
                return new DatabaseUpgradeResult(scripts, originalVersion, currentVersion, true, null);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Upgrade failed", ex);
                return new DatabaseUpgradeResult(scripts, originalVersion, currentVersion, false, ex);
            }
        }
    }
}