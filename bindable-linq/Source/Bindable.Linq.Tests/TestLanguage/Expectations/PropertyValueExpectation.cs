using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq.Tests.TestLanguage.Expectations
{
    internal class CollectionPropertyValueExpectation : IExpectation
    {
        private readonly Func<IBindableCollection, bool> _expectation;

        public CollectionPropertyValueExpectation(Func<IBindableCollection, bool> expectation)
        {
            _expectation = expectation;
        }

        public void Validate(IScenario scenario)
        {
            _expectation((IBindableCollection)scenario.BindableLinqQuery);
        }
    }
}
