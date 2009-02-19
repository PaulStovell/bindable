using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using Bindable.Core.Helpers;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Interfaces.Events;
using Bindable.Core.Threading;
using Bindable.Core;

namespace Bindable.Linq.Collections
{    
    /// <summary>
    /// This class is used as the primary implementation of a bindable collection. Most of the Iterators
    /// in Bindable LINQ use this class eventually to store their bindable results, as it abstracts a lot of the 
    /// logic around adding, replacing, moving and removing collections of items and raising the correct 
    /// events. It is similar to the <see cref="ObservableCollection{T}"/> in most ways, but provides 
    /// additional functionality.
    /// </summary>
    /// <typeparam name="TElement">The type of item held within the collection.</typeparam>
    public class BindableCollection<TElement> : DispatcherBound, IBindableCollection<TElement>
    {
        private readonly IEqualityComparer<TElement> _comparer = ElementComparerFactory.Create<TElement>();
        private readonly List<TElement> _innerList = new List<TElement>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableCollection&lt;TElement&gt;"/> class.
        /// </summary>
        public BindableCollection(IDispatcher dispatcher)
            : base(dispatcher)
        {
            _innerList = new List<TElement>();
        }
        
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <remarks>Warning: No locks should be held when raising this event.</remarks>
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
        /// Gets the list used internally to store the items.
        /// </summary>
        private List<TElement> InnerList
        {
            get { return _innerList; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has property changed subscribers.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has property changed subscribers; otherwise, <c>false</c>.
        /// </value>
        internal bool HasPropertyChangedSubscribers
        {
            get { return PropertyChanged != null; }
        }

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

        void IBindableCollection.Evaluate()
        {
        }

        #region Add

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(TElement item)
        {
            AssertDispatcherThread();

            var index = ((IList) InnerList).Add(item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        /// <summary>
        /// Adds a range of items to the <see cref="BindableCollection{TElement}"/>.
        /// </summary>
        /// <param name="range">The items to add.</param>
        public void AddRange(IEnumerable<TElement> range)
        {
            AssertDispatcherThread();
            
            if (range != null)
            {
                foreach (var element in range)
                {
                    Add(element);
                }
            }
        }

        /// <summary>
        /// Adds or inserts a range of items at a given index (which may be negative, in which case 
        /// the items will be added (appended to the end).
        /// </summary>
        /// <param name="index">The index to add the items at.</param>
        /// <param name="items">The items to add.</param>
        public void AddOrInsertRange(int index, IEnumerable<TElement> items)
        {
            AssertDispatcherThread();

            if (index == -1)
            {
                AddRange(items);
            }
            else
            {
                InsertRange(index, items);
            }
        }

        #endregion

        #region Insert
        
        /// <summary>
        /// Inserts an item to the <see cref="BindableCollection{TElement}"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="BindableCollection{TElement}"/>.</param>
        public void Insert(int index, TElement item)
        {
            AssertDispatcherThread();

            if (index < 0 || index > InnerList.Count)
            {
                Add(item);
            }
            else
            {
                InnerList.Insert(index, item);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            }
        }

        /// <summary>
        /// Inserts a range of items into the <see cref="BindableCollection{TElement}"/>.
        /// </summary>
        /// <param name="index">The index to start inserting at.</param>
        /// <param name="range">The items to insert into the <see cref="BindableCollection{TElement}"/>.</param>
        public void InsertRange(int index, IEnumerable<TElement> range)
        {
            AssertDispatcherThread();
            
            if (range != null)
            {
                if (index < 0 || index > InnerList.Count)
                {
                    AddRange(range);
                }
                else
                {
                    var currentItemIndex = 0;
                    foreach (var currentItem in range)
                    {
                        var insertionIndex = index + currentItemIndex;
                        Insert(insertionIndex, currentItem);
                    }
                }
            }
        }

        /// <summary>
        /// Inserts an item so that it appears in order, using a given comparer.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="comparer">The comparer.</param>
        public void InsertOrdered(TElement element, Comparison<TElement> comparer)
        {
            AssertDispatcherThread();
            
            var inserted = false;
            for (var i = 0; i < InnerList.Count; i++)
            {
                var result = comparer(element, InnerList[i]);
                if (result <= 0)
                {
                    Insert(i, element);
                    inserted = true;
                    break;
                }
            }
            if (!inserted)
            {
                Add(element);
            }
        }
        #endregion

        #region Move
        /// <summary>
        /// Moves an item to a new location within the collection.
        /// </summary>
        /// <param name="newIndex">The new index.</param>
        /// <param name="item">The item to move.</param>
        /// <remarks>
        /// Here is an example of how this logic works:
        /// Index     Start        Step 1       Step 2
        /// --------------------------------------------
        /// 0:        Paul         Paul         Paul
        /// 1:        Chuck        Larry        Larry
        /// 2:        Larry        Timone       Timone
        /// 3:        Timone       Pumba        Pumba
        /// 4:        Pumba        Patrick      Chuck
        /// 5:        Patrick                   Patrick
        /// Operation: Move "Chuck" from ix=1 to ix=4
        /// 1) Remove "Chuck" - removedIndex = 1
        /// 2) Insert "Chuck" - newIndex = 4
        /// </remarks>
        public void Move(int newIndex, TElement item)
        {
            AssertDispatcherThread();

            var originalIndex = IndexOf(item);
            var desiredIndex = newIndex;

            // Remove it temporarily
            var removed = false;
            if (originalIndex >= 0)
            {
                InnerList.Remove(item);
                removed = true;
            }

            // Insert it into the correct spot
            if (desiredIndex >= InnerList.Count)
            {
                desiredIndex = ((IList)InnerList).Add(item);
            }
            else
            {
                InnerList.Insert(desiredIndex, item);
            }
            
            // Record the appropriate event
            if (removed)
            {
#if SILVERLIGHT
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, originalIndex));
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, desiredIndex));
#else
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, desiredIndex, originalIndex));
#endif
            }
            else
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, desiredIndex));
            }
        }

        /// <summary>
        /// Moves an item to the correct place if it is no longer in the right place.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="comparer">The comparer.</param>
        public void MoveOrdered(TElement element, Comparison<TElement> comparer)
        {
            AssertDispatcherThread();

            var originalIndex = IndexOf(element);
            if (originalIndex >= 0)
            {
                for (var i = 0; i < InnerList.Count; i++)
                {
                    var result = comparer(element, InnerList[i]);
                    if (result <= 0)
                    {
                        var itemAlreadyInOrder = true;
                        for (var j = i; j < originalIndex && j < InnerList.Count; j++)
                        {
                            if (comparer(element, InnerList[j]) > 0)
                            {
                                itemAlreadyInOrder = false;
                                break;
                            }
                        }

                        if (!itemAlreadyInOrder)
                        {
                            if (i != originalIndex)
                            {
                                Move(i, element);
                            }
                        }
                        break;
                    }
                }
            }
        }
        #endregion

        #region Replace
        /// <summary>
        /// Replaces a given item with another item.
        /// </summary>
        /// <param name="oldItem">The old item.</param>
        /// <param name="newItem">The new item.</param>
        public void Replace(TElement oldItem, TElement newItem)
        {
            AssertDispatcherThread();

            var oldElement = (oldItem != null) ? (TElement)oldItem : default(TElement);
            var newElement = (newItem != null) ? (TElement)newItem : default(TElement);

            if (oldItem != null && newItem != null)
            {
                var oldItemIndex = IndexOf(oldElement);
                if (oldItemIndex >= 0)
                {
                    InnerList[oldItemIndex] = newElement;
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newElement, oldElement, oldItemIndex));
                }
                else
                {
                    Add(newElement);
                }
            }
            else if (newItem == null && oldItem == null)
            {
            }
            else if (newItem == null)
            {
                Remove(oldElement);
            }
            else if (oldItem == null)
            {
                Add(newElement);
            }
        }
        #endregion

        #region Remove
        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="BindableCollection{TElement}"/>.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// true if <paramref name="element"/> was successfully removed from the <see cref="BindableCollection{TElement}"/>; otherwise, false. This method also returns false if <paramref name="element"/> is not found in the original <see cref="BindableCollection{TElement}"/>.
        /// </returns>
        public bool Remove(TElement element)
        {
            AssertDispatcherThread();

            var result = false;
            int oldIndex = IndexOf(element);
            if (oldIndex >= 0)
            {
                result = InnerList.Remove(element);
                if (result)
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, element, oldIndex));
                }
            }
            return result;
        }

        /// <summary>
        /// Removes the item at the specified index in the <see cref="BindableCollection{TElement}"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            AssertDispatcherThread();

            var item = InnerList[index];
            if (item != null)
            {
                Remove(item);
            }
        }
        #endregion

        #region Clear
        /// <summary>
        /// Removes all items from the <see cref="BindableCollection{TElement}"/>.
        /// </summary>
        public void Clear()
        {
            InnerList.Clear();
            OnCollectionChanged(CommonEventArgsCache.Reset);
        }
        #endregion

        #region GetEnumerator
        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="BindableCollection{TElement}"/>. The 
        /// enumerator returned is a special kind of enumerator that allows the collection to be 
        /// modified even while it is being enumerated.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator"/> that can be used to iterate through the <see cref="BindableCollection{TElement}"/> in a thread-safe way.
        /// </returns>
        public IEnumerator<TElement> GetEnumerator()
        {
            AssertDispatcherThread();
            return InnerList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="BindableCollection{TElement}"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the <see cref="BindableCollection{TElement}"/>.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region IBindableCollection<TElement> Members

        /// <summary>
        /// Gets the <typeparamref name="TElement"/> at the specified index.
        /// </summary>
        /// <value></value>
        public TElement this[int index]
        {
            get { return _innerList[index]; }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="BindableCollection{TElement}"/>.
        /// </summary>
        /// <value></value>
        /// <returns>The number of elements contained in the <see cref="BindableCollection{TElement}"/>.</returns>
        public int Count
        {
            get { return InnerList.Count; }
        }

        #endregion

        /// <summary>
        /// Determines whether the <see cref="BindableCollection{TElement}"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="BindableCollection{TElement}"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="BindableCollection{TElement}"/>; otherwise, false.
        /// </returns>
        public bool Contains(TElement item)
        {
            AssertDispatcherThread();
            
            return IndexOf(item) >= 0;
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="BindableCollection{TElement}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="BindableCollection{TElement}"/>.</param>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(TElement item)
        {
            AssertDispatcherThread();

            // List<T>.IndexOf(item) underneath uses object.Equals(). We want to use object.ReferenceEquals() so that 
            // overloaded Equals operations do not have an effect. 
            var index = -1;
            for (var i = 0; i < InnerList.Count; i++)
            {
                if (_comparer.Equals(item, InnerList[i]))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        void IRefreshable.Refresh()
        {
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "BindableCollection - Count: " + Count);
        }

        void IAcceptsDependencies.AcceptDependency(IDependencyDefinition definition)
        {
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
            if (handler != null)
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
            if (handler != null)
                handler(this, e);
            OnPropertyChanged(CommonEventArgsCache.Count);
        }
    }
}