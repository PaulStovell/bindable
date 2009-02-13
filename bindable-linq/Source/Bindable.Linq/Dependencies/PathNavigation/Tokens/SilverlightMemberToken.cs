using System;
using System.Reflection;
using System.Windows;

namespace Bindable.Linq.Dependencies.PathNavigation.Tokens
{
    /// <summary>
    /// A property monitor for WPF DependencyProperties.
    /// </summary>
    internal sealed class SilverlightMemberToken : MemberToken
    {
        private readonly EventHandler _actualHandler;
        private readonly DependencyProperty _dependencyProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="SilverlightMemberToken"/> class.
        /// </summary>
        /// <param name="objectToObserve">The object to observe.</param>
        /// <param name="dependencyProperty">The dependency property.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="remainingPath">The remaining path.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="pathNavigator">The path navigator.</param>
        public SilverlightMemberToken(DependencyObject objectToObserve, DependencyProperty dependencyProperty, string propertyName, string remainingPath, Action<object, string> callback, IPathNavigator pathNavigator)
            : base(objectToObserve, propertyName, remainingPath, callback, pathNavigator)
        {
            _dependencyProperty = dependencyProperty;
            _actualHandler = CurrentTarget_PropertyChanged;

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
                //var dpd = DependencyPropertyDescriptor.FromProperty(_dependencyProperty, currentTarget.GetType());
                //if (dpd != null)
                //{
                //    dpd.RemoveValueChanged(currentTarget, CurrentTarget_PropertyChanged);
                //}

                var eventInfo = CurrentTarget.GetType().GetEvent(PropertyName + "Changed");
                if (eventInfo != null)
                {
                    var removeMethod = eventInfo.GetRemoveMethod();
                    if (removeMethod != null)
                    {
                        removeMethod.Invoke(CurrentTarget, new object[] {_actualHandler});
                    }
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
                //var dpd = DependencyPropertyDescriptor.FromProperty(_dependencyProperty, currentTarget.GetType());
                //if (dpd != null)
                //{
                //    dpd.AddValueChanged(currentTarget, CurrentTarget_PropertyChanged);
                //}

                var eventInfo = CurrentTarget.GetType().GetEvent(PropertyName + "Changed");
                if (eventInfo != null)
                {
                    var addMethod = eventInfo.GetAddMethod();
                    if (addMethod != null)
                    {
                        var pi = addMethod.GetParameters()[0];

                        var d = Delegate.CreateDelegate(pi.ParameterType, this, GetType().GetMethod("CurrentTarget_PropertyChanged", BindingFlags.Public | BindingFlags.Instance));
                        addMethod.Invoke(CurrentTarget, new object[] {d});
                    }
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
}