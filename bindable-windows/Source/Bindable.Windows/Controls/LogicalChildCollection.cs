using System;
using System.Collections;
using System.Collections.Generic;

namespace Bindable.Windows.Controls
{
    public class LogicalChildCollection<TElement> : IList<TElement>, IList
    {
        private readonly List<TElement> _innerList = new List<TElement>();
        private readonly ILogicalChildContainer _owner;

        public LogicalChildCollection(ILogicalChildContainer owner)
        {
            _owner = owner;
        }

        protected ILogicalChildContainer Owner
        {
            get { return _owner; }
        }

        protected virtual TElement ConnectChild(TElement entry)
        {
            Owner.AddLogicalChild(entry);
            return entry;
        }

        protected virtual TElement DisconnectChild(TElement entry)
        {
            Owner.RemoveLogicalChild(entry);
            return entry;
        }

        public int IndexOf(TElement item)
        {
            return _innerList.IndexOf(item);
        }

        public void Insert(int index, TElement item)
        {
            _innerList.Insert(index, ConnectChild(item));
        }

        public void RemoveAt(int index)
        {
            _innerList.Remove(DisconnectChild(this[index]));
        }

        public TElement this[int index]
        {
            get
            {
                return _innerList[index];
            }
            set
            {
                _innerList[index] = ConnectChild(value);
            }
        }

        public void Add(TElement item)
        {
            _innerList.Add(ConnectChild(item));
        }

        public void Clear()
        {
            foreach (var item in _innerList) Remove(item);
        }

        public bool Contains(TElement item)
        {
            return _innerList.Contains(item);
        }

        public void CopyTo(TElement[] array, int arrayIndex)
        {
            _innerList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _innerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(TElement item)
        {
            return _innerList.Remove(DisconnectChild(item));
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Add(object value)
        {
            Add((TElement)value);
            return IndexOf((TElement)value);
        }

        public bool Contains(object value)
        {
            return Contains((TElement)value);
        }

        public int IndexOf(object value)
        {
            return IndexOf((TElement)value);
        }

        public void Insert(int index, object value)
        {
            Insert(index, (TElement)value);
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public void Remove(object value)
        {
            Remove((TElement)value);
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = (TElement)value;
            }
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotSupportedException();
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return new object(); }
        }
    }
}
