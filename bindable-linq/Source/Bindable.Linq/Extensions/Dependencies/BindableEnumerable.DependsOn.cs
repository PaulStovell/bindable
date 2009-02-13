using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Windows;
using Bindable.Linq.Configuration;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Dependencies.Definitions;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>
        /// Extracts dependencies from the given expression and adds them to the iterator.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="itemParameter">The item parameter.</param>
        /// <returns></returns>
        public static TResult DependsOnExpression<TResult>(this TResult query, System.Linq.Expressions.Expression expression, ParameterExpression itemParameter) where TResult : IAcceptsDependencies
        {
            var analyzer = BindingConfigurations.Default.CreateExpressionAnalyzer();
            var definitions = analyzer.DiscoverDependencies(expression, itemParameter);
            return query.DependsOn(definitions);
        }

        /// <summary>
        /// Adds a dependency for a given property on a given object external to the query.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="externalObject">The external object to monitor for changes. For example, this could be a regular 
        /// class object, an object that implements <see cref="INotifyCollectionChanged" />, or a Windows Forms control.</param>
        /// <param name="propertyPath">The property path. For example: "HomeAddress.Postcode".</param>
        /// <returns></returns>
        public static TResult DependsOn<TResult>(this TResult query, object externalObject, string propertyPath) where TResult : IAcceptsDependencies
        {
            return query.DependsOn(new ExternalDependencyDefinition(propertyPath, externalObject));
        }

#if SILVERLIGHT
        /// <summary>
        /// Adds a dependency on a Silverlight dependency object.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="dependencyObject">A Silverlight dependency object.</param>
        /// <param name="propertyPath">The name of a property on the dependency object.</param>
        /// <returns></returns>
        public static TResult DependsOn<TResult>(this TResult query, System.Windows.DependencyObject dependencyObject, string propertyPath)
            where TResult : IAcceptsDependencies
        {
            return query.DependsOn(new ExternalDependencyDefinition(propertyPath, dependencyObject));
        }
#else
        /// <summary>
        /// Adds a dependency on a WPF dependency object.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="dependencyObject">A WPF dependency object.</param>
        /// <param name="dependencyProperty">A WPF dependency property.</param>
        /// <returns></returns>
        public static TResult DependsOn<TResult>(this TResult query, DependencyObject dependencyObject, DependencyProperty dependencyProperty)
            where TResult : IAcceptsDependencies
        {
            return query.DependsOn(new ExternalDependencyDefinition(dependencyProperty.Name, dependencyObject));
        }
#endif

        /// <summary>
        /// Adds a dependency on items in the source collection, given the path to a property.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="propertyPath">The property name or path. For example: "HomeAddress.Postcode".</param>
        /// <returns></returns>
        public static TResult DependsOn<TResult>(this TResult query, string propertyPath) where TResult : IAcceptsDependencies
        {
            return query.DependsOn(new ItemDependencyDefinition(propertyPath));
        }

        /// <summary>
        /// Adds a dependency to the Bindable LINQ query given a dependency definition. This allows developers to create custom 
        /// dependency types by implementing the <see cref="IDependencyDefinition"/> interface.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="definition">The definition.</param>
        /// <returns></returns>
        public static TResult DependsOn<TResult>(this TResult query, IDependencyDefinition definition) where TResult : IAcceptsDependencies
        {
            if (query != null && definition != null)
            {
                query.AcceptDependency(definition);
            }
            return query;
        }

        /// <summary>
        /// Adds a set of dependencies to the Bindable LINQ query.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="definitions">The definitions.</param>
        /// <returns></returns>
        public static TResult DependsOn<TResult>(this TResult query, IEnumerable<IDependencyDefinition> definitions) where TResult : IAcceptsDependencies
        {
            if (query != null)
            {
                foreach (var definition in definitions)
                {
                    if (definition != null)
                    {
                        query.AcceptDependency(definition);
                    }
                }
            }
            return query;
        }
	}
}
