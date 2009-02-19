using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Bindable.Core.Helpers;
using Bindable.Linq.Dependencies.Observers;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Core.Threading;
using Bindable.Core;

namespace Bindable.Linq.Adapters.Outgoing
{
#if !SILVERLIGHT
    /// <summary>
    /// Converts Bindable LINQ bindable collection result sets into IBindingList implementations compatible with 
    /// Windows Forms.
    /// </summary>
    internal sealed class BindingListAdapter<TElement> : DispatcherBound, IBindingList, IDisposable
        where TElement : class
    {
        private readonly EventHandler<NotifyCollectionChangedEventArgs> _eventHandler;
        private readonly IBindableCollection<TElement> _originalSource;
        private readonly Dictionary<string, PropertyDescriptor> _propertyDescriptors;
        private readonly WeakEventProxy<NotifyCollectionChangedEventArgs> _weakHandler;
        private ElementActioner<TElement> _addActioner;
        private PropertyChangeObserver _propertyChangeObserver;

        private ListSortDirection _sortDirection;
        private PropertyDescriptor _sortProperty;
        private IBindableCollection<TElement> _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingListAdapter&lt;TElement&gt;"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        public BindingListAdapter(IBindableCollection<TElement> source, IDispatcher dispatcher)
            : base(dispatcher)
        {
            Guard.NotNull(source, "source");
            
            _originalSource = source;

            _eventHandler = Source_CollectionChanged;
            _weakHandler = new WeakEventProxy<NotifyCollectionChangedEventArgs>(_eventHandler);
            _sortDirection = ListSortDirection.Ascending;

            WireInterceptor(_originalSource);

            _propertyDescriptors = new Dictionary<string, PropertyDescriptor>();
            var properties = TypeDescriptor.GetProperties(typeof (TElement));
            foreach (PropertyDescriptor descriptor in properties)
            {
                if (descriptor != null && descriptor.Name != null)
                {
                    _propertyDescriptors[descriptor.Name] = descriptor;
                }
            }
        }

        #region IBindingList Members
        /// <summary>
        /// Occurs when the list changes or an item in the list changes.
        /// </summary>
        public event ListChangedEventHandler ListChanged;

        /// <summary>
        /// Adds the <see cref="T:System.ComponentModel.PropertyDescriptor"/> to the indexes used for searching.
        /// </summary>
        /// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor"/> to add to the indexes used for searching.</param>
        public void AddIndex(PropertyDescriptor property) {}

        /// <summary>
        /// Adds a new item to the list.
        /// </summary>
        /// <returns>The item added to the list.</returns>
        /// <exception cref="T:System.NotSupportedException">
        /// 	<see cref="P:System.ComponentModel.IBindingList.AllowNew"/> is false. </exception>
        public object AddNew()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets whether you can update items in the list.
        /// </summary>
        /// <value></value>
        /// <returns>true if you can update the items in the list; otherwise, false.</returns>
        public bool AllowEdit
        {
            get { return false; }
        }

        /// <summary>
        /// Gets whether you can add items to the list using <see cref="M:System.ComponentModel.IBindingList.AddNew"/>.
        /// </summary>
        /// <value></value>
        /// <returns>true if you can add items to the list using <see cref="M:System.ComponentModel.IBindingList.AddNew"/>; otherwise, false.</returns>
        public bool AllowNew
        {
            get { return false; }
        }

        /// <summary>
        /// Gets whether you can remove items from the list, using <see cref="M:System.Collections.IList.Remove(System.Object)"/> or <see cref="M:System.Collections.IList.RemoveAt(System.Int32)"/>.
        /// </summary>
        /// <value></value>
        /// <returns>true if you can remove items from the list; otherwise, false.</returns>
        public bool AllowRemove
        {
            get { return false; }
        }

        /// <summary>
        /// Sorts the list based on a <see cref="T:System.ComponentModel.PropertyDescriptor"/> and a <see cref="T:System.ComponentModel.ListSortDirection"/>.
        /// </summary>
        /// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor"/> to sort by.</param>
        /// <param name="direction">One of the <see cref="T:System.ComponentModel.ListSortDirection"/> values.</param>
        /// <exception cref="T:System.NotSupportedException">
        /// 	<see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
        public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            if (property == _sortProperty && direction == _sortDirection) return;

            if (IsSorted) UnwireInterceptor();

            _sortProperty = property;
            _sortDirection = direction;

            Expression<Func<TElement, object>> selector = item => KeySelector(item);

            var q = ListSortDirection.Ascending == _sortDirection ? _originalSource.OrderBy(selector) : _originalSource.OrderByDescending(selector);

            WireInterceptor(q.DependsOn(_sortProperty.Name));

            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        /// <summary>
        /// Returns the index of the row that has the given <see cref="T:System.ComponentModel.PropertyDescriptor"/>.
        /// </summary>
        /// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor"/> to search on.</param>
        /// <param name="key">The value of the <paramref name="property"/> parameter to search for.</param>
        /// <returns>
        /// The index of the row that has the given <see cref="T:System.ComponentModel.PropertyDescriptor"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// 	<see cref="P:System.ComponentModel.IBindingList.SupportsSearching"/> is false. </exception>
        public int Find(PropertyDescriptor property, object key)
        {
            var query = _source;
            if (query == null) throw new NotSupportedException();

            for (var index = 0; index < query.Count; index++)
            {
                var item = query[0];
                if (null == item) continue;
                if (property.GetValue(item) == key) return index;
            }
            return -1;
        }

        /// <summary>
        /// Gets whether the items in the list are sorted.
        /// </summary>
        /// <value></value>
        /// <returns>true if <see cref="M:System.ComponentModel.IBindingList.ApplySort(System.ComponentModel.PropertyDescriptor,System.ComponentModel.ListSortDirection)"/> has been called and <see cref="M:System.ComponentModel.IBindingList.RemoveSort"/> has not been called; otherwise, false.</returns>
        /// <exception cref="T:System.NotSupportedException">
        /// 	<see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
        public bool IsSorted
        {
            get { return _source != _originalSource; }
        }

        /// <summary>
        /// Removes the <see cref="T:System.ComponentModel.PropertyDescriptor"/> from the indexes used for searching.
        /// </summary>
        /// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor"/> to remove from the indexes used for searching.</param>
        public void RemoveIndex(PropertyDescriptor property) {}

        /// <summary>
        /// Removes any sort applied using <see cref="M:System.ComponentModel.IBindingList.ApplySort(System.ComponentModel.PropertyDescriptor,System.ComponentModel.ListSortDirection)"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">
        /// 	<see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
        public void RemoveSort()
        {
            if (!IsSorted) return;

            UnwireInterceptor();

            _sortDirection = ListSortDirection.Ascending;
            _sortProperty = null;

            WireInterceptor(_originalSource);

            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        /// <summary>
        /// Gets the direction of the sort.
        /// </summary>
        /// <value></value>
        /// <returns>One of the <see cref="T:System.ComponentModel.ListSortDirection"/> values.</returns>
        /// <exception cref="T:System.NotSupportedException">
        /// 	<see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
        public ListSortDirection SortDirection
        {
            get { return _sortDirection; }
        }

        /// <summary>
        /// Gets the <see cref="T:System.ComponentModel.PropertyDescriptor"/> that is being used for sorting.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.ComponentModel.PropertyDescriptor"/> that is being used for sorting.</returns>
        /// <exception cref="T:System.NotSupportedException">
        /// 	<see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
        public PropertyDescriptor SortProperty
        {
            get { return _sortProperty; }
        }

        /// <summary>
        /// Gets whether a <see cref="E:System.ComponentModel.IBindingList.ListChanged"/> event is raised when the list changes or an item in the list changes.
        /// </summary>
        /// <value></value>
        /// <returns>true if a <see cref="E:System.ComponentModel.IBindingList.ListChanged"/> event is raised when the list changes or when an item changes; otherwise, false.</returns>
        public bool SupportsChangeNotification
        {
            get { return true; }
        }

        /// <summary>
        /// Gets whether the list supports searching using the <see cref="M:System.ComponentModel.IBindingList.Find(System.ComponentModel.PropertyDescriptor,System.Object)"/> method.
        /// </summary>
        /// <value></value>
        /// <returns>true if the list supports searching using the <see cref="M:System.ComponentModel.IBindingList.Find(System.ComponentModel.PropertyDescriptor,System.Object)"/> method; otherwise, false.</returns>
        public bool SupportsSearching
        {
            get { return true; }
        }

        /// <summary>
        /// Gets whether the list supports sorting.
        /// </summary>
        /// <value></value>
        /// <returns>true if the list supports sorting; otherwise, false.</returns>
        public bool SupportsSorting
        {
            get { return true; }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"/> to add to the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>
        /// The position into which the new element was inserted.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        public int Add(object value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only. </exception>
        public void Clear()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.IList"/> contains a specific value.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"/> to locate in the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>
        /// true if the <see cref="T:System.Object"/> is found in the <see cref="T:System.Collections.IList"/>; otherwise, false.
        /// </returns>
        public bool Contains(object value)
        {
            return IndexOf(value) >= 0;
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"/> to locate in the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>
        /// The index of <paramref name="value"/> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(object value)
        {
            var index = 0;
            foreach (var item in _source)
            {
                if (value == item) return index;
                index++;
            }

            return -1;
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.IList"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="value"/> should be inserted.</param>
        /// <param name="value">The <see cref="T:System.Object"/> to insert into the <see cref="T:System.Collections.IList"/>.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        /// <exception cref="T:System.NullReferenceException">
        /// 	<paramref name="value"/> is null reference in the <see cref="T:System.Collections.IList"/>.</exception>
        public void Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IList"/> has a fixed size.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Collections.IList"/> has a fixed size; otherwise, false.</returns>
        public bool IsFixedSize
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IList"/> is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Collections.IList"/> is read-only; otherwise, false.</returns>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"/> to remove from the <see cref="T:System.Collections.IList"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        public void Remove(object value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.IList"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> at the specified index.
        /// </summary>
        /// <value></value>
        public object this[int index]
        {
            get
            {
                return _source.ElementAt(index);
            }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="array"/> is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is less than zero. </exception>
        /// <exception cref="T:System.ArgumentException">
        /// 	<paramref name="array"/> is multidimensional.-or- <paramref name="index"/> is equal to or greater than the length of <paramref name="array"/>.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>. </exception>
        /// <exception cref="T:System.ArgumentException">The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>. </exception>
        public void CopyTo(Array array, int index)
        {
            foreach (var element in _source)
            {
                if (index < array.Length)
                {
                    array.SetValue(element, index);
                    index++;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <value></value>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"/>.</returns>
        public int Count
        {
            get { return _source.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
        /// </summary>
        /// <value></value>
        /// <returns>true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.</returns>
        public bool IsSynchronized
        {
            get { return true; }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <value></value>
        /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.</returns>
        public object SyncRoot
        {
            get { return new object(); }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return _source.GetEnumerator();
        }
        #endregion

        private void WireInterceptor(IBindableCollection<TElement> source)
        {
            _source = source;
            _source.CollectionChanged += _weakHandler.Handler;

            _propertyChangeObserver = new PropertyChangeObserver(Element_PropertyChanged);
            _addActioner = new ElementActioner<TElement>(_source, element => _propertyChangeObserver.Attach(element), element => _propertyChangeObserver.Detach(element), Dispatcher);
        }

        private void UnwireInterceptor()
        {
            _addActioner.Dispose();
            _propertyChangeObserver.Dispose();
            _source.CollectionChanged -= _weakHandler.Handler;
            if (_source != _originalSource) _source.Dispose();
            _source = null;
        }

        private object KeySelector(TElement item)
        {
            if (null == item || null == _sortProperty)
            {
                return null;
            }
            return _sortProperty.GetValue(item);
        }

        private void Element_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_propertyDescriptors.ContainsKey(e.PropertyName))
            {
                var descriptor = _propertyDescriptors[e.PropertyName];
                var index = IndexOf(sender);
                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index, descriptor));
            }
        }

        private void Source_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        var index = e.NewStartingIndex;
                        foreach (var item in e.NewItems)
                        {
                            var listEvent = new ListChangedEventArgs(ListChangedType.ItemAdded, index);
                            OnListChanged(listEvent);
                            index++;
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    {
                        var newIndex = e.NewStartingIndex;
                        var oldIndex = e.OldStartingIndex;
                        foreach (var item in e.NewItems)
                        {
                            var listEvent = new ListChangedEventArgs(ListChangedType.ItemMoved, newIndex, oldIndex);
                            OnListChanged(listEvent);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    {
                        var index = e.OldStartingIndex;
                        foreach (var item in e.OldItems)
                        {
                            var listEvent = new ListChangedEventArgs(ListChangedType.ItemDeleted, index);
                            OnListChanged(listEvent);
                            index++;
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    {
                        var index = e.NewStartingIndex;
                        foreach (var item in e.NewItems)
                        {
                            var listEvent = new ListChangedEventArgs(ListChangedType.ItemChanged, index);
                            OnListChanged(listEvent);
                            index++;
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    {
                        var listEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
                        OnListChanged(listEvent);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:ListChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.ListChangedEventArgs"/> instance containing the event data.</param>
        private void OnListChanged(ListChangedEventArgs e)
        {
            var handler = ListChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Called just before the object is disposed and all event subscriptions are released.
        /// </summary>
        protected override void BeforeDisposeOverride()
        {
            _addActioner.Dispose();
            _propertyChangeObserver.Dispose();
            base.BeforeDisposeOverride();
        }
    }
#endif
}