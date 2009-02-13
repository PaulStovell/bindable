namespace Bindable.Linq.Operators
{
    /// <summary>
    /// Represents the result of a call to the Switch operator, providing an object that can be used to build up a switch statement.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    public interface ISwitchDeclaration<TInput>
    {
        /// <summary>
        /// Instantiates the switch statement for a given result type.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <returns>An instance of the switch statement for the given result type.</returns>
        ISwitch<TInput, TResult> CreateForResultType<TResult>();
    }
}