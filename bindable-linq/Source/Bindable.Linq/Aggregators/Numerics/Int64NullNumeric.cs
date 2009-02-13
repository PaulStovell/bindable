using System.Collections.Generic;

namespace Bindable.Linq.Aggregators.Numerics
{
    /// <summary>
    /// Provides a numeric operations wrapper for nullable long types.
    /// </summary>
    internal class Int64NullNumeric : INumeric<long?, double?>
    {
        #region INumeric<long?,double?> Members
        /// <summary>
        /// Given a list of sourceCollection, adds them and returns the sum.
        /// </summary>
        /// <param name="sourceCollection">The sourceCollection to sum.</param>
        /// <returns></returns>
        public long? Sum(IEnumerable<long?> sourceCollection)
        {
            var result = default(long);

            if (sourceCollection != null)
            {
                foreach (var item in sourceCollection)
                {
                    if (item != null)
                    {
                        result += item.Value;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Given a list of sourceCollection, computes the average value.
        /// </summary>
        /// <param name="sourceCollection">The sourceCollection to average.</param>
        /// <returns></returns>
        public double? Average(IEnumerable<long?> sourceCollection)
        {
            var result = default(double);

            var count = 0;
            if (sourceCollection != null)
            {
                foreach (var item in sourceCollection)
                {
                    if (item != null)
                    {
                        result += item.Value;
                        count++;
                    }
                }
            }

            if (count == 0)
            {
                return null;
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
        public long? Min(IEnumerable<long?> sourceCollection)
        {
            var result = default(long);

            var firstItem = true;
            if (sourceCollection != null)
            {
                foreach (var item in sourceCollection)
                {
                    if (item != null)
                    {
                        if (firstItem || item.Value < result)
                        {
                            result = item.Value;
                            firstItem = false;
                        }
                    }
                }
            }

            if (firstItem)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Given a list of sourceCollection, returns the highest value.
        /// </summary>
        /// <param name="sourceCollection">The sourceCollection to locate the maximum value in.</param>
        /// <returns></returns>
        public long? Max(IEnumerable<long?> sourceCollection)
        {
            var result = default(long);

            var firstItem = true;
            if (sourceCollection != null)
            {
                foreach (var item in sourceCollection)
                {
                    if (item != null)
                    {
                        if (firstItem || item > result)
                        {
                            result = item.Value;
                            firstItem = false;
                        }
                    }
                }
            }

            if (firstItem)
            {
                return null;
            }
            else
            {
                return result;
            }
        }
        #endregion
    }
}