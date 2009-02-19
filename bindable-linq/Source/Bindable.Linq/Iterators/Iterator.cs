using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using Bindable.Core;
using Bindable.Core.Helpers;
using Bindable.Core.Threading;
using Bindable.Linq.Collections;
using Bindable.Linq.Configuration;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Interfaces.Events;

namespace Bindable.Linq.Iterators
{
    /// <summary>
    /// Serves as a base class for all Bindable LINQ Iterator containers. Iterators are Bindable LINQ operations 
    /// which take one or more collections of items, and return a collection of items. This is in contrast
    /// with Aggregators, which take one or more collections but return a single result item. 
    /// </summary>
    /// <typeparam name="TSource">The type of source item used in the Iterator.</typeparam>
    /// <typeparam name="TResult">The type of result item provided by the Iterator.</typeparam>
    /// <remarks>
    /// <para>
    /// Whilst Bindable LINQ collections are read-only, we implement the <see cref="IList"/> interface because 
    /// it stops WPF producing a wrapper object around the result set, which results in better performance. 
    /// For more information, see:
    /// http://msdn2.microsoft.com/en-gb/library/aa970683.aspx#data_binding.
    /// </para>
    /// </remarks>
    public abstract class Iterator<TSource, TResult> : DispatcherBound, IBindableCollection<TResult>, IAcceptsDependencies
    {
        private readonly StateScope _collectionChangedSuspendedState = new StateScope();
        private readonly List<IDependency> _dependencies = new List<IDependency>();
        private readonly BindableCollection<TResult> _resultCollection;
        private readonly IBindableCollection<TSource> _sourceCollection;
        private bool _hasEvaluated;

        /// <summary>
        /// Initializes a new instance of the <see cref="Iterator&lt;TSource, TResult&gt;"/> class.
        /// </summary>
        protected Iterator(IBindableCollection<TSource> sourceCollection, IDispatcher dispatcher)
            : base(dispatcher)
        {
            _resultCollection = new BindableCollection<TResult>(dispatcher);
            _resultCollection.CollectionChanged += ((sender, e) => OnCollectionChanged(e));

            _sourceCollection = sourceCollection;
            _sourceCollection.CollectionChanged += Weak.Event<NotifyCollectionChangedEventArgs>((sender, e) => Dispatcher.Dispatch(() => ReactToCollectionChanged(e))).KeepAlive(InstanceLifetime).HandlerProxy.Handler;
        }

        /// <summary>
        /// Occurs when a property on this Iterator changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when the collection is being evaluated (GetEnumerator() is called) for the first time, just before it returns
        /// the results, to provide insight into the items being evaluated. This allows consumers to iterate the items in a collection
        /// just before they are returned to the caller, while still enabling delayed execution of queries.
        /// </summary>
        public event EvaluatingEventHandler<TResult> Evaluating;

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Gets a state scope that can be entered and left to indicate the Iterator should not raise
        /// CollectionChanged events.
        /// </summary>
        protected StateScope CollectionChangedSuspendedState
        {
            get { return _collectionChangedSuspendedState; }
        }

        /// <summary>
        /// The result collection exposed by the Iterator.
        /// </summary>
        protected BindableCollection<TResult> ResultCollection
        {
            get { return _resultCollection; }
        }

        /// <summary>
        /// Gets the source collection.
        /// </summary>
        public IBindableCollection<TSource> SourceCollection
        {
            get { return _sourceCollection; }
        }

        /// <summary>
        /// Gets the <typeparamref name="TResult"/> at the specified index.
        /// </summary>
        /// <value></value>
        public TResult this[int index]
        {
            get
            {
                AssertDispatcherThread();
                Evaluate();
                return ResultCollection[index];
            }
        }

