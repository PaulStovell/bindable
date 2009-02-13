using System.Collections.Generic;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Core.Threading;

namespace Bindable.Linq.Iterators
{
    /// <summary>
    /// The Iterator created when performing a union between one or more collections.
    /// </summary>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    internal sealed class UnionIterator<TElement> : Iterator<IBindableCollection<TElement>, TElement> where TElement : class
    {
        private readonly ElementActioner<IBindableCollection<TElement>> _rootActioner;
        private readonly Dictionary<IBindableCollection<TElement>, ElementActioner<TElement>> _childCollectionActioners;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnionIterator&lt;TElement&gt;"/> class.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        public UnionIterator(IBindableCollection<IBindableCollection<TElement>> elements, IDispatcher dispatcher)
            : base(elements, dispatcher)
        {
            // A map of actioners for each of the collections being unioned
            _childCollectionActioners = new Dictionary<IBindableCollection<TElement>, ElementActioner<TElement>>();

            // An actioner that invokes the Added/Removed delegates each time a collection is added to the list of 
            // collections being unioned
            _rootActioner = new ElementActioner<IBindableCollection<TElement>>( 
                SourceCollection,
                ChildCollectionAdded,
                ChildCollectionRemoved,
                Dispatcher);
        }

        private void ChildCollectionRemoved(IBindableCollection<TElement> collection)
        {
        }

        private void ChildCollectionAdded(IBindableCollection<TElement> collection)
        {
            _childCollectionActioners[collection] = new ElementActioner<TElement>(
                collection,
                item => ResultCollection.Add(item),
                item => ResultCollection.Remove(item),
                Dispatcher);
        }

        /// <summary>
        /// When implemented in a derived class, processes all items in a given source collection.
        /// </summary>
        /// <remarks>Warning: No locks should be held when invoking this method.</remarks>
        protected override void EvaluateSourceCollection()
        {
            foreach (var sourceCollection in SourceCollection) sourceCollection.Evaluate();
        }

        /// <summary>
        /// When overridden in a derived class, processes a PropertyChanged event on a source item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected override void ReactToItemPropertyChanged(IBindableCollection<TElement> item, string propertyName)
        {

        }

        /// <summary>
        /// When overridden in a derived class, processes an Add event over a range of items.
        /// </summary>
        /// <param name="insertionIndex">Index of the insertion.</param>
        /// <param name="addedItem">The added item.</param>
        protected override void ReactToAdd(int insertionIndex, IBindableCollection<TElement> addedItem)
        {
        }

        /// <summary>
        /// When overridden in a derived class, processes a Replace event over a range of items.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="oldItem">The old item.</param>
        /// <param name="newItem">The new item.</param>
        protected override void ReactToReplace(int index, IBindableCollection<TElement> oldItem, IBindableCollection<TElement> newItem)
        {
        }

        /// <summary>
        /// When overridden in a derived class, processes a Move event over a range of items.
        /// </summary>
        /// <param name="originalIndex">Index of the original.</param>
        /// <param name="newIndex">The new index.</param>
        /// <param name="movedItem">The moved item.</param>
        protected override void ReactToMove(int originalIndex, int newIndex, IBindableCollection<TElement> movedItem)
        {
        }

        /// <summary>
        /// When overridden in a derived class, processes a Remove event over a range of items.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="removedItem">The removed item.</param>
        protected override void ReactToRemove(int index, IBindableCollection<TElement> removedItem)
        {
        }

        /// <summary>
        /// When overridden in a derived class, provides the derived class with the ability to perform custom actions when
        /// the collection is reset, before the sources are re-loaded.
        /// </summary>
        /// <remarks>Warning: No locks should be held when invoking this method.</remarks>
        protected override void BeforeResetOverride()
        {
            _childCollectionActioners.Clear();
        }
    }
}