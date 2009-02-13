using System;
using System.Linq.Expressions;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq.Operators
{
    /// <summary>
    /// Represents the result of a call to the Switch operator, providing an object that can be used to build up a switch statement.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface ISwitch<TInput, TResult>
    {
        /// <summary>
        /// Adds a case statement to the switch.
        /// </summary>
        /// <param name="customCase">The custom case.</param>
        /// <returns></returns>
        ISwitch<TInput, TResult> AddCase(ISwitchCase<TInput, TResult> customCase);

        /// <summary>
        /// Adds the default case.
        /// </summary>
        /// <param name="defaultCase">The default case.</param>
        /// <returns></returns>
        ISwitch<TInput, TResult> AddDefault(ISwitchCase<TInput, TResult> defaultCase);
    }
}