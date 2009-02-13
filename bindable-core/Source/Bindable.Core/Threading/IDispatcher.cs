using System;

namespace Bindable.Core.Threading
{
    /// <summary>
    /// Provides a wrapper around the process of dispatching actions onto 
    /// different threads, primarily for unit testing.
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Dispatches the specified action to the thread.
        /// </summary>
        /// <param name="actionToInvoke">The action to invoke.</param>
        void Dispatch(Action actionToInvoke);

        /// <summary>
        /// Dispatches the specified action to the thread.
        /// </summary>
        /// <param name="actionToInvoke">The action to invoke.</param>
        TResult Dispatch<TResult>(Func<TResult> actionToInvoke);

        /// <summary>
        /// Checks whether the thread invoking the method .
        /// </summary>
        /// <returns></returns>
        bool DispatchRequired();
    }
}