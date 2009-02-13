using System;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace Bindable.DatabaseManagement
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
            try
            {
                var builder = new SqlConnectionStringBuilder(connectionString);
                var server = new Server(builder.DataSource);
                return server.Databases.Contains(builder.InitialCatalog);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Enables the server login for windows user.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="username">The username.</param>
        public static void EnableServerLoginForWindowsUser(string connectionString, string username)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            var server = new Server(builder.DataSource);
            var database = new Database(server, builder.InitialCatalog);
            if (server.Logins.Contains(username)) return;
            
            var login = new Login(server, username);
            login.LoginType = LoginType.WindowsUser;
            login.DefaultDatabase = builder.InitialCatalog;
            login.Create();
        }

        /// <summary>
        /// Enables the database role for login.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="loginUsername">The login username.</param>
        /// <param name="role">The role.</param>
        public static void EnableDatabaseRoleForLogin(string connectionString, string loginUsername, string role)
        {
            EnableServerLoginForWindowsUser(connectionString, loginUsername);

            var builder = new SqlConnectionStringBuilder(connectionString);
            var server = new Server(builder.DataSource);
            var database = new Database(server, builder.InitialCatalog);
            var user = new User(database, loginUsername);
            if (database.Users.Contains(loginUsername))
            {
                user.Login = loginUsername;
                user.Create();
            }
            user.AddToRole(role);
        }

        public static string GetServerName(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            return builder.DataSource;
        }

        public static string GetDatabaseName(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            return builder.InitialCatalog;
        }

        private static void CreateInternal(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            var server = new Server(builder.DataSource);
            var database = new Database(server, builder.InitialCatalog);
            database.Create();
        }

        private static void DestroyInternal(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            var server = new Server(builder.DataSource);
            server.KillAllProcesses(builder.InitialCatalog);
            server.KillDatabase(builder.InitialCatalog);
        }
    }
}