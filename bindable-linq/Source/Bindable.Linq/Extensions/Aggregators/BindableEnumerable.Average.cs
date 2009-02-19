using System;
using System.Linq.Expressions;
using Bindable.Core.Helpers;
using Bindable.Linq.Aggregators.Numerics;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        private static IBindable<TAverageResult> AverageInternal<TResult, TAverageResult>(IBindableCollection<TResult> source, INumeric<TResult, TAverageResult> numeric)
        {
            Guard.NotNull(source, "source");
            return Aggregate(source, sources => numeric.Average(sources));
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="T:System.Decimal" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="T:System.Decimal" /> values to calculate the average of.</param>
        /// <returns>The average of the sequence of values.</returns>
        public static IBindable<decimal> Average(this IBindableCollection<decimal> source)
        {
            return AverageInternal(source, new DecimalNumeric());
        }

        /// <summary>
        /// Computes the average of a sequence of nullable <see cref="T:System.Decimal" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="T:System.Decimal" /> values to calculate the average of.</param>
        /// <returns>The average of the sequence of values, or null if the source sequence is empty or contains only values that are null.</returns>
        public static IBindable<decimal?> Average(this IBindableCollection<decimal?> source)
        {
            return AverageInternal(source, new DecimalNullNumeric());
        }

        /// <summary>
        /// Computes the average of a sequence of nullable <see cref="T:System.Double" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="T:System.Double" /> values to calculate the average of.</param>
        /// <returns>The average of the sequence of values, or null if the source sequence is empty or contains only values that are null.</returns>
        public static IBindable<double?> Average(this IBindableCollection<double?> source)
        {
            return AverageInternal(source, new DoubleNullNumeric());
        }

        /// <summary>
        /// Computes the average of a sequence of nullable <see cref="T:System.Int32" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="T:System.Int32" />values to calculate the average of.</param>
        /// <returns>The average of the sequence of values, or null if the source sequence is empty or contains only values that are null.</returns>
        public static IBindable<double?> Average(this IBindableCollection<int?> source)
        {
            return AverageInternal(source, new Int32NullNumeric());
        }

        /// <summary>
        /// Computes the average of a sequence of nullable <see cref="T:System.Int64" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="T:System.Int64" /> values to calculate the average of.</param>
        /// <returns>The average of the sequence of values, or null if the source sequence is empty or contains only values that are null.</returns>
        public static IBindable<double?> Average(this IBindableCollection<long?> source)
        {
            return AverageInternal(source, new Int64NullNumeric());
        }

        /// <summary>
        /// Computes the average of a sequence of nullable <see cref="T:System.Single" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="T:System.Single" /> values to calculate the average of.</param>
        /// <returns>
        /// The average of the sequence of values, or null if the source sequence is empty or contains 
        /// only values that are null.
        /// </returns>
        public static IBindable<float?> Average(this IBindableCollection<float?> source)
        {
            return AverageInternal(source, new FloatNullNumeric());
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="T:System.Double" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="T:System.Double" /> values to calculate the average of.</param>
        /// <returns>
        /// The average of the sequence of values.
        /// </returns>
        public static IBindable<double> Average(this IBindableCollection<double> source)
        {
            return AverageInternal(source, new DoubleNumeric());
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="T:System.Int32" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="T:System.Int32" /> values to calculate the average of.</param>
        /// <returns>The average of the sequence of values.</returns>
        public static IBindable<double> Average(this IBindableCollection<int> source)
        {
            return AverageInternal(source, new Int32Numeric());
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="T:System.Int64" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="T:System.Int64" /> values to calculate the average of.</param>
        /// <returns>The average of the sequence of values.</returns>
        public static IBindable<double> Average(this IBindableCollection<long> source)
        {
            return AverageInternal(source, new Int64Numeric());
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="T:System.Single" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="T:System.Single" /> values to calculate the average of.</param>
        /// <returns>The average of the sequence of values.</returns>
        public static IBindable<float> Average(this IBindableCollection<float> source)
        {
            return AverageInternal(source, new FloatNumeric());
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="T:System.Decimal" /> values that are 
        /// obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="source">A sequence of values that are used to calculate an average.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <returns>The average of the sequence of values.</returns>
        public static IBindable<decimal> Average<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, decimal>> selector) where TSource : class
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="T:System.Double" /> values that are obtained 
        /// by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <returns>The average of the sequence of values.</returns>
        public static IBindable<double> Average<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, double>> selector) where TSource : class
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="T:System.Int32" /> values that are 
        /// obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <returns>The average of the sequence of values.</returns>
        public static IBindable<double> Average<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, int>> selector) where TSource : class
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="T:System.Int64" /> values that are obtained 
        /// by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <returns>The average of the sequence of values.</returns>
        public static IBindable<double> Average<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, long>> selector) where TSource : class
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// Computes the average of a sequence of nullable <see cref="T:System.Decimal" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <returns>The average of the sequence of values, or null if the source sequence is empty or contains only values that are null.</returns>
        public static IBindable<decimal?> Average<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, decimal?>> selector) where TSource : class
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// Computes the average of a sequence of nullable <see cref="T:System.Double" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <returns>The average of the sequence of values, or null if the source sequence is empty or contains only values that are null.</returns>
        public static IBindable<double?> Average<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, double?>> selector) where TSource : class
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// Computes the average of a sequence of nullable <see cref="T:System.Int32" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <returns>The average of the sequence of values, or null if the source sequence is empty or contains only values that are null.</returns>
        public static IBindable<double?> Average<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, int?>> selector) where TSource : class
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// Computes the average of a sequence of nullable <see cref="T:System.Int64" /> values that are 
        /// obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <returns>The average of the sequence of values, or null if the source sequence is empty or contains only values that are null.</returns>
        public static IBindable<double?> Average<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, long?>> selector) where TSource : class
        {
            return source.Select(selector).Average();
        }

        /// <summary>Computes the average of a sequence of nullable <see cref="T:System.Single" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
        /// <returns>The average of the sequence of values, or null if the source sequence is empty or contains only values that are null.</returns>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        public static IBindable<float?> Average<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, float?>> selector) where TSource : class
        {
            return source.Select(selector).Average();
        }

        /// <summary>Computes the average of a sequence of <see cref="T:System.Single" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
        /// <returns>The average of the sequence of values.</returns>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source" /> contains no elements.</exception>
        public static IBindable<float> Average<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, float>> selector) where TSource : class
        {
            return source.Select(selector).Average();
        }
	}
}
