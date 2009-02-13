using System;
using Castle.Core;
using Castle.MicroKernel.Facilities;

namespace Bindable.Cms.Domain.Infrastructure.ApplicationModel.CastleExtensions
{
    /// <summary>
    /// A facility that adds support for resolving components by invoking a callback delegate.
    /// </summary>
    public class DelegateSupportFacility : AbstractFacility
    {
        /// <summary>
        /// The custom initialization for the Facility.
        /// </summary>
        /// <remarks>It must be overriden.</remarks>
        protected override void Init()
        {
            Kernel.ComponentModelCreated += Kernel_ComponentModelCreated;
        }

        private static void Kernel_ComponentModelCreated(ComponentModel model)
        {
            var callback = model.ExtendedProperties["callback"] as Func<object>;
            if (callback == null) return;

            model.Constructors.Clear();
            model.CustomComponentActivator = typeof(DelegateActivator);
        }
    }
}