using System;
using System.Collections.Generic;
using System.Text;

namespace PaulStovell.Database.Management
{
    /// <summary>
    /// A class that can be used within a "using" block to create a temporary SQL Server database that 
    /// is destroyed when the object is disposed.
    /// </summary>
    public sealed class TemporarySqlDatabase : IDisposable
    {
        private string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporarySqlDatabase"/> class.
        /// </summary>
        public TemporarySqlDatabase()
        {
            string databaseName = "TestDatabase_" + Guid.NewGuid().ToString();
            _connectionString = string.Format("Server=(local);Database={0};Trusted_connection=true", databaseName);
            SqlDatabaseHelper.Create(_connectionString);
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            SqlDatabaseHelper.DestroyOrContinue(_connectionString);
            GC.SuppressFinalize(this);
        }
    }
}