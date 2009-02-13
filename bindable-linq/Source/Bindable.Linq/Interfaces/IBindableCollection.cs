using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Interfaces.Events;
using Bindable.Core.Threading;

namespace Bindable.Linq.Interfaces
{
    /// <summary>
    /// An interface implemented by all Bindable LINQ bindable collections.
    /// </summary>
    public interface IBindableCollection : IEnumerable, IRefreshable, INotifyCollectionChanged, INotifyPropertyChanged, IAcceptsDependencies, IDisposable
    {
        /// <summary>
        /// Gets the dispatcher.
        /// </summary>
        /// <value>The dispatcher.</value>
        IDispatcher Dispatcher { get; }

        /// <summary>
        /// Gets the count of items in the collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has already evaluated.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has evaluated; otherwise, <c>false</c>.
        /// </value>
        bool HasEvaluated { get; }

        /// <summary>
        /// Evaluates this instance.
        /// </summary>
        void Evaluate();
    }

    /// <summary>
    /// An interface implemented by all Bindable LINQ bindable collections.
    /// </summary>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    public interface IBindableCollection<TElement> : IEnumerable<TElement>, IBindableCollection
    {
        /// <summary>
        /// Gets the <typeparamref name="TElement"/> at the specified index.
        /// </summary>
        /// <value></value>
        TElement this[int index] { get; }

        /// <summary>
        /// Occurs when the collection is being evaluated (GetEnumerator() is called) for the first time, just before it returns 
        /// the results, to provide insight into the items being evaluated. This allows consumers to iterate the items in a collection 
        /// just before they are returned to the caller, while still enabling delayed execution of queries.
        /// </summary>
        event EvaluatingEventHandler<TElement> Evaluating;
    }
}