using Bindable.Linq.Dependencies.Instances;
using Bindable.Linq.Dependencies.PathNavigation;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq.Dependencies.Definitions
{
    /// <summary>
    /// Defines a dependency on an external object that implements the INotifyPropertyChanged interface.
    /// </summary>
    public sealed class ExternalDependencyDefinition : IDependencyDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalDependencyDefinition"/> class.
        /// </summary>
        /// <param name="propertyPath">The property path.</param>
        /// <param name="targetObject">The target object.</param>
        public ExternalDependencyDefinition(string propertyPath, object targetObject)
        {
            PropertyPath = propertyPath;
            TargetObject = targetObject;
        }

        /// <summary>
        /// Gets or sets the property path.
        /// </summary>
        /// <value>The property path.</value>
        public string PropertyPath { get; set; }

        /// <summary>
        /// Gets or sets the target object.
        /// </summary>
        /// <value>The target object.</value>
        public object TargetObject { get; set; }

        /// <summary>
        /// Determines whether this instance can construct dependencies for a collection.
        /// </summary>
        /// <returns></returns>
        public bool AppliesToCollections()
        {
            return true;
        }

        /// <summary>
        /// Determines whether this instance can construct dependencies for a single element.
        /// </summary>
        /// <returns></returns>
        public bool AppliesToSingleElement()
        {
            return true;
        }

        /// <summary>
        /// Constructs the dependency for a collection of elements.
        /// </summary>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="sourceElements">The source elements.</param>
        /// <param name="pathNavigator">The path navigator.</param>
        /// <returns></returns>
        public IDependency ConstructForCollection<TElement>(IBindableCollection<TElement> sourceElements, IPathNavigator pathNavigator)
        {
            return new ExternalDependency(TargetObject, PropertyPath, pathNavigator);
        }

        /// <summary>
        /// Constructs a dependency for a single element.
        /// </summary>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="sourceElement">The source element.</param>
        /// <param name="pathNavigator">The path navigator.</param>
        /// <returns></returns>
        public IDependency ConstructForElement<TElement>(TElement sourceElement, IPathNavigator pathNavigator)
        {
            return new ExternalDependency(TargetObject, PropertyPath, pathNavigator);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}: '{1}' on '{2}'", GetType().Name, PropertyPath, TargetObject.GetType().Name);
        }
    }
}