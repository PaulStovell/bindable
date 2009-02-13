namespace Bindable.Linq.Configuration
{
    /// <summary>
    /// Contains the inbuilt Bindable LINQ binding configurations.
    /// </summary>
    public static class BindingConfigurations
    {
        private static readonly IBindingConfiguration _default = new DefaultBindingConfiguration();
        private static readonly IBindingConfiguration _explicitDependenciesOnly = new ExplicitBindingConfiguration();

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>The default.</value>
        public static IBindingConfiguration Default
        {
            get { return _default; }
        }

        /// <summary>
        /// Gets the explicit dependencies only.
        /// </summary>
        /// <value>The explicit dependencies only.</value>
        public static IBindingConfiguration ExplicitDependenciesOnly
        {
            get { return _explicitDependenciesOnly; }
        }
    }
}