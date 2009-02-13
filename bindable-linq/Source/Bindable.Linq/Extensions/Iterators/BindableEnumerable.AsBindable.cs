using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Threading;
using Bindable.Linq.Adapters.Incoming;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Core.Threading;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
#if !SILVERLIGHT
        /// <summary>
        /// Converts any <see cref="IEnumerable{T}"/> into a Bindable LINQ <see cref="IBindableCollection{TElement}"/>. The <see cref="IBindableCollection{TElement}"/>
        /// interface is what allows the Bindable LINQ extensions to know whether to apply Bindable LINQ operations or the standard LINQ to Objects operations.
        /// </summary>
        /// <typeparam name="TSource">The type of source item.</typeparam>
        /// <param name="source">The source Iterator.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> containing the items.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <differences>This method is specific to Bindable LINQ and is not available in LINQ to Objects.</differences>
        /// <notimplemented>This method is not i    mplemented yet.</notimplemented>
        public static IBindableCollection<TSource> AsBindable<TSource>(this IEnumerable source) where TSource : class
        {
            return AsBindableInternal<TSource>(source, null);
        }

        /// <summary>
        /// Converts any <see cref="IEnumerable{T}"/> into a Bindable LINQ <see cref="IBindableCollection{TElement}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of source item.</typeparam>
        /// <param name="source">The source Iterator.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> containing the items.
        /// </returns>
        /// <differences>This method is specific to Bindable LINQ and is not available in LINQ to Objects.</differences>
        public static IBindableCollection<TSource> AsBindable<TSource>(this IEnumerable<TSource> source) where TSource : class
        {
            return AsBindableInternal<TSource>(source, null);
        }

        /// <summary>
        /// Converts any <see cref="IEnumerable{T}"/> into a Bindable LINQ <see cref="IBindableCollection{TElement}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of source item.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source Iterator.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> containing the items.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IBindableCollection<TResult> AsBindable<TSource, TResult>(this IEnumerable<TSource> source)
            where TResult : TSource
            where TSource : class
        {
            return AsBindable<TSource, TResult>(source, (IDispatcher)null);
        }
#endif

        /// <summary>
        /// Converts any <see cref="IEnumerable{T}"/> into a Bindable LINQ <see cref="IBindableCollection{TElement}"/>. The <see cref="IBindableCollection{TElement}"/>
        /// interface is what allows the Bindable LINQ extensions to know whether to apply Bindable LINQ operations or the standard LINQ to Objects operations.
        /// </summary>
        /// <typeparam name="TSource">The type of source item.</typeparam>
        /// <param name="source">The source Iterator.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> containing the items.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <differences>This method is specific to Bindable LINQ and is not available in LINQ to Objects.</differences>
        /// <notimplemented>This method is not implemented yet.</notimplemented>
        public static IBindableCollection<TSource> AsBindable<TSource>(this IEnumerable source, IDispatcher dispatcher) where TSource : class
        {
            return AsBindableInternal<TSource>(source, dispatcher);
        }

        /// <summary>
        /// Converts any <see cref="IEnumerable{T}"/> into a Bindable LINQ <see cref="IBindableCollection{TElement}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of source item.</typeparam>
        /// <param name="source">The source Iterator.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> containing the items.
        /// </returns>
        /// <differences>This method is specific to Bindable LINQ and is not available in LINQ to Objects.</differences>
        public static IBindableCollection<TSource> AsBindable<TSource>(this IEnumerable<TSource> source, IDispatcher dispatcher) where TSource : class
        {
            return AsBindableInternal<TSource>(source, dispatcher);
        }

        /// <summary>
        /// Converts any <see cref="IEnumerable{T}"/> into a Bindable LINQ <see cref="IBindableCollection{TElement}"/>. The <see cref="IBindableCollection{TElement}"/>
        /// interface is what allows the Bindable LINQ extensions to know whether to apply Bindable LINQ operations or the standard LINQ to Objects operations.
        /// </summary>
        /// <typeparam name="TSource">The type of source item.</typeparam>
        /// <param name="source">The source Iterator.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> containing the items.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <differences>This method is specific to Bindable LINQ and is not available in LINQ to Objects.</differences>
        /// <notimplemented>This method is not implemented yet.</notimplemented>
        public static IBindableCollection<TSource> AsBindable<TSource>(this IEnumerable source, Dispatcher dispatcher) where TSource : class
        {
            return AsBindableInternal<TSource>(source, DispatcherFactory.Create(dispatcher));
        }

        /// <summary>
        /// Converts any <see cref="IEnumerable{T}"/> into a Bindable LINQ <see cref="IBindableCollection{TElement}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of source item.</typeparam>
        /// <param name="source">The source Iterator.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> containing the items.
        /// </returns>
        /// <differences>This method is specific to Bindable LINQ and is not available in LINQ to Objects.</differences>
        public static IBindableCollection<TSource> AsBindable<TSource>(this IEnumerable<TSource> source, Dispatcher dispatcher) where TSource : class
        {
            return AsBindableInternal<TSource>(source, DispatcherFactory.Create(dispatcher));
        }

        /// <summary>
        /// Converts any <see cref="IEnumerable{T}"/> into a Bindable LINQ <see cref="IBindableCollection{TElement}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of source item.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source Iterator.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> containing the items.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IBindableCollection<TResult> AsBindable<TSource, TResult>(this IEnumerable<TSource> source, IDispatcher dispatcher)
            where TResult : TSource
            where TSource : class
        {
            return source.AsBindable(dispatcher).Where(e => e is TResult).Select(e => (TResult)e);
        }

        /// <summary>
        /// Converts any <see cref="IEnumerable{T}"/> into a Bindable LINQ <see cref="IBindableCollection{TElement}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of source item.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source Iterator.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <returns>
        /// An <see cref="IBindableCollection{TElement}"/> containing the items.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IBindableCollection<TResult> AsBindable<TSource, TResult>(this IEnumerable<TSource> source, Dispatcher dispatcher)
            where TResult : TSource
            where TSource : class
        {
            return AsBindable<TSource, TResult>(source, DispatcherFactory.Create(dispatcher));
        }

        private static IBindableCollection<TSource> AsBindableInternal<TSource>(this IEnumerable source, IDispatcher dispatcher) where TSource : class
        {
            source.ShouldNotBeNull("source");
#if SILVERLIGHT
            dispatcher.ShouldNotBeNull("dispatcher");
#else
            if (dispatcher == null)
            {
                dispatcher = DispatcherFactory.Create();
            }
#endif

            var alreadyBindable = source as IBindableCollection<TSource>;
            if (alreadyBindable != null)
            {
                return alreadyBindable;
            }
#if !SILVERLIGHT
            if (source is IBindingList && !(source is INotifyCollectionChanged))
            {
                return new BindingListToBindableCollectionAdapter<TSource>(source, dispatcher);
            }
#endif
            return new ObservableCollectionToBindableCollectionAdapter<TSource>(source, dispatcher);
        }
	}
}
