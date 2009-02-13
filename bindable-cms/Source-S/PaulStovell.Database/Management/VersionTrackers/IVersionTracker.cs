using PaulStovell.Database.Management.ScriptProviders;

namespace PaulStovell.Database.Management.VersionTrackers
{
    /// <summary>
    /// This interface is provided to allow different projects to store version information differently.
    /// </summary>
    public interface IVersionTracker
    {
        /// <summary>
        /// Recalls the version number of a database specified in a given connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        int RecallVersionNumber(string connectionString);

        /// <summary>
        /// Records a database upgrade for a database specified in a given connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="script">The script.</param>
        void StoreUpgrade(string connectionString, IScript script);
    }
}