using System;
using System.ComponentModel;
using System.Windows;

namespace Bindable.Linq.Dependencies.PathNavigation.Tokens
{
#if !SILVERLIGHT
    /// <summary>
    /// A property monitor for WPF DependencyProperties.
    /// </summary>
    internal sealed class WpfMemberToken : MemberToken
    {
        private readonly DependencyProperty _dependencyProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfMemberToken"/> class.
        /// </summary>
        /// <param name="objectToObserve">The object to observe.</param>
        /// <param name="dependencyProperty">The dependency property.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="remainingPath">The remaining path.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="pathNavigator">The path navigator.</param>
        public WpfMemberToken(DependencyObject objectToObserve, DependencyProperty dependencyProperty, string propertyName, string remainingPath, Action<object, string> callback, IPathNavigator pathNavigator)
            : base(objectToObserve, propertyName, remainingPath, callback, pathNavigator)
        {
            _dependencyProperty = dependencyProperty;

            AcquireTarget(objectToObserve);
        }

        /// <summary>
        /// When overridden in a derived class, gives the class an opportunity to discard the current target.
        /// </summary>
        protected override void DiscardCurrentTargetOverride()
        {
            var currentTarget = CurrentTarget as DependencyObject;
            if (currentTarget != null)
            {
                var dpd = DependencyPropertyDescriptor.FromProperty(_dependencyProperty, currentTarget.GetType());
                if (dpd != null)
                {
                    dpd.RemoveValueChanged(currentTarget, CurrentTarget_PropertyChanged);
                }
            }
        }

        /// <summary>
        /// When overridden in a derived class, gives the class an opportunity to monitor the current target.
        /// </summary>
        protected override void MonitorCurrentTargetOverride()
        {
            var currentTarget = CurrentTarget as DependencyObject;
            if (currentTarget != null)
            {
                var dpd = DependencyPropertyDescriptor.FromProperty(_dependencyProperty, currentTarget.GetType());
                if (dpd != null)
                {
                    dpd.AddValueChanged(currentTarget, CurrentTarget_PropertyChanged);
                }
            }
        }

        /// <summary>
        /// When overriden in a derived class, gets the value of the current target object.
        /// </summary>
        /// <returns></returns>
        protected override object ReadCurrentPropertyValueOverride()
        {
            if (_dependencyProperty != null && CurrentTarget != null)
            {
                return ((DependencyObject) CurrentTarget).GetValue(_dependencyProperty);
            }
            return null;
        }

        public void CurrentTarget_PropertyChanged(object sender, EventArgs e)
        {
            HandleCurrentTargetPropertyValueChanged();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        protected override void DisposeOverride()
        {
            DiscardCurrentTargetOverride();
        }
    }
#endif
}