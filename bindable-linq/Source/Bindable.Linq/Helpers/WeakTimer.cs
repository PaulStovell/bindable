using System;
using System.Threading;

namespace Bindable.Linq.Helpers
{
    /// <summary>
    /// Represents a timer that invokes its callback via a weak references.
    /// </summary>
    internal sealed class WeakTimer : IDisposable
    {
        private readonly WeakReference _callbackReference;
        private readonly TimeSpan _pollTime;
        private Timer _timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakTimer"/> class.
        /// </summary>
        /// <param name="pollTime">The poll time.</param>
        /// <param name="callback">The callback.</param>
        public WeakTimer(TimeSpan pollTime, Action callback)
        {
            _pollTime = pollTime;
            _callbackReference = new WeakReference(callback, true);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            _timer = new Timer(TimerTickCallback, null, _pollTime, _pollTime);
        }

        /// <summary>
        /// Pauses this instance.
        /// </summary>
        public void Pause()
        {
            if (_timer != null)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        /// <summary>
        /// Continues this instance.
        /// </summary>
        public void Continue()
        {
            if (_timer != null)
            {
                _timer.Change(_pollTime, _pollTime);
            }
        }

        /// <summary>
        /// Timers the tick callback.
        /// </summary>
        /// <param name="o">The o.</param>
        private void TimerTickCallback(object o)
        {
            var action = _callbackReference.Target as Action;
            if (action != null)
            {
                action();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Pause();
            _timer.Dispose();
        }
    }
}