using System.Diagnostics;
using Bindable.Linq.Tests.TestLanguage.Helpers;

namespace Bindable.Linq.Tests.TestLanguage.Steps
{
    internal sealed class ReplaceStep<TInput> : Step
    {
        private readonly TInput[] _originalItems;
        private TInput[] _replacementItems;
        private readonly Expectations<ReplaceStep<TInput>> _itWill;

        public ReplaceStep(TInput[] originals)
        {
            _originalItems = originals;
            _itWill = new Expectations<ReplaceStep<TInput>>(this);
        }

        public Expectations<ReplaceStep<TInput>> And
        {
            get { return _itWill; }
        }

        [DebuggerNonUserCode()]
        public Expectations<ReplaceStep<TInput>> ItWill
        {
            get { return _itWill; }
        }

        public ReplaceStep<TInput> With(params TInput[] items)
        {
            _replacementItems = items;
            return this;
        }

        protected override void ExecuteOverride()
        {
            ((IScenario<TInput>) Scenario).Inputs.ReplaceRange(_originalItems, _replacementItems);
        }
    }
}