using System;
using System.Collections.Generic;
using Bindable.Core.Helpers;
using Bindable.Linq.Helpers;

namespace Bindable.Linq.Iterators
{
    /// <summary>
    /// Base class used for creating a list of comparers.
    /// </summary>
    /// <typeparam name="S">The type of sort item source item.</typeparam>
    /// <remarks>
    /// This base class is used so that we can enable strong typing without the comparers 
    /// needing to know the key types (K) used by the subsequent ItemSorters. 
    /// See the remarks for <see cref="ItemSorter{S,K}"/> for details.
    /// </remarks>
    internal abstract class ItemSorter<S>
    {
        /// <summary>
        /// Compares two items, returning an integer signifying if one should be sorted above or 
        /// below another. 
        /// </summary>
        /// <param name="left">The first item to compare.</param>
        /// <param name="right">The second item to compare.</param>
        public abstract int Compare(S left, S right);
    }

    /// <summary>
    /// A class that is nested within itself to create a chain of IComparers. 
    /// </summary>
    /// <typeparam name="S">The source type.</typeparam>
    /// <typeparam name="K">The type of key used in the comparisson.</typeparam>
    /// <remarks>
    /// Suppose someone had the following in a LINQ query:
    /// 
    ///     order by customer.Name, customer.DateOfBirth
    /// 
    /// The first part of the OrderBy would produce an OrderByIterator, initialized with an ItemSorter
    /// that sorts by the customer.Name.
    /// 
    /// The second part of the OrderBy would produce another OrderByIterator, initialized with a 
    /// new ItemSorter. That ItemSorter would have the original ItemSorter (customer.Name) as its "superior", 
    /// and would use customer.DateOfBirth as the "fallback" sort. 
    /// 
    /// Taking this even further:
    /// 
    ///    order by customer.Name, customer.DateOfBirth, customer.EmailAddress
    ///    
    /// Would result in the following graph:
    /// 
    ///     OrderByIterator.ItemSorter          // customer.EmailAddress
    ///                    .Superior            // customer.DateOfBirth
    ///                    .Superior            // customer.Name
    /// 
    /// (Note that this is in the reverse order to the LINQ query)
    /// 
    /// When the Compare method is called on the ItemSorter, it first tries its superior (which tries 
    /// its superior, which tries its superior, ad infinitum). 
    /// </remarks>
    internal sealed class ItemSorter<S, K> : ItemSorter<S>
    {
        private readonly bool _ascending;
        private readonly IComparer<K> _comparer;
        public readonly Func<S, K> _keySelector;
        private readonly ItemSorter<S> _superior;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="superior">The superior.</param>
        /// <param name="keySelector"></param>
        /// <param name="comparer"></param>
        /// <param name="ascending"></param>
        public ItemSorter(ItemSorter<S> superior, Func<S, K> keySelector, IComparer<K> comparer, bool ascending)
        {
            _keySelector = keySelector;
            _superior = superior;
            _comparer = comparer ?? new DefaultComparer<K>();
            _ascending = ascending;
        }

        /// <summary>
        /// Compares two items, returning an integer signifying if one should be sorted above or 
        /// below another. 
        /// </summary>
        /// <param name="left">The first item to compare.</param>
        /// <param name="right">The second item to compare.</param>
        /// <returns>
        /// As described in the class remarks, we first try to use the "superior's" Compare method. 
        /// Only when the superior returns "0" (same value) do we use our own IComparer. 
        /// </returns>
        public override int Compare(S left, S right)
        {
            var result = 0;
            if (_superior != null)
            {
                result = _superior.Compare(left, right);
            }
            if (result == 0)
            {
                // Extract the keys
                var leftKey = _keySelector(left);
                var rightKey = _keySelector(right);

                // Compare
                result = _comparer.Compare(leftKey, rightKey);

                // For descending sorts, we simply flip the sign.
                if (!_ascending)
                {
                    result = -result;
                }
            }
            return result;
        }
    }
}