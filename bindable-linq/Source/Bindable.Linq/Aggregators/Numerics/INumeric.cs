using System.Collections.Generic;

namespace Bindable.Linq.Aggregators.Numerics
{    
    /// <summary>
    /// Implementers of this interface provide a helpful wrapper around numeric functions.
    /// Numeric types (integers, doubles, etc.) don't implement a common interface for numeric operations
    /// (adding, dividing, etc.). To provide generic implementations of bindable aggregates such as Sum,
    /// Min and Max, we use implementations of this interface to wrap these common operations.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    /// <typeparam name="TAverageResult">The type of the result when averaging.</typeparam>
    internal interface INumeric<TItem, TAverageResult>
    {
        /// <summary>
        /// Given a list of items, adds them and returns the sum.
        /// </summary>
        /// <param name="itemsToSum">The items to sum.</param>
        /// <returns></returns>
        TItem Sum(IEnumerable<TItem> itemsToSum);

        /// <summary>
        /// Given a list of items, computes the average value.
        /// </summary>
        /// <param name="items">The items to average.</param>
        /// <returns></returns>
        TAverageResult Average(IEnumerable<TItem> items);

        /// <summary>
        /// Given a list of items, returns the lowest value.
        /// </summary>
        /// <param name="items">The items to locate the minimum value in.</param>
        /// <returns></returns>
        TItem Min(IEnumerable<TItem> items);

        /// <summary>
        /// Given a list of items, returns the highest value.
        /// </summary>
        /// <param name="items">The items to locate the maximum value in.</param>
        /// <returns></returns>
        TItem Max(IEnumerable<TItem> items);
    }
}