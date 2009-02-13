using System;
using System.Collections.Generic;
using Bindable.Linq.Interfaces;
using Bindable.Core.Threading;

namespace Bindable.Linq.Iterators
{    
    /// <summary>
    /// The Iterator created when ordering a collection.
    /// </summary>
    /// <typeparam name="TElement">The source collection type.</typeparam>
    /// <typeparam name="TKey">The type of key used to determine which properties to sort by.</typeparam>
    internal sealed class OrderByIterator<TElement, TKey> : Iterator<TElement, TElement>, IOrderedBindableCollection<TElement>
        where TElement : class
    {
        private readonly ItemSorter<TElement, TKey> _itemSorter;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderByIterator&lt;S, K&gt;"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="itemSorter">The item sorter.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        public OrderByIterator(IBindableCollection<TElement> source, ItemSorter<TElement, TKey> itemSorter, IDispatcher dispatcher)
            : base(source, dispatcher)
        {
            _itemSorter = itemSorter;
        }

        /// <summary>
        /// When implemented in a derived class, processes all items in a given source collection.
        /// </summary>
        /// <remarks>Warning: No locks should be held when invoking this method.</remarks>
        protected override void EvaluateSourceCollection()
        {
            ResultCollection.AddRange(System.Linq.Enumerable.OrderBy(SourceCollection, _itemSorter._keySelector));
            //foreach (var item in SourceCollection)
            //    ReactToAdd(-1, item);
        }

        /// <summary>
        /// Compares the specified items.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns></returns>
        public int Compare(TElement lhs, TElement rhs)
        {
            return _itemSorter.Compare(lhs, rhs);
        }

        /// <summary>
        /// When overridden in a derived class, processes an Add event over a range of items.
        /// </summary>
        /// <param name="insertionIndex">Index of the insertion.</param>
        /// <param name="addedItem">The added item.</param>
        protected override void ReactToAdd(int insertionIndex, TElement addedItem)
        {
            ResultCollection.InsertOrdered(addedItem, Compare);
        }

        /// <summary>
        /// When overridden in a derived class, processes a Move event over a range of items.
        /// </summary>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="newIndex">The new index.</param>
        /// <param name="movedItem">The moved item.</param>
        protected override void ReactToMove(int oldIndex, int newIndex, TElement movedItem)
        {
            // Nothing to do here
        }

        /// <summary>
        /// When overridden in a derived class, processes a Remove event over a range of items.
        /// </summary>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="removedItem">The removed item.</param>
        protected override void ReactToRemove(int oldIndex, TElement removedItem)
        {
            ResultCollection.Remove(removedItem);
        }

        /// <summary>
        /// When overridden in a derived class, processes a Replace event over a range of items.
        /// </summary>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="oldItem">The old item.</param>
        /// <param name="newItem">The new item.</param>
        protected override void ReactToReplace(int oldIndex, TElement oldItem, TElement newItem)
        {
            ReactToRemove(oldIndex, oldItem);
            ReactToAdd(-1, newItem);
        }

        /// <summary>
        /// When overridden in a derived class, processes a PropertyChanged event on a source item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected override void ReactToItemPropertyChanged(TElement item, string propertyName)
        {
            ResultCollection.MoveOrdered(item, Compare);
        }

        /// <summary>
        /// Performs a subsequent ordering on the elements of an <see cref="IOrderedBindableCollection{TElement}"/>
        /// according to a key.
        /// </summary>
        /// <typeparam name="TNewKey">The type of the key.</typeparam>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="descending">if set to <c>true</c> [descending].</param>
        /// <returns></returns>
        public IOrderedBindableCollection<TElement> CreateOrderedIterator<TNewKey>(Func<TElement, TNewKey> keySelector, IComparer<TNewKey> comparer, bool descending)
        {
            return new OrderByIterator<TElement, TNewKey>(SourceCollection, new ItemSorter<TElement, TNewKey>(_itemSorter, keySelector, comparer, !descending), Dispatcher);
        }
    }
}