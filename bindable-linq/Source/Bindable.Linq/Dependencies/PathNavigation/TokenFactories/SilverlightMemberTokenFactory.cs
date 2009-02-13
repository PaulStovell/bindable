using System;
using System.Windows;
using System.Reflection;
using Bindable.Linq.Dependencies.PathNavigation.Tokens;

namespace Bindable.Linq.Dependencies.PathNavigation.TokenFactories
{
    /// <summary>
    /// A parser for Silverlight dependency properties.
    /// </summary>
    public sealed class SilverlightMemberTokenFactory : ITokenFactory
    {
        #region ITokenFactory Members
        /// <summary>
        /// Creates an appropriate property monitor for the remaining property path string on the target object.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="propertyPath">The property path.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="pathNavigator">The path navigator.</param>
        /// <returns>
        /// An appropriate <see cref="IToken"/> for the property.
        /// </returns>
        public IToken ParseNext(object target, string propertyPath, Action<object, string> callback, IPathNavigator pathNavigator)
        {
            IToken result = null;
#if SILVERLIGHT
            if (target is DependencyObject)
            {
                string propertyName = propertyPath;
                string remainingPath = null;
                int dotIndex = propertyPath.IndexOf('.');
                if (dotIndex >= 0)
                {
                    propertyName = propertyPath.Substring(0, dotIndex);
                    remainingPath = propertyPath.Substring(dotIndex + 1);
                }

                DependencyObject dependencyObject = (DependencyObject)target;
                if (dependencyObject != null)
                {
                    FieldInfo field = dependencyObject.GetType().GetField(propertyName + "Property", BindingFlags.Public | BindingFlags.Static);
                    if (field != null)
                    {
                        DependencyProperty dependencyProperty = (DependencyProperty)field.GetValue(null);
                        if (dependencyProperty != null)
                        {
                            result = new SilverlightMemberToken(dependencyObject, dependencyProperty, propertyName, remainingPath, callback, pathNavigator);
                        }
                    }
                }
            }
#endif
            return result;
        }
        #endregion
    }
}