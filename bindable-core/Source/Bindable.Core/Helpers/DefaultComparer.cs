using System;
using System.Collections.Generic;

namespace Bindable.Core.Helpers
{
    /// <summary>
    /// A class that provides a default <see cref="IComparer{T}"/> and <see cref="IEqualityComparer{T}"/> implementation for any type.
    /// </summary>
    /// <typeparam name="TCompared">The type of the compared.</typeparam>
    /// <remarks>
    /// When ordering, we are normally given an IComparer, but sometimes we are given IComparable
    /// types instead. Rather than having "if" statements all through our OrderBy implementation
    /// we wrap the IComparable into an IComparer using this class.
    /// </remarks>
    public sealed class DefaultComparer<TCompared> : IComparer<TCompared>, IEqualityComparer<TCompared>
    {
        /// <summary>
        /// Compares the specified items.
        /// </summary>
        /// <param name="left">The first item.</param>
        /// <param name="right">The second items.</param>
        public int Compare(TCompared left, TCompared right)
        {
            if (left == null && right == null)
            {
                return 0;
            }
            if (left == null && right != null)
            {
                return -1;
            }
            if (left != null && right == null)
            {
                return 1;
            }
            
            var genericComparableLeft = left as IComparable<TCompared>;
            if (genericComparableLeft != null)
            {
                return genericComparableLeft.CompareTo(right);
            }
            var comparableLeft = left as IComparable;
            if (comparableLeft != null)
            {
                return comparableLeft.CompareTo(right);
            }
            return left.GetHashCode().CompareTo(right.GetHashCode());
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <paramref name="T"/> to compare.</param>
        /// <param name="y">The second object of type <paramref name="T"/> to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public bool Equals(TCompared x, TCompared y)
        {
            var areEqual = (x == null && y == null);
            if (!areEqual)
            {
                if (x != null && y != null)
                {
                    var equatableX = x as IEquatable<TCompared>;
                    if (equatableX != null)
                    {
                        areEqual = equatableX.Equals(y);
                    }
                    else
                    {
                        areEqual = x.Equals(y);
                    }
                }
            }
            return areEqual;
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param>
        /// <returns>A hash code for the specified object.</returns>
        /// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
        public int GetHashCode(TCompared obj)
        {
            var result = 0;
            if (obj != null)
            {
                result = obj.GetHashCode();
            }
            return result;
        }
    }
}