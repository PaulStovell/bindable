using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using PaulStovell.Database.Management;
using PaulStovell.Database.Management.Execution;
using PaulStovell.Database.Management.ScriptProviders;
using PaulStovell.Database.Management.VersionTrackers;

namespace PaulStovell.Tests.Database.Management
{
    [TestFixture]
    public class DatabaseUpgraderTests
    {
        [Test(Description="Tests the database upgrade process by running through a series of test scripts. This test will create a temporary database, so run at your own risk.")]
        [Ignore]
        public void MockUpgradeProcessTest()
        {
            using (var temporary = new TemporarySqlDatabase())
            {
                var executor = new SqlScriptExecutor();
                var versionTracker = new SchemaVersionsTableSqlVersionTracker();
                var scriptProvider = new EmbeddedSqlScriptProvider(
                    Assembly.GetExecutingAssembly(),
                    versionNumber => string.Format(
                                         "PaulStovell.Tests.Database.Management.TestScripts.Script{0}.sql",
                                         versionNumber.ToString().PadLeft(4, '0')));

                var upgrader = new DatabaseUpgrader(temporary.ConnectionString, scriptProvider, versionTracker, executor);

                var upgrade1 = upgrader.PerformUpgrade();
                Assert.AreEqual(0, upgrade1.OriginalVersion);
                Assert.AreEqual(3, upgrade1.UpgradedVersion);
                Assert.AreEqual(3, upgrade1.Scripts.Count());

                var upgrade2 = upgrader.PerformUpgrade();
                Assert.AreEqual(3, upgrade2.OriginalVersion);
                Assert.AreEqual(3, upgrade2.UpgradedVersion);
                Assert.AreEqual(0, upgrade2.Scripts.Count());

                // Script 2 creates a "Foo" table, and script 2 adds a couple of records to it. Let's 
                // see if it worked. This also proves that the final script isn't called twice - 
                // otherwise there would be four records rather than two.
                var fooNames = new List<string>();
                using (var connection = new SqlConnection(temporary.ConnectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "select FooName from Foo order by FooName";
                        command.CommandType = CommandType.Text;

                        connection.Open();
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            fooNames.Add(reader[0].ToString());
                        }
                    }
                }
                Assert.AreEqual(2, fooNames.Count);
                Assert.AreEqual("Goodbye", fooNames[0]);
                Assert.AreEqual("Hello", fooNames[1]);
            }
        }
    }
}