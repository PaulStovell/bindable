using System;
using System.Linq.Expressions;
using Bindable.Linq.Aggregators.Numerics;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        private static IBindable<TElement> MaxInternal<TElement, TAverage>(IBindableCollection<TElement> source, INumeric<TElement, TAverage> numeric)
        {
            source.ShouldNotBeNull("source");
            return Aggregate<TElement, TElement>(source, sources => numeric.Max(sources));
        }

        /// <summary>Returns the maximum value in a sequence of <see cref="T:System.Decimal" /> values.</summary>
        /// <returns>The maximum value in the sequence.</returns>
        /// <param name="source">A sequence of <see cref="T:System.Decimal" /> values to determine the maximum value of.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source" /> contains no elements.</exception>
        public static IBindable<decimal> Max(this IBindableCollection<decimal> source)
        {
            return MaxInternal(source, new DecimalNumeric());
        }

        /// <summary>Returns the maximum value in a sequence of nullable <see cref="T:System.Double" /> values.</summary>
        /// <returns>A value of type Nullable&lt;Double&gt; in C# or Nullable(Of Double) in Visual Basic that corresponds to the maximum value in the sequence.</returns>
        /// <param name="source">A sequence of nullable <see cref="T:System.Double" /> values to determine the maximum value of.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        public static IBindable<double?> Max(this IBindableCollection<double?> source)
        {
            return MaxInternal(source, new DoubleNullNumeric());
        }

        /// <summary>Returns the maximum value in a sequence of <see cref="T:System.Double" /> values.</summary>
        /// <returns>The maximum value in the sequence.</returns>
        /// <param name="source">A sequence of <see cref="T:System.Double" /> values to determine the maximum value of.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source" /> contains no elements.</exception>
        public static IBindable<double> Max(this IBindableCollection<double> source)
        {
            return MaxInternal(source, new DoubleNumeric());
        }

        /// <summary>Returns the maximum value in a sequence of nullable <see cref="T:System.Int32" /> values.</summary>
        /// <returns>A value of type Nullable&lt;Int32&gt; in C# or Nullable(Of Int32) in Visual Basic that corresponds to the maximum value in the sequence. </returns>
        /// <param name="source">A sequence of nullable <see cref="T:System.Int32" /> values to determine the maximum value of.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        public static IBindable<int?> Max(this IBindableCollection<int?> source)
        {
            return MaxInternal(source, new Int32NullNumeric());
        }

        /// <summary>Returns the maximum value in a sequence of nullable <see cref="T:System.Int64" /> values.</summary>
        /// <returns>A value of type Nullable&lt;Int64&gt; in C# or Nullable(Of Int64) in Visual Basic that corresponds to the maximum value in the sequence. </returns>
        /// <param name="source">A sequence of nullable <see cref="T:System.Int64" /> values to determine the maximum value of.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        public static IBindable<long?> Max(this IBindableCollection<long?> source)
        {
            return MaxInternal(source, new Int64NullNumeric());
        }

        /// <summary>Returns the maximum value in a sequence of <see cref="T:System.Int32" /> values.</summary>
        /// <returns>The maximum value in the sequence.</returns>
        /// <param name="source">A sequence of <see cref="T:System.Int32" /> values to determine the maximum value of.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source" /> contains no elements.</exception>
        public static IBindable<int> Max(this IBindableCollection<int> source)
        {
            return MaxInternal(source, new Int32Numeric());
        }

        /// <summary>Returns the maximum value in a sequence of nullable <see cref="T:System.Decimal" /> values.</summary>
        /// <returns>A value of type Nullable&lt;Decimal&gt; in C# or Nullable(Of Decimal) in Visual Basic that corresponds to the maximum value in the sequence. </returns>
        /// <param name="source">A sequence of nullable <see cref="T:System.Decimal" /> values to determine the maximum value of.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        public static IBindable<decimal?> Max(this IBindableCollection<decimal?> source)
        {
            return MaxInternal(source, new DecimalNullNumeric());
        }

        /// <summary>Returns the maximum value in a sequence of <see cref="T:System.Int64" /> values.</summary>
        /// <returns>The maximum value in the sequence.</returns>
        /// <param name="source">A sequence of <see cref="T:System.Int64" /> values to determine the maximum value of.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source" /> contains no elements.</exception>
        public static IBindable<long> Max(this IBindableCollection<long> source)
        {
            return MaxInternal(source, new Int64Numeric());
        }

        /// <summary>Returns the maximum value in a sequence of nullable <see cref="T:System.Single" /> values.</summary>
        /// <returns>A value of type Nullable&lt;Single&gt; in C# or Nullable(Of Single) in Visual Basic that corresponds to the maximum value in the sequence.</returns>
        /// <param name="source">A sequence of nullable <see cref="T:System.Single" /> values to determine the maximum value of.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        public static IBindable<float?> Max(this IBindableCollection<float?> source)
        {
            return MaxInternal(source, new FloatNullNumeric());
        }

        /// <summary>Returns the maximum value in a sequence of <see cref="T:System.Single" /> values.</summary>
        /// <returns>The maximum value in the sequence.</returns>
        /// <param name="source">A sequence of <see cref="T:System.Single" /> values to determine the maximum value of.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source" /> contains no elements.</exception>
        public static IBindable<float> Max(this IBindableCollection<float> source)
        {
            return MaxInternal(source, new FloatNumeric());
        }

        /// <summary>Invokes a transform function on each element of a sequence and returns the maximum <see cref="T:System.Decimal" /> value.</summary>
        /// <returns>The maximum value in the sequence.</returns>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source" /> contains no elements.</exception>
        public static IBindable<decimal> Max<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, decimal>> selector) where TSource : class
        {
            source.ShouldNotBeNull("source");
            return source.Select(selector).Max();
        }

        /// <summary>Invokes a transform function on each element of a sequence and returns the maximum <see cref="T:System.Double" /> value.</summary>
        /// <returns>The maximum value in the sequence.</returns>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source" /> contains no elements.</exception>
        public static IBindable<double> Max<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, double>> selector) where TSource : class
        {
            source.ShouldNotBeNull("source");
            return source.Select(selector).Max();
        }

        /// <summary>Invokes a transform function on each element of a sequence and returns the maximum nullable <see cref="T:System.Decimal" /> value.</summary>
        /// <returns>The value of type Nullable&lt;Decimal&gt; in C# or Nullable(Of Decimal) in Visual Basic that corresponds to the maximum value in the sequence.</returns>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        public static IBindable<decimal?> Max<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, decimal?>> selector) where TSource : class
        {
            source.ShouldNotBeNull("source");
            return source.Select(selector).Max();
        }

        /// <summary>Invokes a transform function on each element of a sequence and returns the maximum nullable <see cref="T:System.Double" /> value.</summary>
        /// <returns>The value of type Nullable&lt;Double&gt; in C# or Nullable(Of Double) in Visual Basic that corresponds to the maximum value in the sequence.</returns>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        public static IBindable<double?> Max<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, double?>> selector) where TSource : class
        {
            source.ShouldNotBeNull("source");
            return source.Select(selector).Max();
        }

        /// <summary>Invokes a transform function on each element of a sequence and returns the maximum <see cref="T:System.Int32" /> value.</summary>
        /// <returns>The maximum value in the sequence.</returns>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source" /> contains no elements.</exception>
        public static IBindable<int> Max<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, int>> selector) where TSource : class
        {
            source.ShouldNotBeNull("source");
            return source.Select(selector).Max();
        }

        /// <summary>Invokes a transform function on each element of a sequence and returns the maximum nullable <see cref="T:System.Int32" /> value.</summary>
        /// <returns>The value of type Nullable&lt;Int32&gt; in C# or Nullable(Of Int32) in Visual Basic that corresponds to the maximum value in the sequence.</returns>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        public static IBindable<int?> Max<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, int?>> selector) where TSource : class
        {
            source.ShouldNotBeNull("source");
            return source.Select(selector).Max();
        }

        /// <summary>Invokes a transform function on each element of a sequence and returns the maximum nullable <see cref="T:System.Int64" /> value.</summary>
        /// <returns>The value of type Nullable&lt;Int64&gt; in C# or Nullable(Of Int64) in Visual Basic that corresponds to the maximum value in the sequence.</returns>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        public static IBindable<long?> Max<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, long?>> selector) where TSource : class
        {
            source.ShouldNotBeNull("source");
            return source.Select(selector).Max();
        }

        /// <summary>Invokes a transform function on each element of a sequence and returns the maximum <see cref="T:System.Int64" /> value.</summary>
        /// <returns>The maximum value in the sequence.</returns>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source" /> contains no elements.</exception>
        public static IBindable<long> Max<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, long>> selector) where TSource : class
        {
            source.ShouldNotBeNull("source");
            return source.Select(selector).Max();
        }

        /// <summary>Invokes a transform function on each element of a sequence and returns the maximum nullable <see cref="T:System.Single" /> value.</summary>
        /// <returns>The value of type Nullable&lt;Single&gt; in C# or Nullable(Of Single) in Visual Basic that corresponds to the maximum value in the sequence.</returns>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        public static IBindable<float?> Max<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, float?>> selector) where TSource : class
        {
            source.ShouldNotBeNull("source");
            return source.Select(selector).Max();
        }

        /// <summary>Invokes a transform function on each element of a sequence and returns the maximum <see cref="T:System.Single" /> value.</summary>
        /// <returns>The maximum value in the sequence.</returns>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source" /> contains no elements.</exception>
        public static IBindable<float> Max<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, float>> selector) where TSource : class
        {
            source.ShouldNotBeNull("source");
            return source.Select(selector).Max();
        }
	}
}
