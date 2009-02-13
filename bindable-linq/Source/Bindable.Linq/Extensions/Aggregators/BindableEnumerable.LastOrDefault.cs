using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Threading;
using Bindable.Linq.Adapters.Incoming;
using Bindable.Linq.Adapters.Outgoing;
using Bindable.Linq.Aggregators;
using Bindable.Linq.Aggregators.Numerics;
using Bindable.Linq.Collections;
using Bindable.Linq.Configuration;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Dependencies.Definitions;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Iterators;
using Bindable.Linq.Operators;
using Bindable.Core.Threading;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>Returns the last element of a sequence, or a default value if the sequence contains no elements.</summary>
        /// <returns>default(<typeparamref name="TSource" />) if the source sequence is empty; otherwise, the last element in the <see cref="IBindableCollection{TElement}" />.</returns>
        /// <param name="source">An <see cref="IBindableCollection{TElement}" /> to return the last element of.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        public static IBindable<TSource> LastOrDefault<TSource>(this IBindableCollection<TSource> source)
        {
            return source.ElementAtOrDefault(-1);
        }

        /// <summary>Returns the last element of a sequence that satisfies a condition or a default value if no such element is found.</summary>
        /// <returns>default(<typeparamref name="TSource" />) if the sequence is empty or if no elements pass the test in the predicate function; otherwise, the last element that passes the test in the predicate function.</returns>
        /// <param name="source">An <see cref="IBindableCollection{TElement}" /> to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
        public static IBindable<TSource> LastOrDefault<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, bool>> predicate) where TSource : class
        {
            return source.Where(predicate).LastOrDefault();
        }
	}
}
