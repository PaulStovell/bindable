using System;
using Bindable.Linq.Interfaces;
using Bindable.Core.Threading;

namespace Bindable.Linq.Iterators
{
    /// <summary>
    /// The Iterator created when performing a select and projection into another type.
    /// </summary>
    /// <typeparam name="TSource">The type of source item.</typeparam>
    /// <typeparam name="TResult">The type of result item.</typeparam>
    internal sealed class SelectIterator<TSource, TResult> : Iterator<TSource, TResult>
        where TSource : class
    {
        private readonly ProjectionRegister<TSource, TResult> _projectionRegister;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectIterator&lt;T, R&gt;"/> class.
        /// </summary>
        /// <param name="sourceCollection">The source collection.</param>
        /// <param name="projector">The projector.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        public SelectIterator(IBindableCollection<TSource> sourceCollection, Func<TSource, TResult> projector, IDispatcher dispatcher)
            : base(sourceCollection, dispatcher)
        {
            _projectionRegister = new ProjectionRegister<TSource, TResult>(projector);
        }

        /// <summary>
        /// Gets the projection register. This register keeps track of projections from a source type 
        /// to a result type.
        /// </summary>
        /// <value>The projection register.</value>
        public ProjectionRegister<TSource, TResult> ProjectionRegister
        {
            get { return _projectionRegister; }
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
        /// When overridden in a derived class, processes an Add event over a range of items.
        /// </summary>
        /// <param name="insertionIndex">Index of the insertion.</param>
        /// <param name="addedItem">The added item.</param>
        protected override void ReactToAdd(int insertionIndex, TSource addedItem)
        {
            var projectedItem = ProjectionRegister.Project(addedItem);
            ResultCollection.Insert(insertionIndex, projectedItem);
        }

        /// <summary>
        /// When overridden in a derived class, processes a Move event over a range of items.
        /// </summary>
        /// <param name="originalIndex">Index of the original.</param>
        /// <param name="newIndex">The new index.</param>
        /// <param name="movedItem">The moved item.</param>
        protected override void ReactToMove(int originalIndex, int newIndex, TSource movedItem)
        {
            ResultCollection.Move(newIndex, ProjectionRegister.Project(movedItem));
        }

        protected override void ReactToRemove(int index, TSource removedItem)
        {
            var projection = ProjectionRegister.GetExistingProjection(removedItem);
            if (projection != null)
            {
                ResultCollection.Remove((TResult)projection);
                ProjectionRegister.Remove(removedItem);
            }
        }

        /// <summary>
        /// When overridden in a derived class, processes a Replace event over a range of items.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="oldItem">The old item.</param>
        /// <param name="newItem">The new item.</param>
        protected override void ReactToReplace(int index, TSource oldItem, TSource newItem)
        {
            ResultCollection.Replace((TResult)ProjectionRegister.GetExistingProjection(oldItem), ProjectionRegister.Project(newItem));
            ProjectionRegister.Remove(oldItem);
        }

        /// <summary>
        /// When overridden in a derived class, processes a PropertyChanged event on a source item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected override void ReactToItemPropertyChanged(TSource item, string propertyName)
        {
            var existing = ProjectionRegister.GetExistingProjection(item);
            if (existing is TResult)
            {
                ResultCollection.Replace((TResult) existing, ProjectionRegister.ReProject(item));
            }
        }

        /// <summary>
        /// When overridden in a derived class, provides the derived class with the ability to perform custom actions when
        /// the collection is reset, before the sources are re-loaded.
        /// </summary>
        /// <remarks>Warning: No locks should be held when invoking this method.</remarks>
        protected override void BeforeResetOverride()
        {
            ProjectionRegister.Clear();
            base.BeforeResetOverride();
        }

        /// <summary>
        /// When overridden in a derived class, gives the class an opportunity to dispose any expensive components.
        /// </summary>
        protected override void BeforeDisposeOverride()
        {
            ProjectionRegister.Dispose();
            base.BeforeDisposeOverride();
        }
    }
}