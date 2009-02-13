using System;
using System.Linq.Expressions;
using Bindable.Linq.Aggregators.Numerics;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        private static IBindable<TElement> SumInternal<TElement, TAverage>(IBindableCollection<TElement> source, INumeric<TElement, TAverage> numeric)
        {
            source.ShouldNotBeNull("source");
            return Aggregate<TElement>(source, sources => numeric.Sum(sources));
        }

        /// <summary>Computes the sum of a sequence of <see cref="T:System.Double" /> values.</summary>
        /// <returns>The sum of the values in the sequence.</returns>
        /// <param name="source">A sequence of <see cref="T:System.Double" /> values to calculate the sum of.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        public static IBindable<double> Sum(this IBindableCollection<double> source)
        {
            return SumInternal(source, new DoubleNumeric());
        }

        /// <summary>Computes the sum of a sequence of nullable <see cref="T:System.Decimal" /> values.</summary>
        /// <returns>The sum of the values in the sequence.</returns>
        /// <param name="source">A sequence of nullable <see cref="T:System.Decimal" /> values to calculate the sum of.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Decimal.MaxValue" />.</exception>
        public static IBindable<decimal?> Sum(this IBindableCollection<decimal?> source)
        {
            return SumInternal(source, new DecimalNullNumeric());
        }

        /// <summary>Computes the sum of a sequence of <see cref="T:System.Decimal" /> values.</summary>
        /// <returns>The sum of the values in the sequence.</returns>
        /// <param name="source">A sequence of <see cref="T:System.Decimal" /> values to calculate the sum of.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Decimal.MaxValue" />.</exception>
        public static IBindable<decimal> Sum(this IBindableCollection<decimal> source)
        {
            return SumInternal(source, new DecimalNumeric());
        }

        /// <summary>Computes the sum of a sequence of <see cref="T:System.Int32" /> values.</summary>
        /// <returns>The sum of the values in the sequence.</returns>
        /// <param name="source">A sequence of <see cref="T:System.Int32" /> values to calculate the sum of.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
        public static IBindable<int> Sum(this IBindableCollection<int> source)
        {
            return SumInternal(source, new Int32Numeric());
        }

        /// <summary>Computes the sum of a sequence of <see cref="T:System.Int64" /> values.</summary>
        /// <returns>The sum of the values in the sequence.</returns>
        /// <param name="source">A sequence of <see cref="T:System.Int64" /> values to calculate the sum of.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int64.MaxValue" />.</exception>
        public static IBindable<long> Sum(this IBindableCollection<long> source)
        {
            return SumInternal(source, new Int64Numeric());
        }

        /// <summary>Computes the sum of a sequence of nullable <see cref="T:System.Double" /> values.</summary>
        /// <returns>The sum of the values in the sequence.</returns>
        /// <param name="source">A sequence of nullable <see cref="T:System.Double" /> values to calculate the sum of.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        public static IBindable<double?> Sum(this IBindableCollection<double?> source)
        {
            return SumInternal(source, new DoubleNullNumeric());
        }

        /// <summary>Computes the sum of a sequence of nullable <see cref="T:System.Int32" /> values.</summary>
        /// <returns>The sum of the values in the sequence.</returns>
        /// <param name="source">A sequence of nullable <see cref="T:System.Int32" /> values to calculate the sum of.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
        public static IBindable<int?> Sum(this IBindableCollection<int?> source)
        {
            return SumInternal(source, new Int32NullNumeric());
        }

        /// <summary>Computes the sum of a sequence of nullable <see cref="T:System.Int64" /> values.</summary>
        /// <returns>The sum of the values in the sequence.</returns>
        /// <param name="source">A sequence of nullable <see cref="T:System.Int64" /> values to calculate the sum of.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int64.MaxValue" />.</exception>
        public static IBindable<long?> Sum(this IBindableCollection<long?> source)
        {
            return SumInternal(source, new Int64NullNumeric());
        }

        /// <summary>Computes the sum of a sequence of nullable <see cref="T:System.Single" /> values.</summary>
        /// <returns>The sum of the values in the sequence.</returns>
        /// <param name="source">A sequence of nullable <see cref="T:System.Single" /> values to calculate the sum of.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        public static IBindable<float?> Sum(this IBindableCollection<float?> source)
        {
            return SumInternal(source, new FloatNullNumeric());
        }

        /// <summary>Computes the sum of a sequence of <see cref="T:System.Single" /> values.</summary>
        /// <returns>The sum of the values in the sequence.</returns>
        /// <param name="source">A sequence of <see cref="T:System.Single" /> values to calculate the sum of.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        public static IBindable<float> Sum(this IBindableCollection<float> source)
        {
            return SumInternal(source, new FloatNumeric());
        }

        /// <summary>Computes the sum of the sequence of nullable <see cref="T:System.Decimal" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
        /// <returns>The sum of the projected values.</returns>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Decimal.MaxValue" />.</exception>
        public static IBindable<decimal?> Sum<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, decimal?>> selector) where TSource : class
        {
            return source.Select(selector).Sum();
        }

        /// <summary>Computes the sum of the sequence of nullable <see cref="T:System.Double" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
        /// <returns>The sum of the projected values.</returns>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        public static IBindable<double?> Sum<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, double?>> selector) where TSource : class
        {
            return source.Select(selector).Sum();
        }

        /// <summary>Computes the sum of the sequence of <see cref="T:System.Decimal" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
        /// <returns>The sum of the projected values.</returns>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Decimal.MaxValue" />.</exception>
        public static IBindable<decimal> Sum<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, decimal>> selector) where TSource : class
        {
            return source.Select(selector).Sum();
        }

        /// <summary>Computes the sum of the sequence of nullable <see cref="T:System.Int32" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
        /// <returns>The sum of the projected values.</returns>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
        public static IBindable<int?> Sum<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, int?>> selector) where TSource : class
        {
            return source.Select(selector).Sum();
        }

        /// <summary>Computes the sum of the sequence of <see cref="T:System.Double" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
        /// <returns>The sum of the projected values.</returns>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        public static IBindable<double> Sum<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, double>> selector) where TSource : class
        {
            return source.Select(selector).Sum();
        }

        /// <summary>Computes the sum of the sequence of <see cref="T:System.Int32" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
        /// <returns>The sum of the projected values.</returns>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
        public static IBindable<int> Sum<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, int>> selector) where TSource : class
        {
            return source.Select(selector).Sum();
        }

        /// <summary>Computes the sum of the sequence of <see cref="T:System.Int64" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
        /// <returns>The sum of the projected values.</returns>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int64.MaxValue" />.</exception>
        public static IBindable<long> Sum<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, long>> selector) where TSource : class
        {
            return source.Select(selector).Sum();
        }

        /// <summary>Computes the sum of the sequence of nullable <see cref="T:System.Int64" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
        /// <returns>The sum of the projected values.</returns>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int64.MaxValue" />.</exception>
        public static IBindable<long?> Sum<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, long?>> selector) where TSource : class
        {
            return source.Select(selector).Sum();
        }

        /// <summary>Computes the sum of the sequence of nullable <see cref="T:System.Single" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
        /// <returns>The sum of the projected values.</returns>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        public static IBindable<float?> Sum<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, float?>> selector) where TSource : class
        {
            return source.Select(selector).Sum();
        }

        /// <summary>Computes the sum of the sequence of <see cref="T:System.Single" /> values that are obtained by invoking a transform function on each element of the input sequence.</summary>
        /// <returns>The sum of the projected values.</returns>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        public static IBindable<float> Sum<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, float>> selector) where TSource : class
        {
            return source.Select(selector).Sum();
        }
	}
}
