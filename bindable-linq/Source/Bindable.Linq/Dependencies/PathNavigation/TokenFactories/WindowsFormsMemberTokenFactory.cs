using System;
using Bindable.Linq.Dependencies.PathNavigation.Tokens;

namespace Bindable.Linq.Dependencies.PathNavigation.TokenFactories
{
    /// <summary>
    /// A property parser for properties on a Windows Forms control.
    /// </summary>
    public sealed class WindowsFormsMemberTokenFactory : ITokenFactory
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
#if !SILVERLIGHT
            if (target is System.Windows.Forms.Control)
            {
                var propertyName = propertyPath;
                string remainingPath = null;
                var dotIndex = propertyPath.IndexOf('.');
                if (dotIndex >= 0)
                {
                    propertyName = propertyPath.Substring(0, dotIndex);
                    remainingPath = propertyPath.Substring(dotIndex + 1);
                }
                result = new WindowsFormsMemberToken(target, propertyName, remainingPath, callback, pathNavigator);
            }
#endif
            return result;
        }
        #endregion
    }
}