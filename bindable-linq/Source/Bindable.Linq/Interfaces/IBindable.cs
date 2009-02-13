using System;
using System.ComponentModel;
using Bindable.Core.Threading;

namespace Bindable.Linq.Interfaces
{
    /// <summary>
    /// This interface is implemented by the results of any Bindable LINQ query which result in 
    /// single items, as opposed to collections. 
    /// </summary>
    public interface IBindable : INotifyPropertyChanged, IRefreshable, IDisposable
    {
        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>The current.</value>
        object Current { get; }

        /// <summary>
        /// Gets the dispatcher.
        /// </summary>
        /// <value>The dispatcher.</value>
        IDispatcher Dispatcher { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has evaluated.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has evaluated; otherwise, <c>false</c>.
        /// </value>
        bool HasEvaluated { get; }

        /// <summary>
        /// Evaluates this instance.
        /// </summary>
        void Evaluate();
    }

    /// <summary>
    /// This interface is implemented by the results of any Bindable LINQ query which result in 
    /// single items, as opposed to collections. 
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained within the instance.</typeparam>
    public interface IBindable<TValue> : IBindable
    {
        /// <summary>
        /// The resulting value. Rather than being returned directly, the value is housed 
        /// within the <see cref="IBindable{TValue}"/> container so that it can be updated when 
        /// the source it was created from changes. 
        /// </summary>
        TValue Current { get; }
    }
}