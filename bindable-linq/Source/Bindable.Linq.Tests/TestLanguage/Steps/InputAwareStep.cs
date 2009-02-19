using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Bindable.Linq.Tests.TestLanguage.Steps
{    
    internal sealed class InputAwareStep<TInput> : Step
    {
        private readonly Action<ObservableCollection<TInput>> _action;
        private readonly Expectations<InputAwareStep<TInput>> _itWill;

        public InputAwareStep(Action<ObservableCollection<TInput>> action)
        {
            _action = action;
            _itWill = new Expectations<InputAwareStep<TInput>>(this);
        }

        public Expectations<InputAwareStep<TInput>> And
        {
            get { return _itWill; }
        }

        [DebuggerNonUserCode()]
        public Expectations<InputAwareStep<TInput>> ItWill
        {
            get { return _itWill; }
        }

        protected override void ExecuteOverride()
        {
            _action(((IScenario<TInput>) Scenario).Inputs);
        }
    }
}