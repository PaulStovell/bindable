using System.Collections;
using System.Collections.Generic;
using Bindable.Linq.Tests.TestLanguage.Expectations;
using Bindable.Linq.Tests.TestLanguage.Expectations;
using Bindable.Linq.Tests.TestLanguage.Steps;

namespace Bindable.Linq.Tests.TestLanguage
{
    /// <summary>
    /// Specifies which expectations are to be verified after performing the previous action.
    /// </summary>
    /// <typeparam name="TStep">The type of the step.</typeparam>
    internal class Expectations<TStep> 
        where TStep : Step
    {
        private readonly TStep _owningStep;

        /// <summary>
        /// Initializes a new instance of the <see cref="Expectations&lt;TStep&gt;"/> class.
        /// </summary>
        /// <param name="owningStep">The owning step.</param>
        public Expectations(TStep owningStep)
        {
            _owningStep = owningStep;
        }

        /// <summary>
        /// An event will be raised.
        /// </summary>
        /// <param name="expectedEvent">The expected event.</param>
        /// <returns>The original step.</returns>
        public TStep Raise(IExpectation expectedEvent)
        {
            expectedEvent = expectedEvent ?? new RaiseCollectionEventExpectation() { Action = null };
            _owningStep.AddExpectation(expectedEvent);
            return _owningStep;
        }

        /// <summary>
        /// No events should be raised.
        /// </summary>
        /// <returns>The original step.</returns>
        public TStep NotRaiseAnything()
        {
            return Raise(null);
        }

        /// <summary>
        /// The query will not have evaluated yet.
        /// </summary>
        /// <returns>The original step.</returns>
        public TStep NotHaveEvaluated()
        {
            _owningStep.AddExpectation(new CollectionPropertyValueExpectation(query => query.HasEvaluated == false));
            _owningStep.AddExpectation(new RaiseCollectionEventExpectation() { Action = null });
            return _owningStep;
        }

        /// <summary>
        /// The Count property will be of a specified value (the query will be evaluated if it 
        /// has not already been evaluated).
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>The original step.</returns>
        public TStep HaveCount(int count)
        {
            _owningStep.AddExpectation(new CountExpectation(count));
            return _owningStep;
        }

        /// <summary>
        /// The Current property will evaluate to the specified value (the query will be evaluated if it 
        /// has not already been evaluated).
        /// </summary>
        /// <param name="value">The expected value.</param>
        /// <returns>The original step.</returns>
        public TStep HaveCurrentValue(object value)
        {
            _owningStep.AddExpectation(new CurrentValueExpectation(value));
            return _owningStep;
        }
    }
}
