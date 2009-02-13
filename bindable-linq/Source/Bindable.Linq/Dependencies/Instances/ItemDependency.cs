using System;
using System.Collections.Generic;
using System.ComponentModel;
using Bindable.Linq.Dependencies.PathNavigation;
using Bindable.Linq.Dependencies.PathNavigation.Tokens;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq.Dependencies.Instances
{
    /// <summary>
    /// Represents an property dependency applied over a collection of items.
    /// </summary>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    public sealed class ItemDependency<TElement> : IDependency
    {
        private readonly ElementActioner<TElement> _actioner;
        private readonly object _dependencyLock = new object();
        private readonly IPathNavigator _pathNavigator;
        private readonly string _propertyPath;
        private readonly Dictionary<TElement, IToken> _sourceElementObservers;
        private Action<object, string> _reevaluateElementCallback;
        private IBindableCollection<TElement> _sourceElements;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemDependency&lt;TElement&gt;"/> class.
        /// </summary>
        /// <param name="propertyPath">The property path.</param>
        /// <param name="sourceElements">The source elements.</param>
        /// <param name="pathNavigator">The path navigator.</param>
        public ItemDependency(string propertyPath, IBindableCollection<TElement> sourceElements, IPathNavigator pathNavigator)
        {
            _pathNavigator = pathNavigator;
            _sourceElementObservers = new Dictionary<TElement, IToken>();
            _propertyPath = propertyPath;
            _sourceElements = sourceElements;
            _actioner = new ElementActioner<TElement>(
                sourceElements, 
                AddItem, 
                RemoveItem,
                sourceElements.Dispatcher);
        }

        /// <summary>
        /// Gets the dependency lock.
        /// </summary>
        /// <value>The dependency lock.</value>
        private object DependencyLock
        {
            get { return _dependencyLock; }
        }

        #region IDependency Members
        /// <summary>
        /// Sets the callback action the dependency should invoke when the dependent object has a property that changes.
        /// </summary>
        /// <param name="action">The callback action to invoke.</param>
        public void SetReevaluateElementCallback(Action<object, string> action)
        {
            _reevaluateElementCallback = action;
        }

        /// <summary>
        /// Sets the callback action the dependency should invoke when the dependent object changes, signalling the
        /// whole collection should re-evaluate.
        /// </summary>
        /// <param name="action">The callback action to invoke.</param>
        public void SetReevaluateCallback(Action<object> action) {}

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _actioner.Dispose();
        }
        #endregion

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="addedItem">The added item.</param>
        private void AddItem(TElement addedItem)
        {
            lock (DependencyLock)
            {
                if (addedItem is INotifyPropertyChanged && !_sourceElementObservers.ContainsKey(addedItem))
                {
                    _sourceElementObservers[addedItem] = _pathNavigator.TraverseNext(addedItem, _propertyPath, Element_PropertyChanged);
                }
            }
        }

        /// <summary>
        /// Removes the item.
        /// </summary>
        /// <param name="removedItem">The removed item.</param>
        private void RemoveItem(TElement removedItem)
        {
            lock (DependencyLock)
            {
                if (_sourceElementObservers.ContainsKey(removedItem))
                {
                    var monitor = _sourceElementObservers[removedItem];
                    if (monitor != null)
                    {
                        monitor.Dispose();
                    }
                    _sourceElementObservers.Remove(removedItem);
                }
            }
        }

        /// <summary>
        /// Called when a property on an element changes.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="propertyPath">The property path.</param>
        private void Element_PropertyChanged(object element, string propertyPath)
        {
            var action = _reevaluateElementCallback;
            if (action != null)
            {
                action(element, propertyPath);
            }
        }
    }
}