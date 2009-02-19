using Bindable.Linq.Interfaces;
using MbUnit.Framework;

namespace Bindable.Linq.Tests.TestLanguage.Expectations
{
    internal sealed class CountExpectation : IExpectation
    {
        private readonly int _count;

        public CountExpectation(int count)
        {
            _count = count;
        }

        public void Validate(IScenario scenario)
        {
            var count = ((IBindableCollection)scenario.BindableLinqQuery).Count;
            Assert.AreEqual(_count, count);
        }
    }
}
