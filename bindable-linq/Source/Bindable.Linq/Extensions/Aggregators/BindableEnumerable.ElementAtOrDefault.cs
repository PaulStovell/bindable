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
        /// <summary>Returns the element at a specified index in a sequence or a default value if the index is out of range.</summary>
        /// <returns>default(<typeparamref name="TSource" />) if the index is outside the bounds of the source sequence; otherwise, the element at the specified position in the source sequence.</returns>
        /// <param name="source">An <see cref="IBindableCollection{TElement}" /> to return an element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        public static IBindable<TSource> ElementAtOrDefault<TSource>(this IBindableCollection<TSource> source, int index)
        {
            source.ShouldNotBeNull("source");
            return new ElementAtAggregator<TSource>(source, index, source.Dispatcher);
        }
	}
}
