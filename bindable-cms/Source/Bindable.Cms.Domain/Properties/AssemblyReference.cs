using System.Reflection;

namespace Bindable.Cms.Domain.Properties
{
    /// <summary>
    /// Returns a reference to this assembly.
    /// </summary>
    public static class AssemblyReference
    {
        /// <summary>
        /// Gets the Domain assembly.
        /// </summary>
        public static Assembly Assembly
        {
            get { return Assembly.GetExecutingAssembly(); }
        }
    }
}