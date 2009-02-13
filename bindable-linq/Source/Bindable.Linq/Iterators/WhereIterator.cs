using System;
using Bindable.Linq.Interfaces;
using Bindable.Core.Threading;

namespace Bindable.Linq.Iterators
{
    /// <summary>
    /// The Iterator created when a Where operation is performed.
    /// </summary>
    /// <typeparam name="TElement">The type of source item being filtered.</typeparam>
    internal sealed class WhereIterator<TElement> : Iterator<TElement, TElement>
        where TElement : class
    {
        private readonly Func<TElement, bool> _predicate;

        /// <summary>
        /// Initializes a new instance of the <see cref="WhereIterator&lt;TElement&gt;"/> class.
        /// </summary>
        /// <param name="sourceCollection">The source collection.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        public WhereIterator(IBindableCollection<TElement> sourceCollection, Func<TElement, bool> predicate, IDispatcher dispatcher)
            : base(sourceCollection, dispatcher)
        {
            _predicate = predicate;
        }

        /// <summary>
        /// When implemented in a derived class, processes all items in a given source collection.
        /// </summary>
        /// <remarks>Warning: No locks should be held when invoking this method.</remarks>
        protected override void EvaluateSourceCollection()
        {
            foreach (var item in SourceCollection)
                ReactToAdd(-1, item);
        }

        /// <summary>
        /// Filters an item from the source collection.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        /// <value>The predicate.</value>
        public bool Filter(TElement element)
        {
            return _predicate(element);
        }

        /// <summary>
        /// When overridden in a derived class, processes an Add event over a range of items.
        /// </summary>
        /// <param name="insertionIndex">Index of the insertion.</param>
        /// <param name="addedItem">The added item.</param>
        protected override void ReactToAdd(int insertionIndex, TElement addedItem)
        {
            if (Filter(addedItem))
            {
                ResultCollection.Insert(insertionIndex, addedItem);
            }
        }

        /// <summary>
        /// Reacts to move.
        /// </summary>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="newIndex">The new index.</param>
        /// <param name="movedItem">The moved item.</param>
        protected override void ReactToMove(int oldIndex, int newIndex, TElement movedItem)
        {
            if (Filter(movedItem))
            {
                ResultCollection.Move(newIndex, movedItem);    
            }
        }

        /// <summary>
        /// When overridden in a derived class, processes a Remove event over a range of items.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="removedItem">The removed item.</param>
        protected override void ReactToRemove(int index, TElement removedItem)
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
            if (Filter(newItem))
            {
                ResultCollection.Replace(oldItem, newItem);
            }
            else
            {
                ResultCollection.Remove(oldItem);
            }
        }

        /// <summary>
        /// When overridden in a derived class, processes a PropertyChanged event on a source item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected override void ReactToItemPropertyChanged(TElement item, string propertyName)
        {
            if (!Filter(item))
            {
                if (ResultCollection.Contains(item))
                {
                    ResultCollection.Remove(item);
                }
            }
            else
            {
                if (!ResultCollection.Contains(item))
                {
                    ResultCollection.Add(item);
                }
            }
        }
    }
}