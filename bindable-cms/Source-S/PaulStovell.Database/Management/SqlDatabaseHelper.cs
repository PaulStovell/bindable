using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace PaulStovell.Database.Management
{
    /// <summary>
    /// A class that helps for creating and destroying SQL Server databases.
    /// </summary>
    public static class SqlDatabaseHelper
    {
        /// <summary>
        /// Creates a database specified in a given connection string. Throws an exception if the database 
        /// already exists.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public static void Create(string connectionString)
        {
            if (Exists(connectionString))
            {
                throw new Exception(string.Format("The database specified by the connection string '{0}' already exists.", connectionString));
            }
            CreateInternal(connectionString);
        }

        /// <summary>
        /// Creates a database specified in a given connection string. Ignores the call if the database
        /// already exists.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public static void CreateOrContinue(string connectionString)
        {
            if (!Exists(connectionString))
            {
                CreateInternal(connectionString);
            }
        }

        /// <summary>
        /// Destroys the database specified in a given connection string. Throws an exception if the 
        /// database does not exist.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public static void Destroy(string connectionString)
        {
            if (!Exists(connectionString))
            {
                throw new Exception(string.Format("The database specified by the connection string '{0}' does not exists.", connectionString));
            }
            DestroyInternal(connectionString);
        }

        /// <summary>
        /// Destroys the database specified in a given connection string. Ignores the call if the database
        /// does not exist.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public static void DestroyOrContinue(string connectionString)
        {
            if (Exists(connectionString))
            {
                DestroyInternal(connectionString);
            }
        }

        /// <summary>
        /// Attempts to validate a connection string and throws an exception if the string could not be validated.
        /// </summary>
        /// <param name="connectionString">The connection string to validate.</param>
        public static void ValidateConnectionStringOrThrow(string connectionString)
        {
            new SqlConnectionStringBuilder(connectionString);
        }

        /// <summary>
        /// Confirms whether a database specified at a given connection string exists.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public static bool Exists(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            Server server = new Server(builder.DataSource);
            return server.Databases.Contains(builder.InitialCatalog);
        }

        private static void CreateInternal(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            Server server = new Server(builder.DataSource);
            Microsoft.SqlServer.Management.Smo.Database database = new Microsoft.SqlServer.Management.Smo.Database(server, builder.InitialCatalog);
            database.Create();
        }

        private static void DestroyInternal(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            Server server = new Server(builder.DataSource);
            server.KillAllProcesses(builder.InitialCatalog);
            server.KillDatabase(builder.InitialCatalog);
        }
    }
}