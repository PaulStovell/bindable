using System;
using System.Windows.Threading;
using System.Threading;

namespace Bindable.Core.Threading
{
#if SILVERLIGHT    
    /// <summary>
    /// This dispatcher is used at runtime by both Windows Forms and WPF. The WPF Dispatcher 
    /// class works within Windows Forms, so this appears to be safe.
    /// </summary>
    public class SilverlightDispatcher : IDispatcher
    {
        private Dispatcher _dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="SilverlightDispatcher"/> class.
        /// </summary>
        public SilverlightDispatcher(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Dispatches the specified action to the thread.
        /// </summary>
        /// <param name="actionToInvoke">The action to invoke.</param>
        public void Dispatch(Action actionToInvoke)
        {
            if (DispatchRequired()) 
            {
                // We want to make this operation synchronous, but the Silverlight dispatcher only currently supports async posting
                var resetEvent = new AutoResetEvent(false);
                _dispatcher.BeginInvoke(
                    () => 
                    {
                        try
                        {
                            actionToInvoke();
                        }
                        finally 
                        {
                            resetEvent.Set();
                        }
                    }
                    );
                resetEvent.WaitOne();
            }
            else
            {
                actionToInvoke();
            }
        }

        /// <summary>
        /// Dispatches the specified action to the thread.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="actionToInvoke">The action to invoke.</param>
        /// <returns></returns>
        public TResult Dispatch<TResult>(Func<TResult> actionToInvoke)
        {
            if (DispatchRequired()) 
            {
                // We want to make this operation synchronous, but the Silverlight dispatcher only currently supports async posting
                var result = default(TResult);
                var resetEvent = new AutoResetEvent(false);
                _dispatcher.BeginInvoke(
                    () => 
                    {
                        try
                        {
                            result = actionToInvoke();
                        }
                        finally 
                        {
                            resetEvent.Set();
                        }
                    }
                    );
                resetEvent.WaitOne();
                return result;
            }
            else
            {
                return actionToInvoke();
            }
        }

        /// <summary>
        /// Checks whether the thread invoking the method .
        /// </summary>
        /// <returns></returns>
        public bool DispatchRequired()
        {
            return !_dispatcher.CheckAccess();
        }
    }
#endif
}