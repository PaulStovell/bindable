using System;
using System.Collections.Generic;
using Bindable.Core.Helpers;
using Bindable.Core.Threading;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq.Operators
{
    /// <summary>
    /// The Operator created when a Switch is set up.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    internal sealed class SwitchOperator<TSource, TResult> : Operator<TSource, TResult>, ISwitch<TSource, TResult>
    {
        private readonly List<ISwitchCase<TSource, TResult>> _conditionalCases = new List<ISwitchCase<TSource, TResult>>();
        private ISwitchCase<TSource, TResult> _defaultCase;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchOperator&lt;TSource, TResult&gt;"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        public SwitchOperator(IBindable<TSource> source, IDispatcher dispatcher)
            : base(source, dispatcher)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(dispatcher, "dispatcher");
        }

        /// <summary>
        /// Adds a case statement to the switch.
        /// </summary>
        /// <param name="customCase">The custom case.</param>
        /// <returns></returns>
        public ISwitch<TSource, TResult> AddCase(ISwitchCase<TSource, TResult> customCase)
        {
            Guard.NotNull(customCase, "customCase");
            _conditionalCases.Add(customCase);
            return this;
        }

        /// <summary>
        /// Adds the default case.
        /// </summary>
        /// <param name="defaultCase">The default case.</param>
        /// <returns></returns>
        public ISwitch<TSource, TResult> AddDefault(ISwitchCase<TSource, TResult> defaultCase)
        {
            if (_defaultCase != null)
            {
                _defaultCase = defaultCase;
            }
            else
            {
                throw new InvalidOperationException("A default state has already been defined for this switch statement.");
            }
            return this;
        }

        /// <summary>
        /// When overridden in a derived class, refreshes the operator.
        /// </summary>
        protected override void RefreshOverride()
        {
            var result = default(TResult);
            var source = Source.Current;

            var caseMatched = false;
            foreach (var conditionalCase in _conditionalCases)
            {
                if (conditionalCase.Evaluate(source))
                {
                    result = conditionalCase.Return(source);
                    caseMatched = true;
                    break;
                }
            }

            if (!caseMatched && _defaultCase != null)
            {
                result = _defaultCase.Return(source);
            }

            Current = result;
        }
    }
}
