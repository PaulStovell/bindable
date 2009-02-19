using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bindable.Linq.Tests.TestLanguage.Steps
{
    internal sealed class SimpleStep : Step
    {
        private Action<IScenario> _callback;
        private Expectations<SimpleStep> _itWill;

        public SimpleStep(Action<IScenario> callback)
        {
            _callback = callback;
            _itWill = new Expectations<SimpleStep>(this);
        }

        public Expectations<SimpleStep> And
        {
            get { return _itWill; }
        }

        public Expectations<SimpleStep> ItWill
        {
            get { return _itWill; }
        }

        protected override void ExecuteOverride()
        {
            _callback(this.Scenario);
        }
    }
}
