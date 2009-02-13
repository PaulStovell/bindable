using System;
using Bindable.Linq.Dependencies.Instances;
using Bindable.Linq.Dependencies.PathNavigation;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq.Dependencies.Definitions
{
    /// <summary>
    /// Defines a dependency on a property on an item where the item implements the INotifyPropertyChanged interface.
    /// </summary>
    public sealed class ItemDependencyDefinition : IDependencyDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemDependencyDefinition"/> class.
        /// </summary>
        /// <param name="propertyPath">The property path.</param>
        public ItemDependencyDefinition(string propertyPath)
        {
            PropertyPath = propertyPath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemDependencyDefinition"/> class.
        /// </summary>
        /// <param name="propertyPath">The property path.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        public ItemDependencyDefinition(string propertyPath, string parameterName)
        {
            PropertyPath = propertyPath;
            ParameterName = parameterName;
        }

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>The name of the parameter.</value>
        public string ParameterName { get; set; }

        /// <summary>
        /// Gets or sets the property path.
        /// </summary>
        /// <value>The property path.</value>
        public string PropertyPath { get; set; }

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
            return false;
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
            return new ItemDependency<TElement>(PropertyPath, sourceElements, pathNavigator);
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
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}: '{1}' on '{2}'", GetType().Name, PropertyPath, ParameterName);
        }
    }
}