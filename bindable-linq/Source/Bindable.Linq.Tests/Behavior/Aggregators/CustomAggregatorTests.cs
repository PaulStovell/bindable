using System.Linq;
using Bindable.Linq;
using Bindable.Linq.Aggregators;
using Bindable.Linq.Tests.MockObjectModel;
using Bindable.Linq.Tests.TestLanguage;
using Bindable.Linq.Tests.TestLanguage.Helpers;
using MbUnit.Framework;

namespace Bindable.Linq.Tests.Behavior.Aggregators
{
    /// <summary>
    /// Contains unit tests for the <see cref="CustomAggregator{TSource,TAccumulate}"/> class.
    /// </summary>
    [TestFixture]
    public class CustomAggregatorTests : TestFixture
    {
        /// <summary>
        /// Tests that the aggregator calculates correctly.
        /// </summary>
        [Test]
        public void CustomAggregatorAccumulateTypeCalculate()
        {
            Specification.Title("Custom aggregators")
                .TestingOver<Contact>()
                .UsingBindableLinq(contacts => contacts.AsBindable().Aggregate(0, (i, contact) => i + contact.Name.Length))
                .UsingStandardLinq(contacts => contacts.Aggregate(0, (i, contact) => i + contact.Name.Length))
                .Scenario("Name lengths calculated correctly",
                    With.Inputs(Tom, Jack, Sally),
                    step => Upon.Evaluate().ItWill.Raise(PropertyChange("HasEvaluated")).And.Raise(PropertyChange("Current")).And.HaveCurrentValue(12)
                    )
                .Scenario("Add items will cause a refresh",
                    With.Inputs(Tom, Jack, Sally),
                    step => Upon.Evaluate().ItWill.Raise(PropertyChange("HasEvaluated")).And.Raise(PropertyChange("Current")).And.HaveCurrentValue(12),
                    step => Upon.Add(Fred).ItWill.Raise(PropertyChange("Current")).And.HaveCurrentValue(16),
                    step => Upon.Add(Jack, John).ItWill.Raise(PropertyChange("Current")).And.Raise(PropertyChange("Current")).And.HaveCurrentValue(24),
                    step => Upon.Add(NoName).ItWill.NotRaiseAnything().And.HaveCurrentValue(24)
                    )
                .Scenario("Move items will do nothing",
                    With.Inputs(Tom, Jack, Sally),
                    step => Upon.Evaluate().ItWill.Raise(PropertyChange("HasEvaluated")).And.Raise(PropertyChange("Current")).And.HaveCurrentValue(12),
                    step => Upon.Move(1, Tom).ItWill.NotRaiseAnything().And.HaveCurrentValue(12)
                    )
                .Scenario("Replace items will cause a refresh",
                    With.Inputs(Tom, Jack, Sally),
                    step => Upon.Evaluate().ItWill.Raise(PropertyChange("HasEvaluated")).And.Raise(PropertyChange("Current")).And.HaveCurrentValue(12),
                    step => Upon.Replace(Jack).With(Tim).ItWill.Raise(PropertyChange("Current")).And.HaveCurrentValue(11),
                    step => Upon.Replace(Sally).With(Brian).ItWill.NotRaiseAnything().And.HaveCurrentValue(11)
                    )
                .Scenario("Remove items will cause a refresh",
                    With.Inputs(Tom, Jack, Sally),
                    step => Upon.Evaluate().ItWill.Raise(PropertyChange("HasEvaluated")).And.Raise(PropertyChange("Current")).And.HaveCurrentValue(12),
                    step => Upon.Remove(Tom).ItWill.Raise(PropertyChange("Current")).And.HaveCurrentValue(9),
                    step => Upon.Remove(Tim).ItWill.NotRaiseAnything().And.HaveCurrentValue(9)
                    )
                .Verify();
        }
    }
}