using Bindable.Cms.Tests.Helpers;
using Gallio.Framework;
using Gallio.Model;
using MbUnit.Framework;

namespace Bindable.Cms.Tests.Domain.Repositories.Base
{
    public class BaseRepositoryTestFixture
    {
        public TestableDatabase Database { get; private set; }

        [TestFixtureSetUp]
        public void Start()
        {
            Database = new TestableDatabase();
        }

        [TestFixtureTearDown]
        public void Stop()
        {
            if (TestContext.CurrentContext.Outcome.Status == TestStatus.Passed)
            {
                Database.Dispose();
            }
        }
    }
}