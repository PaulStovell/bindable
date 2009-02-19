using System.Linq;
using Bindable.Linq;
using Bindable.Linq.Tests.TestLanguage.Helpers;
using MbUnit.Framework;
using Bindable.Linq.Tests.TestLanguage;
using Bindable.Linq.Tests.MockObjectModel;

namespace Bindable.Linq.Tests.Behavior.Iterators
{
    /// <summary>
    /// Contains high-level behavioral tests for the Where iterator. 
    /// </summary>
    [TestFixture]
    public class WhereBehavior : TestFixture
    {
        [Test]
        public void WhereSpecification()
        {
            Specification.Title("Where() specification")
                .TestingOver<Contact>()
                .UsingBindableLinq(inputs => inputs.AsBindable().Where(p => p.Name.Length >= 4))
                .UsingStandardLinq(inputs => inputs.Where(p => p.Name.Length >= 4))
                .WithCompatabilityLevel(CompatabilityLevel.FullyCompatibleExceptOrdering)
                .Scenario("Delayed evaluation",
                    With.Inputs(Mike, Tom, Jack),
                    step => Upon.Construction().ItWill.NotHaveEvaluated(),
                    step => Upon.Evaluate().ItWill.HaveCount(2)
                    )
                .Scenario("Adding items",
                    With.Inputs(Mike, Tom, Jack),
                    step => Upon.Add(John, Mick).ItWill.NotRaiseAnything(),
                    step => Upon.Evaluate().ItWill.NotRaiseAnything(),
                    step => Upon.Add(Jarryd).ItWill.Raise(Add.With(Jarryd)),
                    step => Upon.Add(Tim).ItWill.NotRaiseAnything(),
                    step => Upon.Add(Tom, Sally).ItWill.Raise(Add.With(Sally)),
                    step => Upon.Insert(2, Simon).ItWill.Raise(Add.With(Simon)),
                    step => Upon.Insert(3, Phil, Jake).ItWill.Raise(Add.With(Phil)).And.Raise(Add.With(Jake)),
                    step => Upon.Evaluate().ItWill.NotRaiseAnything()
                    )
                .Scenario("Removing items",
                    With.Inputs(Mike, Tom, Jack),
                    step => Upon.Evaluate().ItWill.NotRaiseAnything().And.HaveCount(2),
                    step => Upon.Remove(Mike).ItWill.Raise(Remove.WithOld(Mike)),
                    step => Upon.Remove(Tom).ItWill.NotRaiseAnything()
                    )
                .Verify();
        }
    }
}