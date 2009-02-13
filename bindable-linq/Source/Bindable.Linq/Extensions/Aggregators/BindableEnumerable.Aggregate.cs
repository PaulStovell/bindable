using System;
using System.Linq.Expressions;
using Bindable.Linq.Aggregators;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Dependencies;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>
        /// Applies an accumulator function over a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TSource}"/> to aggregate over.</param>
        /// <param name="func">An accumulator function to be invoked on each element.</param>
        /// <returns>The final accumulator value.</returns>
        public static IBindable<TResult> Aggregate<TSource, TResult>(this IBindableCollection<TSource> source, Expression<Func<IBindableCollection<TSource>, TResult>> func)
        {
            return source.Aggregate(func, DefaultDependencyAnalysis);
        }

        /// <summary>
        /// Applies an accumulator function over a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TSource}"/> to aggregate over.</param>
        /// <param name="func">An accumulator function to be invoked on each element.</param>
        /// <returns>The final accumulator value.</returns>
        public static IBindable<TSource> Aggregate<TSource>(this IBindableCollection<TSource> source, Expression<Func<IBindableCollection<TSource>, TSource>> func)
        {
            return source.Aggregate(func, DefaultDependencyAnalysis);
        }

        /// <summary>
        /// Applies an accumulator function over a sequence. The specified seed value is used as the 
        /// initial accumulator value.
        /// </summary>
        /// <param name="source">An <see cref="IBindableCollection{TElement}" /> to aggregate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="func">An accumulator function to be invoked on each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <returns>The final accumulator value.</returns>
        public static IBindable<TAccumulate> Aggregate<TSource, TAccumulate>(this IBindableCollection<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func)
        {
            return source.Aggregate(seed, func, DefaultDependencyAnalysis);
        }

        /// <summary>
        /// Applies an accumulator function over a sequence. The specified seed value is used as the initial accumulator value, and the specified function is used to select the result value.
        /// </summary>
        /// <param name="source">An <see cref="IBindableCollection{TElement}" /> to aggregate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="func">An accumulator function to be invoked on each element.</param>
        /// <param name="resultSelector">A function to transform the final accumulator value into the result value.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <returns>The transformed final accumulator value.</returns>
        public static IBindable<TResult> Aggregate<TSource, TAccumulate, TResult>(this IBindableCollection<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> resultSelector)
        {
            return source.Aggregate(seed, func, resultSelector, DefaultDependencyAnalysis);
        }

        /// <summary>
        /// Applies an accumulator function over a sequence. The specified seed value is used as the initial accumulator value, and the specified function is used to select the result value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TElement}"/> to aggregate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="func">An accumulator function to be invoked on each element.</param>
        /// <param name="resultSelector">A function to transform the final accumulator value into the result value.</param>
        /// <param name="dependencyAnalysisMode">The dependency analysis mode.</param>
        /// <returns>The transformed final accumulator value.</returns>
        public static IBindable<TResult> Aggregate<TSource, TAccumulate, TResult>(this IBindableCollection<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> resultSelector, DependencyDiscovery dependencyAnalysisMode)
        {
            return source.Aggregate(seed, func, dependencyAnalysisMode).Project(resultSelector, dependencyAnalysisMode);
        }

        /// <summary>
        /// Applies an accumulator function over a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TSource}"/> to aggregate over.</param>
        /// <param name="func">An accumulator function to be invoked on each element.</param>
        /// <param name="dependencyAnalysisMode">The dependency analysis mode.</param>
        /// <returns>The final accumulator value.</returns>
        public static IBindable<TResult> Aggregate<TSource, TResult>(this IBindableCollection<TSource> source, Expression<Func<IBindableCollection<TSource>, TResult>> func, DependencyDiscovery dependencyAnalysisMode)
        {
            source.ShouldNotBeNull("source");
            func.ShouldNotBeNull("func");
            var result = new CustomAggregator<TSource, TResult>(source, func.Compile(), source.Dispatcher);
            if (dependencyAnalysisMode == DependencyDiscovery.Enabled)
            {
                return result.DependsOnExpression(func, func.Parameters[0]);
            }
            return result;
        }

        /// <summary>
        /// Applies an accumulator function over a sequence. The specified seed value is used as the
        /// initial accumulator value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TElement}"/> to aggregate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="func">An accumulator function to be invoked on each element.</param>
        /// <param name="dependencyAnalysisMode">The dependency analysis mode.</param>
        /// <returns>The final accumulator value.</returns>
        public static IBindable<TAccumulate> Aggregate<TSource, TAccumulate>(this IBindableCollection<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func, DependencyDiscovery dependencyAnalysisMode)
        {
            source.ShouldNotBeNull("source");
            func.ShouldNotBeNull("func");
            seed.ShouldNotBeNull("seed");

            var compiledAccumulator = func.Compile();
            var function = new Func<IBindableCollection<TSource>, TAccumulate>(
                sourceElements =>
                {
                    var current = seed;
                    foreach (var sourceElement in sourceElements)
                    {
                        current = compiledAccumulator(current, sourceElement);
                    }
                    return current;
                }
                );

            var result = new CustomAggregator<TSource, TAccumulate>(source, function, source.Dispatcher);
            if (dependencyAnalysisMode == DependencyDiscovery.Enabled)
            {
                return result.DependsOnExpression(func, func.Parameters[1]);
            }
            return result;
        }
	}
}
