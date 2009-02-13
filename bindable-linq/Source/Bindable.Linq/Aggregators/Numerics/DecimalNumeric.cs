using System.Collections.Generic;

namespace Bindable.Linq.Aggregators.Numerics
{
    /// <summary>
    /// Provides a numeric operations wrapper for decimal types.
    /// </summary>
    internal class DecimalNumeric : INumeric<decimal, decimal>
    {
        #region INumeric<decimal,decimal> Members
        /// <summary>
        /// Given a list of sourceCollection, adds them and returns the sum.
        /// </summary>
        /// <param name="sourceCollection">The sourceCollection to sum.</param>
        /// <returns></returns>
        public decimal Sum(IEnumerable<decimal> sourceCollection)
        {
            var result = default(decimal);
            if (sourceCollection != null)
            {
                foreach (var item in sourceCollection)
                {
                    result += item;
                }
            }
            return result;
        }

        /// <summary>
        /// Given a list of sourceCollection, computes the average value.
        /// </summary>
        /// <param name="sourceCollection">The sourceCollection to average.</param>
        /// <returns></returns>
        public decimal Average(IEnumerable<decimal> sourceCollection)
        {
            var result = default(decimal);

            var count = 0;
            if (sourceCollection != null)
            {
                foreach (var item in sourceCollection)
                {
                    result += item;
                    count++;
                }
            }

            if (count == 0)
            {
                return default(decimal);
            }
            else
            {
                return result / count;
            }
        }

        /// <summary>
        /// Given a list of sourceCollection, returns the lowest value.
        /// </summary>
        /// <param name="sourceCollection">The sourceCollection to locate the minimum value in.</param>
        /// <returns></returns>
        public decimal Min(IEnumerable<decimal> sourceCollection)
        {
            var result = default(decimal);

            var firstItem = true;
            if (sourceCollection != null)
            {
                foreach (var item in sourceCollection)
                {
                    if (firstItem || item < result)
                    {
                        result = item;
                        firstItem = false;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Given a list of sourceCollection, returns the highest value.
        /// </summary>
        /// <param name="sourceCollection">The sourceCollection to locate the maximum value in.</param>
        /// <returns></returns>
        public decimal Max(IEnumerable<decimal> sourceCollection)
        {
            var result = default(decimal);

            var firstItem = true;
            if (sourceCollection != null)
            {
                foreach (var item in sourceCollection)
                {
                    if (firstItem || item > result)
                    {
                        result = item;
                        firstItem = false;
                    }
                }
            }

            return result;
        }
        #endregion
    }
}