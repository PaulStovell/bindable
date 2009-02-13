using System.Windows.Threading;

namespace Bindable.Core.Threading
{
    /// <summary>
    /// A factory for creating the correct <see cref="IDispatcher"/> implementation based on
    /// the current environment.
    /// </summary>
    public sealed class DispatcherFactory
    {
        /// <summary>
        /// Creates the specified dispatcher.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <returns>The correct IDispatcher.</returns>
        public static IDispatcher Create(Dispatcher dispatcher)
        {
            IDispatcher result = null;
            if (dispatcher != null)
            {
#if SILVERLIGHT
                result = new SilverlightDispatcher(dispatcher);
#else
                result = new WpfDispatcher(dispatcher);
#endif
            }
            return result;
        }

#if! SILVERLIGHT
        public static IDispatcher Create()
        {
            return Create(Dispatcher.CurrentDispatcher);
        }
#endif
    }
}