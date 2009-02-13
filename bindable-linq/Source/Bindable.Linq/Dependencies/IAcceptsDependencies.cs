namespace Bindable.Linq.Dependencies
{
    /// <summary>
    /// Implemented by Bindable LINQ operations which can have dependencies.
    /// </summary>
    public interface IAcceptsDependencies
    {
        /// <summary>
        /// Sets a new dependency on a Bindable LINQ operation.
        /// </summary>
        /// <param name="definition">A definition of the dependency.</param>
        void AcceptDependency(IDependencyDefinition definition);
    }
}