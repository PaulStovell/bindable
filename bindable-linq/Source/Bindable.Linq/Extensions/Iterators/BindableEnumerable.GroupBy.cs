using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Bindable.Core.Helpers;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Iterators;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TElement}"/> whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <returns>
        /// An IEnumerable&lt;IGrouping&lt;TKey, TSource&gt;&gt; in C# or IEnumerable(Of IGrouping(Of TKey, TSource)) in Visual Basic where each <see cref="T:System.Linq.IGrouping`2"/> object contains a sequence of objects and a key.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
	    public static IBindableCollection<IBindableGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector) where TSource : class
	    {
	        return GroupBy(source, keySelector, DefaultDependencyAnalysis);
	    }

	    /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TElement}"/> whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="dependencyAnalysisMode">The dependency analysis mode.</param>
        /// <returns>
        /// An IEnumerable&lt;IGrouping&lt;TKey, TSource&gt;&gt; in C# or IEnumerable(Of IGrouping(Of TKey, TSource)) in Visual Basic where each <see cref="T:System.Linq.IGrouping`2"/> object contains a sequence of objects and a key.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IBindableCollection<IBindableGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, DependencyDiscovery dependencyAnalysisMode) where TSource : class
        {
            return source.GroupBy(keySelector, s => s, new DefaultComparer<TKey>(), dependencyAnalysisMode);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function and compares the keys by using a specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TElement}"/> whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> to compare keys.</param>
        /// <returns>
        /// An IEnumerable&lt;IGrouping&lt;TKey, TSource&gt;&gt; in C# or IEnumerable(Of IGrouping(Of TKey, TSource)) in Visual Basic where each <see cref="T:System.Linq.IGrouping`2"/> object contains a collection of objects and a key.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
	    public static IBindableCollection<IBindableGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer) where TSource : class
	    {
	        return GroupBy(source, keySelector, comparer, DefaultDependencyAnalysis);
	    }

	    /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function and compares the keys by using a specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TElement}"/> whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> to compare keys.</param>
        /// <param name="dependencyAnalysisMode">The dependency analysis mode.</param>
        /// <returns>
        /// An IEnumerable&lt;IGrouping&lt;TKey, TSource&gt;&gt; in C# or IEnumerable(Of IGrouping(Of TKey, TSource)) in Visual Basic where each <see cref="T:System.Linq.IGrouping`2"/> object contains a collection of objects and a key.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IBindableCollection<IBindableGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer, DependencyDiscovery dependencyAnalysisMode) where TSource : class
        {
            return source.GroupBy(keySelector, s => s, comparer, dependencyAnalysisMode);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function and projects the elements for each group by using a specified function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <typeparam name="TElement">The type of the elements in the <see cref="T:System.Linq.IGrouping`2"/>.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TElement}"/> whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in the <see cref="T:System.Linq.IGrouping`2"/>.</param>
        /// <returns>
        /// An IEnumerable&lt;IGrouping&lt;TKey, TElement&gt;&gt; in C# or IEnumerable(Of IGrouping(Of TKey, TElement)) in Visual Basic where each <see cref="T:System.Linq.IGrouping`2"/> object contains a collection of objects of type TElement and a key.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> is null.</exception>
	    public static IBindableCollection<IBindableGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector)
	        where TSource : class
	        where TElement : class
	    {
	        return GroupBy(source, keySelector, elementSelector, DefaultDependencyAnalysis);
	    }

	    /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function and projects the elements for each group by using a specified function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <typeparam name="TElement">The type of the elements in the <see cref="T:System.Linq.IGrouping`2"/>.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TElement}"/> whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in the <see cref="T:System.Linq.IGrouping`2"/>.</param>
        /// <param name="dependencyAnalysisMode">The dependency analysis mode.</param>
        /// <returns>
        /// An IEnumerable&lt;IGrouping&lt;TKey, TElement&gt;&gt; in C# or IEnumerable(Of IGrouping(Of TKey, TElement)) in Visual Basic where each <see cref="T:System.Linq.IGrouping`2"/> object contains a collection of objects of type TElement and a key.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> is null.</exception>
        public static IBindableCollection<IBindableGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, DependencyDiscovery dependencyAnalysisMode)
            where TSource : class
            where TElement : class
        {
            return source.GroupBy(keySelector, elementSelector, null, dependencyAnalysisMode);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result value returned by <paramref name="resultSelector"/>.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TElement}"/> whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="resultSelector">A function to create a result value from each group.</param>
        /// <returns>
        /// A collection of elements of type TResult where each element represents a projection over a group and its key.
        /// </returns>
	    public static IBindableCollection<TResult> GroupBy<TSource, TKey, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IBindableCollection<TSource>, TResult>> resultSelector)
	        where TSource : class
	        where TResult : class
	    {
	        return GroupBy(source, keySelector, resultSelector, DefaultDependencyAnalysis);
	    }

	    /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result value returned by <paramref name="resultSelector"/>.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TElement}"/> whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="resultSelector">A function to create a result value from each group.</param>
        /// <param name="dependencyAnalysisMode">The dependency analysis mode.</param>
        /// <returns>
        /// A collection of elements of type TResult where each element represents a projection over a group and its key.
        /// </returns>
        public static IBindableCollection<TResult> GroupBy<TSource, TKey, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IBindableCollection<TSource>, TResult>> resultSelector, DependencyDiscovery dependencyAnalysisMode)
            where TSource : class
            where TResult : class
        {
            return source.GroupBy(keySelector, s => s, new DefaultComparer<TKey>(), dependencyAnalysisMode).Into(resultSelector);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a key selector function. The keys are compared by using a comparer and each group's elements are projected by using a specified function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <typeparam name="TElement">The type of the elements in the <see cref="T:System.Linq.IGrouping`2"/>.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TElement}"/> whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in an <see cref="T:System.Linq.IGrouping`2"/>.</param>
        /// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> to compare keys.</param>
        /// <returns>
        /// An IEnumerable&lt;IGrouping&lt;TKey, TElement&gt;&gt; in C# or IEnumerable(Of IGrouping(Of TKey, TElement)) in Visual Basic where each <see cref="T:System.Linq.IGrouping`2"/> object contains a collection of objects of type TElement and a key.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> is null.</exception>
	    public static IBindableCollection<IBindableGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> comparer)
	        where TSource : class
	        where TElement : class
	    {
	        return GroupBy(source, keySelector, elementSelector, comparer, DefaultDependencyAnalysis);
	    }

	    /// <summary>
        /// Groups the elements of a sequence according to a key selector function. The keys are compared by using a comparer and each group's elements are projected by using a specified function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <typeparam name="TElement">The type of the elements in the <see cref="T:System.Linq.IGrouping`2"/>.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TElement}"/> whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in an <see cref="T:System.Linq.IGrouping`2"/>.</param>
        /// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> to compare keys.</param>
        /// <param name="dependencyAnalysisMode">The dependency analysis mode.</param>
        /// <returns>
        /// An IEnumerable&lt;IGrouping&lt;TKey, TElement&gt;&gt; in C# or IEnumerable(Of IGrouping(Of TKey, TElement)) in Visual Basic where each <see cref="T:System.Linq.IGrouping`2"/> object contains a collection of objects of type TElement and a key.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> is null.</exception>
        public static IBindableCollection<IBindableGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> comparer, DependencyDiscovery dependencyAnalysisMode)
            where TSource : class
            where TElement : class
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(keySelector, "keySelector");
            Guard.NotNull(elementSelector, "elementSelector");
            var result = new GroupByIterator<TKey, TSource, TElement>(source, keySelector, elementSelector, comparer, source.Dispatcher);
            if (dependencyAnalysisMode == DependencyDiscovery.Enabled)
            {
                return result.DependsOnExpression(keySelector.Body, keySelector.Parameters[0]);
            }
            return result;
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key. The elements of each group are projected by using a specified function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <typeparam name="TElement">The type of the elements in each <see cref="T:System.Linq.IGrouping`2"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result value returned by <paramref name="resultSelector"/>.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TElement}"/> whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in an <see cref="T:System.Linq.IGrouping`2"/>.</param>
        /// <param name="resultSelector">A function to create a result value from each group.</param>
        /// <returns>
        /// A collection of elements of type TResult where each element represents a projection over a group and its key.
        /// </returns>
	    public static IBindableCollection<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IBindableCollection<TElement>, TResult>> resultSelector)
	        where TSource : class
	        where TElement : class
	        where TResult : class
	    {
	        return GroupBy(source, keySelector, elementSelector, resultSelector, DefaultDependencyAnalysis);
	    }

	    /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key. The elements of each group are projected by using a specified function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <typeparam name="TElement">The type of the elements in each <see cref="T:System.Linq.IGrouping`2"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result value returned by <paramref name="resultSelector"/>.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TElement}"/> whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in an <see cref="T:System.Linq.IGrouping`2"/>.</param>
        /// <param name="resultSelector">A function to create a result value from each group.</param>
        /// <param name="dependencyAnalysisMode">The dependency analysis mode.</param>
        /// <returns>
        /// A collection of elements of type TResult where each element represents a projection over a group and its key.
        /// </returns>
        public static IBindableCollection<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IBindableCollection<TElement>, TResult>> resultSelector, DependencyDiscovery dependencyAnalysisMode)
            where TSource : class
            where TElement : class
            where TResult : class
        {
            return source.GroupBy(keySelector, elementSelector, null, dependencyAnalysisMode).Into(resultSelector);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key. The keys are compared by using a specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result value returned by <paramref name="resultSelector"/>.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TElement}"/> whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="resultSelector">A function to create a result value from each group.</param>
        /// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> to compare keys with.</param>
        /// <returns>
        /// A collection of elements of type TResult where each element represents a projection over a group and its key.
        /// </returns>
	    public static IBindableCollection<TResult> GroupBy<TSource, TKey, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IBindableCollection<TSource>, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
	        where TSource : class
	        where TResult : class
	    {
	        return GroupBy(source, keySelector, resultSelector, comparer, DefaultDependencyAnalysis);
	    }

	    /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key. The keys are compared by using a specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result value returned by <paramref name="resultSelector"/>.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TElement}"/> whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="resultSelector">A function to create a result value from each group.</param>
        /// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> to compare keys with.</param>
        /// <param name="dependencyAnalysisMode">The dependency analysis mode.</param>
        /// <returns>
        /// A collection of elements of type TResult where each element represents a projection over a group and its key.
        /// </returns>
        public static IBindableCollection<TResult> GroupBy<TSource, TKey, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IBindableCollection<TSource>, TResult>> resultSelector, IEqualityComparer<TKey> comparer, DependencyDiscovery dependencyAnalysisMode)
            where TSource : class
            where TResult : class
        {
            return source.GroupBy(keySelector, s => s, comparer, dependencyAnalysisMode).Into(resultSelector);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key. Key values are compared by using a specified comparer, and the elements of each group are projected by using a specified function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <typeparam name="TElement">The type of the elements in each <see cref="T:System.Linq.IGrouping`2"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result value returned by <paramref name="resultSelector"/>.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TElement}"/> whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in an <see cref="T:System.Linq.IGrouping`2"/>.</param>
        /// <param name="resultSelector">A function to create a result value from each group.</param>
        /// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> to compare keys with.</param>
        /// <returns>
        /// A collection of elements of type TResult where each element represents a projection over a group and its key.
        /// </returns>
	    public static IBindableCollection<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IBindableCollection<TElement>, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
	        where TSource : class
	        where TElement : class
	        where TResult : class
	    {
	        return GroupBy(source, keySelector, elementSelector, resultSelector, comparer, DefaultDependencyAnalysis);
	    }

	    /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key. Key values are compared by using a specified comparer, and the elements of each group are projected by using a specified function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <typeparam name="TElement">The type of the elements in each <see cref="T:System.Linq.IGrouping`2"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result value returned by <paramref name="resultSelector"/>.</typeparam>
        /// <param name="source">An <see cref="IBindableCollection{TElement}"/> whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in an <see cref="T:System.Linq.IGrouping`2"/>.</param>
        /// <param name="resultSelector">A function to create a result value from each group.</param>
        /// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> to compare keys with.</param>
        /// <param name="dependencyAnalysisMode">The dependency analysis mode.</param>
        /// <returns>
        /// A collection of elements of type TResult where each element represents a projection over a group and its key.
        /// </returns>
        public static IBindableCollection<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IBindableCollection<TElement>, TResult>> resultSelector, IEqualityComparer<TKey> comparer, DependencyDiscovery dependencyAnalysisMode)
            where TSource : class
            where TElement : class
            where TResult : class
        {
            return source.GroupBy(keySelector, elementSelector, comparer, dependencyAnalysisMode).Into(resultSelector);
        }
	}
}
