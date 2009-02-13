using Bindable.Linq.Dependencies;

namespace Bindable.Linq
{
    /// <summary>
    /// This class contains all of the extension method implementations provided by Bindable LINQ. 
    /// </summary>
    public static partial class BindableEnumerable
    {
        // You will find the implementations of each extension method 
        // under the /Extensions folders. 
        private static DependencyDiscovery DefaultDependencyAnalysis = DependencyDiscovery.Enabled;
    }
}