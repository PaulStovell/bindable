using System;
using System.Linq.Expressions;
using Bindable.Core.Helpers;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Operators;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
    {
        /// <summary>
        /// Projects a single bindable object into another bindable object, using a lambda to select the new
        /// type of object.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source colection.</param>
        /// <param name="projector">The projector function used to turn the source type into the result type.</param>
        /// <returns>
        /// An object created by the <paramref name="projector"/>. If the source value changes, the item will be projected again.
        /// </returns>
        public static IBindable<TResult> Project<TSource, TResult>(this IBindable<TSource> source, Expression<Func<TSource, TResult>> projector)
        {
            return source.Project(projector, DefaultDependencyAnalysis);
        }

        /// <summary>
        /// Projects a single bindable object into another bindable object, using a lambda to select the new
        /// type of object.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source colection.</param>
        /// <param name="projector">The projector function used to turn the source type into the result type.</param>
        /// <param name="dependencyAnalysisMode">The dependency analysis mode.</param>
        /// <returns>
        /// An object created by the <paramref name="projector"/>. If the source value changes, the item will be projected again.
        /// </returns>
        public static IBindable<TResult> Project<TSource, TResult>(this IBindable<TSource> source, Expression<Func<TSource, TResult>> projector, DependencyDiscovery dependencyAnalysisMode)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(projector, "projector");
            var result = new ProjectOperator<TSource, TResult>(source, projector.Compile(), source.Dispatcher);
            if (dependencyAnalysisMode == DependencyDiscovery.Enabled)
            {
                return result.DependsOnExpression(projector.Body, projector.Parameters[0]);
            }
            return result;
        }
	}
}
