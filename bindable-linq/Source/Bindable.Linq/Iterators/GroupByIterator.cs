using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using Bindable.Linq.Collections;
using Bindable.Linq.Interfaces;
using Bindable.Core.Threading;

namespace Bindable.Linq.Iterators
{
    /// <summary>
    /// An Iterator that reads items from the source collection and groups them by a common key. 
    /// </summary>
    internal sealed class GroupByIterator<TKey, TSource, TElement> : Iterator<TSource, IBindableGrouping<TKey, TElement>>
        where TSource : class
    {
        private readonly Expression<Func<TSource, TElement>> _elementSelector;
        private readonly IEqualityComparer<TKey> _keyComparer;
        private readonly Expression<Func<TSource, TKey>> _keySelector;
        private readonly Func<TSource, TKey> _keySelectorCompiled;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupByIterator&lt;TKey, TSource, TElement&gt;"/> class.
        /// </summary>
        /// <param name="sourceCollection">The source collection.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="elementSelector">The element selector.</param>
        /// <param name="keyComparer">The key comparer.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        public GroupByIterator(IBindableCollection<TSource> sourceCollection, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> keyComparer, IDispatcher dispatcher)
            : base(sourceCollection, dispatcher)
        {
            _keySelector = keySelector;
            _keySelectorCompiled = keySelector.Compile();
            _elementSelector = elementSelector;
            _keyComparer = keyComparer;
        }

        /// <summary>
        /// When implemented in a derived class, processes all items in a given source collection.
        /// </summary>
        /// <remarks>Warning: No locks should be held when invoking this method.</remarks>
        protected override void EvaluateSourceCollection()
        {
            foreach (var item in SourceCollection)
            {
                ReactToAdd(-1, item);
            }
        }

        /// <summary>
        /// Extracts a key from a given source item.
        /// </summary>
        /// <param name="sourceItem">The source item.</param>
        /// <returns></returns>
        public TKey KeySelector(TSource sourceItem)
        {
            return _keySelectorCompiled(sourceItem);
        }

        /// <summary>
        /// Compares two keys.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        private bool CompareKeys(TKey lhs, TKey rhs)
        {
            return _keyComparer.Equals(lhs, rhs);
        }

        private bool FindGroup(TKey key, IEnumerable<IBindableGrouping<TKey, TElement>> groups)
        {
            foreach (IBindableGrouping<TKey, TSource> existingGroup in groups)
            {
                if (CompareKeys(existingGroup.Key, key))
                {
                    return true;
                }
            }
            return false;
        }

        private void EnsureGroupsExists(TSource element)
        {
            var key = _keySelectorCompiled(element);
            var groupExists = FindGroup(key, ResultCollection);
            if (!groupExists)
            {
                IBindableGrouping<TKey, TElement> newGroup = new BindableGrouping<TKey, TElement>(key, SourceCollection.Where(e => CompareKeys(_keySelectorCompiled(e), key)).DependsOnExpression(_keySelector.Body, _keySelector.Parameters[0]).Select(_elementSelector), Dispatcher);
                newGroup.CollectionChanged += Group_CollectionChanged;
                ResultCollection.Add(newGroup);
            }
        }

        /// <summary>
        /// When overridden in a derived class, processes an Add event over a range of items.
        /// </summary>
        /// <param name="sourceStartingIndex">Index of the source starting.</param>
        /// <param name="addedItem">The added item.</param>
        protected override void ReactToAdd(int sourceStartingIndex, TSource addedItem)
        {
            EnsureGroupsExists(addedItem);
        }

        /// <summary>
        /// When overridden in a derived class, processes a Move event over a range of items.
        /// </summary>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="newIndex">The new index.</param>
        /// <param name="movedItem">The moved item.</param>
        protected override void ReactToMove(int oldIndex, int newIndex, TSource movedItem)
        {
            // Nothing to do here
        }

        /// <summary>
        /// When overridden in a derived class, processes a Remove event over a range of items.
        /// </summary>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="removedItem">The removed item.</param>
        protected override void ReactToRemove(int oldIndex, TSource removedItem)
        {
            // Nothing to do here
        }

        /// <summary>
        /// When overridden in a derived class, processes a Replace event over a range of items.
        /// </summary>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="oldItem">The old item.</param>
        /// <param name="newItem">The new item.</param>
        protected override void ReactToReplace(int oldIndex, TSource oldItem, TSource newItem)
        {
            EnsureGroupsExists(newItem);
        }

        /// <summary>
        /// When overridden in a derived class, processes a PropertyChanged event on a source item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected override void ReactToItemPropertyChanged(TSource item, string propertyName)
        {
            if (SourceCollection.Contains(item).Current)
            {
                EnsureGroupsExists(item);
            }
        }

        private void Group_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var group = sender as IBindableGrouping<TKey, TElement>;
                if (group != null && group.Count == 0)
                {
                    ResultCollection.Remove(group);
                }
            }
        }
    }
}