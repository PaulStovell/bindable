using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Bindable.Core.Helpers;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Interfaces.Events;
using Bindable.Core.Threading;
using Bindable.Core;

namespace Bindable.Linq.Adapters.Incoming
{
    /// <summary>
    /// Serves as a base class for adapters that convert collections to bindable collections.
    /// </summary>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    internal abstract class BindableCollectionAdapterBase<TElement> : DispatcherBound, IBindableCollection<TElement>
    {
        private readonly StateScope _collectionChangedSuspendedState = new StateScope();
        private readonly IEnumerable _sourceCollection;
        private bool _hasEvaluated;

        protected BindableCollectionAdapterBase(IEnumerable sourceCollection, IDispatcher dispatcher)
            : base(dispatcher)
        {
            Guard.NotNull(sourceCollection, "sourceCollection");
            _sourceCollection = sourceCollection;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when the collection is being evaluated (GetEnumerator() is called) for the first time, just before it returns
        /// the results, to provide insight into the items being evaluated. This allows consumers to iterate the items in a collection
        /// just before they are returned to the caller, while still enabling delayed execution of queries.
        /// </summary>
        public event EvaluatingEventHandler<TElement> Evaluating;

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Gets a value indicating whether this instance has already evaluated.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has evaluated; otherwise, <c>false</c>.
        /// </value>
        public bool HasEvaluated
        {
            get
            {
                return _hasEvaluated;
            }
            private set
            {
                AssertDispatcherThread();
                _hasEvaluated = value;
                OnPropertyChanged(CommonEventArgsCache.HasEvaluated);
            }
        }

        /// <summary>
        /// Evaluates this instance.
        /// </summary>
        public void Evaluate()
        {
            GetEnumerator();
        }

        /// <summary>
        /// Gets the state of the collection changed suspended.
        /// </summary>
        /// <value>The state of the collection changed suspended.</value>
        protected StateScope CollectionChangedSuspendedState
        {
            get { return _collectionChangedSuspendedState; }
        }

        /// <summary>
        /// Gets the <typeparamref name="TElement"/> at the specified index.
        /// </summary>
        /// <value></value>
        public TElement this[int index]
        {
            get
            {
                if (Dispatcher.DispatchRequired())
                {
                    return Dispatcher.Dispatch(() => this[index]);
                }

                var itemIndex = 0;
                foreach (var item in this)
                {
                    if (index == itemIndex)
                    {
                        return item;
                    }
                    itemIndex++;
                }
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Gets the count of items in the collection.
        /// </summary>
        /// <value></value>
        public int Count
        {
            get
            {
                if (Dispatcher.DispatchRequired())
                {
                    return Dispatcher.Dispatch(() => Count);
                }

                var result = 0;
                foreach (var item in this)
                {
                    result++;
                }
                return result;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<TElement> GetEnumerator()
        {
            if (Dispatcher.DispatchRequired())
            {
                return Dispatcher.Dispatch(() => GetEnumerator());
            }

            using (CollectionChangedSuspendedState.Enter())
            {
                var results = new List<TElement>();
                foreach (var element in _sourceCollection)
                {
                    if (element is TElement)
                    {
                        results.Add((TElement) element);
                    }
                    else if (element != null)
                    {
                        throw new InvalidCastException(string.Format("Could not cast object of type {0} to type {1}", element.GetType(), typeof (TElement)));
                    }
                }

                if (!HasEvaluated)
                {
                    HasEvaluated = true;
                    OnEvaluating(new EvaluatingEventArgs<TElement>(results));
                }
                return results.GetEnumerator();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Refreshes the object.
        /// </summary>
        public void Refresh()
        {
            if (Dispatcher.DispatchRequired())
            {
                Dispatcher.Dispatch(Refresh);
            }
            else
            {
                HasEvaluated = false;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        /// <summary>
        /// Sets a new dependency on a Bindable LINQ operation.
        /// </summary>
        /// <param name="definition">A definition of the dependency.</param>
        public void AcceptDependency(IDependencyDefinition definition)
        {
            Guard.NotNull(definition, "definition");
            throw new NotSupportedException("This object cannot accept dependencies directly.");
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            AssertDispatcherThread();
            var handler = PropertyChanged;
            if (handler != null && !CollectionChangedSuspendedState.IsWithin)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Evaluating"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EvaluatingEventArgs{TElement}"/> instance containing the event data.</param>
        protected virtual void OnEvaluating(EvaluatingEventArgs<TElement> e)
        {
            AssertDispatcherThread();
            var handler = Evaluating;
            if (handler != null)
                handler(this, e);
            OnPropertyChanged(CommonEventArgsCache.Count);
        }

        /// <summary>
        /// Raises the <see cref="CollectionChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            AssertDispatcherThread();
            var handler = CollectionChanged;
            if (handler != null && !CollectionChangedSuspendedState.IsWithin)
                handler(this, e);
            OnPropertyChanged(CommonEventArgsCache.Count);
        }
    }
}