using Bindable.Linq.Interfaces;
using MbUnit.Framework;

namespace Bindable.Linq.Tests.TestLanguage.Expectations
{
    internal sealed class CurrentValueExpectation : IExpectation
    {
        private readonly object _expectedValue;

        public CurrentValueExpectation(object currentValue)
        {
            _expectedValue = currentValue;
        }

        public void Validate(IScenario scenario)
        {
            var value = ((IBindable)scenario.BindableLinqQuery).Current;
            Assert.AreEqual(_expectedValue, value);
        }
    }
}