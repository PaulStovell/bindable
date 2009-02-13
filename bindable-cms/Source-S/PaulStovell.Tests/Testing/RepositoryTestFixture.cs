using NUnit.Framework;

namespace PaulStovell.Tests.Testing
{
    public class RepositoryTestFixture
    {
        public TemporaryDatabase Database { get; private set; }

        [TestFixtureSetUp]
        public void Start()
        {
            Database = new TemporaryDatabase();    
        }

        [TestFixtureTearDown]
        public void Stop()
        {
            Database.Dispose();
        }
    }
}