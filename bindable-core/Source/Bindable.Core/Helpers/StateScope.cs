using System;

namespace Bindable.Core.Helpers
{
    /// <summary>
    /// This class is used to suppress events and to temporarily set property values. It is necessary 
    /// because when suppressing things like events using simple boolean flags, if one thread 
    /// suppresses it, then another suppresses, the first will then release while the other is still 
    /// running - leading to some inconsistent runtime behavior. 
    /// </summary>
    /// <remarks>
    /// <example>
    /// private StateScope _collectionChangedSuspension;
    /// 
    /// using (_collectionChangedSuspension.Enter()) 
    /// {
    ///     // Do stuff
    /// } // Will "Leave()" automatically
    /// 
    /// StateScope isLoadingState = _loadingState.Enter();
    /// // Do stuff
    /// isLoadingState.Leave();
    /// </example>
    /// </remarks>
    public sealed class StateScope : IDisposable
    {
        private readonly Action _callback;
        private readonly object _stateScopeLock = new object();
        private int _childrenCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateScope"/> class.
        /// </summary>
        public StateScope()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateScope"/> class.
        /// </summary>
        /// <param name="callback">A callback called when the state's IsWithin property changes.</param>
        public StateScope(Action callback)
        {
            _callback = callback;
        }

        /// <summary>
        /// Gets a value indicating whether anyone is currently within this state scope.
        /// </summary>
        public bool IsWithin
        {
            get { return _childrenCount > 0; }
        }

        /// <summary>
        /// Enters this state scope.
        /// </summary>
        public StateScope Enter()
        {
            var raiseCallback = false;
            lock (_stateScopeLock)
            {
                var wasWithin = IsWithin;
                _childrenCount++;
                if (wasWithin != IsWithin && _callback != null)
                {
                    raiseCallback = true;
                }
            }
            if (raiseCallback)
            {
                _callback();
            }
            return this;
        }

        /// <summary>
        /// Leaves this state scope.
        /// </summary>
        public void Leave()
        {
            var raiseCallback = false;
            lock (_stateScopeLock)
            {
                if (_childrenCount > 0)
                {
                    var wasWithin = IsWithin;
                    _childrenCount--;
                    if (wasWithin != IsWithin && _callback != null)
                    {
                        raiseCallback = true;
                    }
                }
            }
            if (raiseCallback)
            {
                _callback();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            Leave();
        }
    }
}