using System;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.ComponentActivator;

namespace Bindable.Cms.Domain.Infrastructure.ApplicationModel.CastleExtensions
{
    /// <summary>
    /// A custom activator that uses a delegate to instantiate a component.
    /// </summary>
    public class DelegateActivator : DefaultComponentActivator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateActivator"/> class.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="kernel"></param>
        /// <param name="onCreation"></param>
        /// <param name="onDestruction"></param>
        public DelegateActivator(ComponentModel model, IKernel kernel, ComponentInstanceDelegate onCreation, ComponentInstanceDelegate onDestruction)
            : base(model, kernel, onCreation, onDestruction)
        {
        }

        /// <summary>
        /// Instantiates the model from the specified context using the delegate.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected override object Instantiate(CreationContext context)
        {
            var callback = (Func<object>)Model.ExtendedProperties["callback"];
            return callback();
        }
    }
}