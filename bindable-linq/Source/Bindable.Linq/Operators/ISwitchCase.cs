namespace Bindable.Linq.Operators
{
    /// <summary>
    /// Represents an instance of a case in a switch statement.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface ISwitchCase<TInput, TResult>
    {
        /// <summary>
        /// Evaluates the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        bool Evaluate(TInput input);

        /// <summary>
        /// Returns the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        TResult Return(TInput input);
    }
}