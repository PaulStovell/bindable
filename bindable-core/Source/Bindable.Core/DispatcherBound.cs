using System;
using Bindable.Aspects.Parameters;
using Bindable.Core.Helpers;
using Bindable.Core.Threading;

namespace Bindable.Core
{
    /// <summary>
    /// Serves as a base class for all Bindable LINQ objects which are bound to a specific owning thread.
    /// </summary>
    public abstract class DispatcherBound : IDisposable
    {
        private readonly IDispatcher _dispatcher;
        private readonly object _instanceLock = new object();
        private readonly LifetimeCouplings _instanceLifetime = new LifetimeCouplings();
        private bool _isSealed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatcherBound"/> class.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        protected DispatcherBound([NotNull]IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Gets the instance lock.
        /// </summary>
        /// <value>The instance lock.</value>
        protected object InstanceLock
        {
            get { return _instanceLock; }
        }

        /// <summary>
        /// Gets the instance lifetime.
        /// </summary>
        protected LifetimeCouplings InstanceLifetime
        {
            get { return _instanceLifetime; }
        }

        /// <summary>
        /// Gets the dispatcher that owns this object.
        /// </summary>
        public IDispatcher Dispatcher
        {
            get { return _dispatcher; }
        }

        /// <summary>
        /// Asserts that the calling thread is owned by the current dispatcher.
        /// </summary>
        protected void AssertDispatcherThread()
        {
            if (Dispatcher.DispatchRequired())
            {
                // TODO: Better messages
                var message = "Must be called on UI thread.";
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Asserts that the current object has not been sealed and evaluated.
        /// </summary>
        protected void AssertUnsealed()
        {
            if (_isSealed)
            {
                // TODO: Better messages
                var message = "Must not be sealed.";
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Seals this instance. Certain settings cannot be changed once the instance has been sealed.
        /// </summary>
        protected void Seal()
        {
            BeforeSealingOverride();
            _isSealed = true;
        }

        /// <summary>
        /// When overridden in a derived class, provides the ability for the class to validate any settings before it is "sealed" permanently.
        /// </summary>
        protected virtual void BeforeSealingOverride()
        {
        }

        /// <summary>
        /// Called just before the object is disposed and all event subscriptions are released.
        /// </summary>
        protected virtual void BeforeDisposeOverride()
        {
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            BeforeDisposeOverride();
            InstanceLifetime.Dispose();
        }
    }
}