        /// <summary>
        /// Gets a count of the number of items in this Iterator.
        /// </summary>
        public int Count
        {
            get
            {
                AssertDispatcherThread();
                Evaluate();
                return ResultCollection.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has already evaluated.
        /// </summary>
        public bool HasEvaluated
        {
            get { return _hasEvaluated; }
            private set
            {
                _hasEvaluated = value;
                OnPropertyChanged(CommonEventArgsCache.HasEvaluated);
            }
        }

        /// <summary>
        /// Evaluates this instance.
        /// </summary>
        public void Evaluate()
        {
            AssertDispatcherThread();

            if (HasEvaluated == false)
            {
                Seal();
                EvaluateSourceCollection();
                HasEvaluated = true;
                OnEvaluating(new EvaluatingEventArgs<TResult>(ResultCollection.EnumerateSafely()));
            }
        }

        /// <summary>
        /// Refreshes this query by telling sources to refresh. The sources should tell their sources to refresh, until eventually, someone repopulates itself and raises a Reset event.
        /// </summary>
        public void Refresh()
        {
            AssertDispatcherThread();
            if (HasEvaluated)
            {
                SourceCollection.Refresh();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<TResult> GetEnumerator()
        {
            AssertDispatcherThread();
            using (CollectionChangedSuspendedState.Enter())
            {
                Evaluate();
            }
            return ResultCollection.GetEnumerator();
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
        /// Accepts a dependency.
        /// </summary>
        /// <param name="definition">The definition.</param>
        public void AcceptDependency(IDependencyDefinition definition)
        {
            AssertDispatcherThread();
            AssertUnsealed();

            if (!definition.AppliesToCollections())
            {
                return;
            }
            
            var dependency = definition.ConstructForCollection(SourceCollection, BindingConfigurations.Default.CreatePathNavigator());
            dependency.SetReevaluateElementCallback(((element, propertyName) => ReactToItemPropertyChanged((TSource) element, propertyName)));
            dependency.SetReevaluateCallback((element => ReactToReset()));
            _dependencies.Add(dependency);
        }
        
        private static void ValidateCollectionChangedEventArgs(NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 1)
            {
                throw new NotSupportedException();
            }
            if (e.OldItems != null && e.OldItems.Count > 1)
            {
                throw new NotSupportedException();
            }
            if (e.Action == NotifyCollectionChangedAction.Replace && e.NewItems.Count != e.OldItems.Count)
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Resets the result collection of the Iterator and re-reads all of the source collections.
        /// </summary>
        protected void ReactToReset()
        {
            BeforeResetOverride();
            ResultCollection.Clear();
            HasEvaluated = false;
            Evaluate();
        }

        private void ReactToCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            AssertDispatcherThread();
            
            if (!HasEvaluated) return;
            
            ValidateCollectionChangedEventArgs(e);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems.Count == 1)
                    {
                        ReactToAdd(e.NewStartingIndex, (TSource)e.NewItems[0]);
                    }
                    break;
#if !SILVERLIGHT
                case NotifyCollectionChangedAction.Move:
                    if (e.OldItems.Count == 1)
                    {
                        ReactToMove(e.OldStartingIndex, e.NewStartingIndex, (TSource)e.OldItems[0]);
                    }
                    break;
#endif              
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems.Count == 1)
                    {
                        ReactToRemove(e.OldStartingIndex, (TSource)e.OldItems[0]);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.NewItems.Count == 1 && e.OldItems.Count == 1)
                    {
                        ReactToReplace(e.OldStartingIndex, (TSource)e.OldItems[0], (TSource)e.NewItems[0]);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ReactToReset();
                    break;
            }
        }

        /// <summary>
        /// When implemented in a derived class, processes all items in a given source collection.
        /// </summary>
        /// <remarks>Warning: No locks should be held when invoking this method.</remarks>
        protected abstract void EvaluateSourceCollection();

        /// <summary>
        /// When overridden in a derived class, processes a PropertyChanged event on a source item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected abstract void ReactToItemPropertyChanged(TSource item, string propertyName);

        /// <summary>
        /// When overridden in a derived class, processes an Add event over a range of items.
        /// </summary>
        /// <param name="insertionIndex">Index of the insertion.</param>
        /// <param name="addedItem">The added item.</param>
        protected abstract void ReactToAdd(int insertionIndex, TSource addedItem);

        /// <summary>
        /// When overridden in a derived class, processes a Replace event over a range of items.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="oldItem">The old item.</param>
        /// <param name="newItem">The new item.</param>
        protected abstract void ReactToReplace(int index, TSource oldItem, TSource newItem);

        /// <summary>
        /// When overridden in a derived class, processes a Move event over a range of items.
        /// </summary>
        /// <param name="originalIndex">Index of the original.</param>
        /// <param name="newIndex">The new index.</param>
        /// <param name="movedItem">The moved item.</param>
        protected abstract void ReactToMove(int originalIndex, int newIndex, TSource movedItem);

        /// <summary>
        /// When overridden in a derived class, processes a Remove event over a range of items.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="removedItem">The removed item.</param>
        protected abstract void ReactToRemove(int index, TSource removedItem);

        /// <summary>
        /// When overridden in a derived class, provides the derived class with the ability to perform custom actions when 
        /// the collection is reset, before the sources are re-loaded.
        /// </summary>
        /// <remarks>Warning: No locks should be held when invoking this method.</remarks>
        protected virtual void BeforeResetOverride() {}

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <remarks>Warning: No locks should be held when invoking this method.</remarks>
        public override string ToString()
        {
            var result = string.Format(CultureInfo.InvariantCulture, "{0}<{1},{2}>", GetType().Name, typeof (TSource).Name, typeof (TResult).Name);
            if (HasEvaluated)
            {
                return result + string.Format(" - Evaluated - Count: {0}", Count);
            }
            return result + " - Not Evaluated";
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
        protected virtual void OnEvaluating(EvaluatingEventArgs<TResult> e)
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
            if (handler != null && !CollectionChangedSuspendedState.IsWithin && HasEvaluated) 
                handler(this, e);
            OnPropertyChanged(CommonEventArgsCache.Count);
        }

        /// <summary>
        /// When overridden in a derived class, gives the class an opportunity to dispose any expensive components.
        /// </summary>
        protected override void BeforeDisposeOverride()
        {
            foreach (var dependency in _dependencies)
            {
                dependency.Dispose();
            }
        }

        public object AddNew()
        {
            
        }

        public void CommitNew()
        {
            throw new System.NotImplementedException();
        }

        public void CancelNew()
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(object item)
        {
            throw new System.NotImplementedException();
        }

        public void EditItem(object item)
        {
            throw new System.NotImplementedException();
        }

        public void CommitEdit()
        {
            throw new System.NotImplementedException();
        }

        public void CancelEdit()
        {
            throw new System.NotImplementedException();
        }

        public NewItemPlaceholderPosition NewItemPlaceholderPosition
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool CanAddNew
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool IsAddingNew
        {
            get { throw new System.NotImplementedException(); }
        }

        public object CurrentAddItem
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanRemove
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanCancelEdit
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool IsEditingItem
        {
            get { throw new System.NotImplementedException(); }
        }

        public object CurrentEditItem
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}