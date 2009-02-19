using MbUnit.Framework;

namespace Bindable.Linq.Tests.TestLanguage.Expectations
{
    /// <summary>
    /// Sets up an expectation that a PropertyChanged event will be raised.
    /// </summary>
    internal sealed class RaisePropertyEventExpectation : IExpectation
    {
        private readonly string _propertyName;

        public RaisePropertyEventExpectation(string propertyName)
        {
            _propertyName = propertyName;
        }

        public void Validate(IScenario scenario)
        {
            var lastEvent = scenario.PropertyEventMonitor.Dequeue();
            if (lastEvent == null)
            {
                Assert.Fail("Property changed event for property '{0}' was expected but was not raised.", _propertyName);
            }
            else if (lastEvent.Arguments.PropertyName != _propertyName)
            {
                Assert.Fail("Property changed event for property '{0}' was expected, but '{1}' was raised instead.", _propertyName, lastEvent.Arguments.PropertyName);
            }
        }
    }
}
