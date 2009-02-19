using System;
using System.Linq.Expressions;
using Bindable.Core.Helpers;

namespace Bindable.Linq.Operators
{
    /// <summary>
    /// Represents a case in a switch operation.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    internal sealed class SwitchCase<TInput, TResult> : ISwitchCase<TInput, TResult>
    {
        private readonly Expression<Func<TInput, bool>> _inputCondition;
        private readonly Expression<Func<TInput, TResult>> _resultExpression;
        private readonly Func<TInput, bool> _inputConditionCompiled;
        private readonly Func<TInput, TResult> _resultExpressionCompiled;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchCase&lt;TInput, TResult&gt;"/> class.
        /// </summary>
        /// <param name="inputCondition">The input condition.</param>
        /// <param name="resultExpression">The result expression.</param>
        public SwitchCase(Expression<Func<TInput, bool>> inputCondition, Expression<Func<TInput, TResult>> resultExpression)
        {
            Guard.NotNull(inputCondition, "inputCondition");
            Guard.NotNull(resultExpression, "resultExpression");
            _inputCondition = inputCondition;
            _resultExpression = resultExpression;
            _inputConditionCompiled = _inputCondition.Compile();
            _resultExpressionCompiled = _resultExpression.Compile();
        }

        /// <summary>
        /// Evaluates the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public bool Evaluate(TInput input)
        {
            return _inputConditionCompiled(input);
        }

        /// <summary>
        /// Returns the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public TResult Return(TInput input)
        {
            return _resultExpressionCompiled(input);
        }
    }
}
