using System;
using System.Linq;
using PaulStovell.Database.Management;

namespace PaulStovell.Database
{
    /// <summary>
    /// Contains the main entry point for the application.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The entry point for the application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        internal static int Main(string[] args)
        {
            if (args.Contains("help") || args.Contains("/?") || args.Contains("?") || args.Contains("-?") || args.Contains("-help") || args.Contains("--help"))
            {
                Console.WriteLine("Usage: Database.exe [connectionString] [-i] [-nocreate]");
                Console.WriteLine();
                Console.WriteLine("Example: Database.exe Server=(local);Database=YourDatabaseName;Trusted_connection=true;");
                Console.WriteLine();
                Console.WriteLine("  connectionString - Your connection string. May be enclosed in quotes.");
                Console.WriteLine("           -nowait - The application will terminate one the upgrade has completed.");
                Console.WriteLine("         -nocreate - If the database does not exist, it will not be created. ");
                Console.WriteLine("                     Recommended for servers.");
                Console.WriteLine();
                Console.WriteLine("Error codes: ");
                Console.WriteLine("     0 - Success");
                Console.WriteLine("    -1 - Incorrectly formatted connection string supplied.");
                Console.WriteLine("    -2 - Database does not exist and the application was not told not to create it.");
                Console.WriteLine("    -3 - An upgrade script failed.");
                Console.WriteLine("  -100 - Other error. Please see the application output.");
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return 0;
            }

            var nowait = args.Contains("-nowait");
            var nocreate = args.Contains("-nocreate");
            var connectionString = "server=(local);database=StovellBliki;trusted_connection=yes;";
            if (args.Length > 0)
            {
                connectionString = args[0];
            }

            try
            {
                // Validate the connection string
                SqlDatabaseHelper.ValidateConnectionStringOrThrow(connectionString);

                // Ensure the database exists - if not, create it
                Console.WriteLine("Database upgrade");
                Console.WriteLine("  Connection string: {0}", connectionString);
                Console.WriteLine();
                Console.WriteLine("Ensuring database exists...");
                var manager = new DatabaseManager(connectionString);
                if (!manager.DoesDatabaseExist())
                {
                    Console.WriteLine("Database does not exist. ");
                    if (!nocreate)
                    {
                        Console.WriteLine("Creating database...");
                        manager.CreateDatabase();
                        Console.WriteLine("Database created");
                    }
                    else
                    {
                        return -2;
                    }
                }
                else
                {
                    Console.WriteLine("Database exists.");
                }

                Console.WriteLine();
                // Upgrade information
                Console.WriteLine("Getting information about current setup...");
                Console.WriteLine("   Current database version: {0}", manager.GetCurrentVersion());
                Console.WriteLine("   Will be upgraded to:      {0}", manager.GetApplicationVersion());
                Console.WriteLine();
                Console.WriteLine("Performing upgrade...");
                var result = manager.PerformUpgrade();

                // List all scripts that were successfully executed
                Console.WriteLine("Upgraded from {0} to {1}", result.OriginalVersion, result.UpgradedVersion);
                Console.ResetColor();
                if (result.Scripts != null && result.Scripts.Count() > 0)
                {
                    Console.WriteLine("The following scripts were executed:");
                    foreach (var script in result.Scripts)
                    {
                        Console.WriteLine("    {0}", ("#" + script.VersionNumber).PadLeft(10));
                    }
                }

                // Display the result
                if (result.Successful)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Database upgrade complete");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Success");
                    Console.ResetColor();
                    return 0;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Database upgrade failed. Please see the list of scripts that were executed above and the error below.");
                    Console.WriteLine();
                    Console.ResetColor();
                    Console.WriteLine(result.Error);
                    return -3;
                }
            }
            catch (FormatException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Connection string was invalid: {0}", connectionString, ex.Message);
                Console.ResetColor();
                return -1;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unknown error occurred:");
                Console.ResetColor();
                Console.WriteLine(ex);
                return -100;
            }
            finally
            {
                if (!nowait)
                {
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey();
                }
            }
        }
    }
}