namespace Bindable.Linq
{
    /// <summary>
    /// Indicates which mode of automatic dependency analysis to conduct on an expression. 
    /// </summary>
    public enum DependencyDiscovery
    {
        /// <summary>
        /// Bindable LINQ will inspect the expression(s) passed to this extension method and look for property access or control access in order to detect  
        /// and wire up dependencies automatically. This is the default behavior.
        /// </summary>
        Enabled,

        /// <summary>
        /// No automatic dependency analysis will take place. Dependencies can still be added manually using the DependantOn extension methods. 
        /// </summary>
        Disabled
    }
}
