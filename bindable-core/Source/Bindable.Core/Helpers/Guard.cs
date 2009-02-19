using System;

namespace Bindable.Core.Helpers
{
    /// <summary>
    /// A helper for validating arguments passed to a method.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Ensures the given value is not null.
        /// </summary>
        /// <param name="o">The value of the argument.</param>
        /// <param name="name">The name of the argument.</param>
        public static void NotNull(object o, string name)
        {
            if (o == null) throw new ArgumentNullException(name);
        }

        /// <summary>
        /// Ensures the given value is not null or an empty string.
        /// </summary>
        /// <param name="o">The value of the argument.</param>
        /// <param name="name">The name of the argument.</param>
        public static void NotNullOrEmpty(object o, string name)
        {
            if (o == null || string.IsNullOrEmpty((string)o)) throw new ArgumentNullException(name);
        }
    }
}
