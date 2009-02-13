using System;
using System.Collections.Specialized;
using Bindable.Core.Helpers;
using Bindable.Linq.Collections;
using Bindable.Linq.Interfaces;
using Bindable.Core.Threading;
using Bindable.Core;

namespace Bindable.Linq.Helpers
{
    /// <summary>
    /// Performs actions on the elements of a collection when they are added or removed. Ensures 
    /// the action is always performed at least once. 
    /// </summary>
    /// <remarks>
    /// This object uses a direct event reference rather than weak references on purpose. The lifetime of 
    /// the object should be coupled to the owning class. 
    /// </remarks>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    internal sealed class ElementActioner<TElement> : DispatcherBound
    {
        private readonly WeakEvent<NotifyCollectionChangedEventArgs> _collection_CollectionChangedHandler;
        private readonly Action<TElement> _addAction;
        private readonly IBindableCollection<TElement> _collection;
        private readonly BindableCollection<TElement> _copy;
        private readonly object _object = new object();
        private readonly Action<TElement> _removeAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementActioner&lt;TElement&gt;"/> class.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="addAction">The add action.</param>
        /// <param name="removeAction">The remove action.</param>
        public ElementActioner(IBindableCollection<TElement> collection, Action<TElement> addAction, Action<TElement> removeAction, IDispatcher dispatcher)
            : base(dispatcher)
        {
            _addAction = addAction;
            _removeAction = removeAction;
            _collection = collection;

            _copy = new BindableCollection<TElement>(dispatcher);
            _collection_CollectionChangedHandler = Weak.Event<NotifyCollectionChangedEventArgs>(Collection_CollectionChanged);
            _collection.CollectionChanged += _collection_CollectionChangedHandler.HandlerProxy.Handler;

            var internalBindableCollection = collection as IBindableCollection<TElement>;
            if (internalBindableCollection != null && !internalBindableCollection.HasEvaluated)
            {
                internalBindableCollection.Evaluating += (sender, e) =>
                {
                    foreach (var element in e.ItemsYieldedFromEvaluation)
                    {
                        HandleElement(NotifyCollectionChangedAction.Add, element);
                        _copy.Add(element);
                    }
                };
            }
            else
            {
                _collection.ForEach(element =>
                {
                    HandleElement(NotifyCollectionChangedAction.Add, element);
                    _copy.Add(element);
                });
            }
        }

        private void HandleElement(NotifyCollectionChangedAction action, TElement element)
        {
            if (action == NotifyCollectionChangedAction.Add)
            {
                _addAction(element);
            }
            else if (action == NotifyCollectionChangedAction.Remove)
            {
                _removeAction(element);
            }
        }

        /// <summary>
        /// Handles the CollectionChanged event of the Collection.
        /// </summary>
        private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (TElement element in e.NewItems)
                    {
                        HandleElement(NotifyCollectionChangedAction.Add, element);
                        _copy.Add(element);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (TElement element in e.OldItems)
                    {
                        HandleElement(NotifyCollectionChangedAction.Remove, element);
                        _copy.Remove(element);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (TElement element in e.OldItems)
                    {
                        HandleElement(NotifyCollectionChangedAction.Remove, element);
                        _copy.Remove(element);
                    }
                    foreach (TElement element in e.NewItems)
                    {
                        HandleElement(NotifyCollectionChangedAction.Add, element);
                        _copy.Add(element);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    HandleReset();
                    break;
            }
        }

        private void HandleReset()
        {
            _copy.ForEach(a => HandleElement(NotifyCollectionChangedAction.Remove, a));
            _collection.ForEach(a => HandleElement(NotifyCollectionChangedAction.Add, a));
            _copy.Clear();
            _copy.AddRange(_collection);
        }

        protected override void BeforeDisposeOverride()
        {
            if (_collection != null)
            {
                _copy.ForEach(e => HandleElement(NotifyCollectionChangedAction.Remove, e));
                _collection.CollectionChanged -= _collection_CollectionChangedHandler.HandlerProxy.Handler;
            }
            base.BeforeDisposeOverride();
        }
    }
}