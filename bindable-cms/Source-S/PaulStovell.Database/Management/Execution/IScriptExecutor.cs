using PaulStovell.Database.Management.ScriptProviders;

namespace PaulStovell.Database.Management.Execution
{
    /// <summary>
    /// This interface is implemented by classes that execute upgrade scripts against a database.
    /// </summary>
    public interface IScriptExecutor
    {
        /// <summary>
        /// Executes the specified script against a database at a given connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="script">The script.</param>
        void Execute(string connectionString, IScript script);
    }
}