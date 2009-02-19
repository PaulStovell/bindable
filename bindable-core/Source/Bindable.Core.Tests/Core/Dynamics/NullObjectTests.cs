using System.Collections;
using Bindable.Core.Dynamics;
using MbUnit.Framework;

namespace Bindable.Core.Tests.Core.Dynamics
{
    [TestFixture]
    public class NullObjectTests
    {
        [Test]
        public void Examples()
        {
            var test = NullObject.For<IEnumerable>();
            var test2 = test.GetEnumerator();
            test2.MoveNext();
            test2.MoveNext();
            test2.MoveNext();
            test2.MoveNext();
            test2.MoveNext();
            test2.MoveNext();
            test2.MoveNext();
            test2.MoveNext();
            test2.MoveNext();
        }
    }
}