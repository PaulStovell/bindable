using System;
using Bindable.Linq.Helpers;

namespace Bindable.Linq.Dependencies.PathNavigation.Tokens
{    
    /// <summary>
    /// A property monitor for CLR based properties.
    /// </summary>
    internal sealed class WindowsFormsMemberToken : MemberToken
    {
        private readonly EventHandler _actualHandler;
        private IPropertyReader<object> _propertyReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsFormsMemberToken"/> class.
        /// </summary>
        /// <param name="objectToObserve">The object to observe.</param>
        /// <param name="propertyName">The property path.</param>
        /// <param name="remainingPath">The remaining path.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="pathNavigator">The path navigator.</param>
        public WindowsFormsMemberToken(object objectToObserve, string propertyName, string remainingPath, Action<object, string> callback, IPathNavigator pathNavigator)
            : base(objectToObserve, propertyName, remainingPath, callback, pathNavigator)
        {
            _actualHandler = CurrentTarget_PropertyChanged;

            AcquireTarget(objectToObserve);
        }

        /// <summary>
        /// When overridden in a derived class, gives the class an opportunity to discard the current target.
        /// </summary>
        protected override void DiscardCurrentTargetOverride()
        {
            if (CurrentTarget != null)
            {
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
            if (CurrentTarget != null)
            {
                var eventInfo = CurrentTarget.GetType().GetEvent(PropertyName + "Changed");
                if (eventInfo != null)
                {
                    var addMethod = eventInfo.GetAddMethod();
                    if (addMethod != null)
                    {
                        addMethod.Invoke(CurrentTarget, new object[] {_actualHandler});
                    }
                }
            }
            _propertyReader = PropertyReaderFactory.Create<object>(CurrentTarget.GetType(), PropertyName);
        }

        /// <summary>
        /// When overriden in a derived class, gets the value of the current target object.
        /// </summary>
        /// <returns></returns>
        protected override object ReadCurrentPropertyValueOverride()
        {
            if (_propertyReader != null)
            {
                return _propertyReader.GetValue(CurrentTarget);
            }
            return null;
        }

        private void CurrentTarget_PropertyChanged(object sender, EventArgs e)
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