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
    public class ConcatBehavior : TestFixture
    {
        [Test]
        public void ConcatSpecification()
        {
            var additionalContacts = With.Inputs(John, Tom, Jarryd);

            Specification.Title("Union() specification")
                .TestingOver<Contact>()
                .UsingBindableLinq(inputs => inputs.AsBindable().Concat(additionalContacts.AsBindable()))
                .UsingStandardLinq(inputs => inputs.Concat(additionalContacts))
                .WithCompatabilityLevel(CompatabilityLevel.FullyCompatibleExceptOrdering)
                .Scenario("Delayed evaluation",
                    With.Inputs(Mike, Tom, Jack),
                    step => Upon.Construction().ItWill.NotHaveEvaluated(),
                    step => Upon.Evaluate().ItWill.HaveCount(6)
                    )
                .Scenario("Adding items",
                    With.Inputs(Mike, Tom, Jack),
                    step => Upon.Add(Rick, Mick).ItWill.NotRaiseAnything(),
                    step => Upon.Evaluate().ItWill.NotRaiseAnything(),
                    step => Upon.Add(Fred).ItWill.Raise(Add.With(Fred).At(8)),
                    step => Upon.Add(Gordon, Sally).ItWill.Raise(Add.With(Gordon).At(9)).And.Raise(Add.With(Sally).At(10)),
                    step => Upon.Insert(2, Simon).ItWill.Raise(Add.With(Simon).At(11)),
                    step => Upon.Insert(3, Phil, Jake).ItWill.Raise(Add.With(Phil).At(12)).And.Raise(Add.With(Jake).At(13)),
                    step => Upon.Evaluate().ItWill.NotRaiseAnything(),
                    step => Upon.Add(Tom).ItWill.Raise(Add.With(Tom)),
                    step => Upon.Add(Jack).ItWill.Raise(Add.With(Jack))
                    )
                .Verify();
        }
    }
}