using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Bindable.Core;
using Bindable.Core.Helpers;
using Bindable.Core.Threading;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Interfaces.Events;
using Bindable.Linq.Iterators;

namespace Bindable.Linq.Collections
{
    /// <summary>
    /// Used in the <see cref="GroupByIterator{TKey,TSource,TElement}"/> as the result of a grouping.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    internal sealed class BindableGrouping<TKey, TElement> : DispatcherBound, IBindableGrouping<TKey, TElement>
    {
        private readonly IBindableCollection<TElement> _groupWhereQuery;
        private readonly TKey _key;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableGrouping&lt;TKey, TElement&gt;"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="groupWhereQuery">The group query.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        public BindableGrouping(TKey key, IBindableCollection<TElement> groupWhereQuery, IDispatcher dispatcher)
            : base(dispatcher)
        {
            Guard.NotNull(groupWhereQuery, "groupWhereQuery");
            _key = key;
            _groupWhereQuery = groupWhereQuery;
            _groupWhereQuery.Evaluating += (sender, e) => OnEvaluating(e);
            _groupWhereQuery.CollectionChanged += (sender, e) => OnCollectionChanged(e);
            _groupWhereQuery.PropertyChanged += (sender, e) => OnPropertyChanged(e);
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
            get { return true; }
        }

        public void Evaluate()
        {
            GetEnumerator();
        }

        /// <summary>
        /// Gets the key of the <see cref="T:System.Linq.IGrouping`2"/>.
        /// </summary>
        /// <value></value>
        /// <returns>The key of the <see cref="T:System.Linq.IGrouping`2"/>.</returns>
        public TKey Key
        {
            get { return _key; }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<TElement> GetEnumerator()
        {
            return _groupWhereQuery.GetEnumerator();
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
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return _groupWhereQuery.Count; }
        }

        /// <summary>
        /// Gets the <typeparamref name="TElement"/> at the specified index.
        /// </summary>
        /// <value></value>
        public TElement this[int index]
        {
            get { return _groupWhereQuery[index]; }
        }

        /// <summary>
        /// Refreshes the object.
        /// </summary>
        public void Refresh()
        {
            _groupWhereQuery.Refresh();
        }

        /// <summary>
        /// Sets a new dependency on a Bindable LINQ operation.
        /// </summary>
        /// <param name="definition">A definition of the dependency.</param>
        public void AcceptDependency(IDependencyDefinition definition)
        {
            Guard.NotNull(definition, "definition");
            throw new NotSupportedException("This object cannot accept dependencies directly");
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            AssertDispatcherThread();
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Evaluating"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EvaluatingEventArgs{TElement}"/> instance containing the event data.</param>
        private void OnEvaluating(EvaluatingEventArgs<TElement> e)
        {
            AssertDispatcherThread();
            var handler = Evaluating;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="CollectionChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            AssertDispatcherThread();
            var handler = CollectionChanged;
            if (handler != null)
                handler(this, e);
        }
    }
}