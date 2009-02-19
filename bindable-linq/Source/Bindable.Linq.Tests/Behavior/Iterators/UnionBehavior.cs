using System.Linq;
using Bindable.Linq;
using Bindable.Linq.Tests.MockObjectModel;
using Bindable.Linq.Tests.TestLanguage;
using Bindable.Linq.Tests.TestLanguage.Helpers;
using MbUnit.Framework;

namespace Bindable.Linq.Tests.Behavior.Iterators
{
    /// <summary>
    /// Contains high-level behavioral tests for the Union iterator. 
    /// </summary>
    [TestFixture]
    public class UnionBehavior : TestFixture
    {
        [Test]
        public void UnionSpecification()
        {
            var additionalContacts = With.Inputs(John, Tom, Jarryd);

            Specification.Title("Union() specification")
                .TestingOver<Contact>()
                .UsingBindableLinq(inputs => inputs.AsBindable().Union(additionalContacts.AsBindable()))
                .UsingStandardLinq(inputs => inputs.Union(additionalContacts))
                .WithCompatabilityLevel(CompatabilityLevel.FullyCompatibleExceptOrdering)
                .Scenario("Delayed evaluation",
                    With.Inputs(Mike, Tom, Jack),
                    step => Upon.Construction().ItWill.NotHaveEvaluated(),
                    step => Upon.Evaluate().ItWill.HaveCount(5)
                    )
                .Scenario("Adding items",
                    With.Inputs(Mike, Tom, Jack),
                    step => Upon.Add(Rick, Mick).ItWill.NotRaiseAnything(),
                    step => Upon.Evaluate().ItWill.NotRaiseAnything(),
                    step => Upon.Add(Fred).ItWill.Raise(Add.With(Fred).At(7)),
                    step => Upon.Add(Gordon, Sally).ItWill.Raise(Add.With(Gordon).At(8)).And.Raise(Add.With(Sally).At(9)),
                    step => Upon.Insert(2, Simon).ItWill.Raise(Add.With(Simon).At(10)),
                    step => Upon.Insert(3, Phil, Jake).ItWill.Raise(Add.With(Phil).At(11)).And.Raise(Add.With(Jake).At(12)),
                    step => Upon.Evaluate().ItWill.NotRaiseAnything(),
                    step => Upon.Add(Tom).ItWill.NotRaiseAnything(),
                    step => Upon.Add(Jack).ItWill.NotRaiseAnything()
                    )
                .Verify();
        }
    }
}