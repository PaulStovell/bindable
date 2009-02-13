using System;
using System.Linq.Expressions;

namespace Bindable.Linq.Operators
{
    /// <summary>
    /// Represents a default case in a switch statement.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    internal sealed class SwitchDefault<TInput, TResult> : ISwitchCase<TInput, TResult>
    {
        private readonly Expression<Func<TInput, TResult>> _resultExpression;
        private readonly Func<TInput, TResult> _resultExpressionCompiled;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchDefault&lt;TInput, TResult&gt;"/> class.
        /// </summary>
        /// <param name="resultExpression">The result expression.</param>
        public SwitchDefault(Expression<Func<TInput, TResult>> resultExpression)
        {
            _resultExpression = resultExpression;
            _resultExpressionCompiled = _resultExpression.Compile();
        }

        /// <summary>
        /// Evaluates the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public bool Evaluate(TInput input)
        {
            return true;
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
