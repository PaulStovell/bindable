using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq.Tests.TestLanguage.Expectations;

namespace Bindable.Linq.Tests.TestLanguage.Steps
{
    internal abstract class Step
    {
        private List<IExpectation> _expectations;
        private IScenario _scenario;

        public Step()
        {
            _expectations = new List<IExpectation>();
        }

        public IScenario Scenario
        {
            get { return _scenario; }
            set { _scenario = value; }
        }

        public void AddExpectation(IExpectation expectation)
        {
            _expectations.Add(expectation);
        }

        protected abstract void ExecuteOverride();

        public void Execute()
        {
            ExecuteOverride();
            foreach (var expectation in _expectations)
            {
                expectation.Validate(Scenario);
            }
        }
    }
}
