using System;
using System.Collections;
using System.Collections.Generic;
using Bindable.Core.Helpers;

namespace Bindable.Linq.Helpers
{
    /// <summary>
    /// Contains useful extension methods used only in Bindable LINQ.
    /// </summary>
    internal static class InternalExtensions
    {
        /// <summary>
        /// Enumerates the safely.
        /// </summary>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        public static List<TElement> EnumerateSafely<TElement>(this IEnumerable<TElement> elements)
        {
            var elementsType = elements.GetType();
            if (elementsType == typeof (List<TElement>))
            {
                return (List<TElement>) elements;
            }

            List<TElement> results = null;
            if (elements is ICollection)
            {
                results = new List<TElement>(((ICollection) elements).Count);
            }
            else
            {
                results = new List<TElement>();
            }
            results.AddRange(elements);
            return results;
        }

        /// <summary>
        /// Enables indexed item retrieval over IEnumerable. O(n).
        /// </summary>
        public static TElement Item<TElement>(this IEnumerable<TElement> collection, int index)
        {
            Guard.NotNull(collection, "collection");

            var list = collection as IList<TElement>;
            if (null != list) return list[index];

            if (index < 0) throw new ArgumentOutOfRangeException("index");

            var remaining = index;
            foreach (var item in collection)
            {
                if (0 == remaining) return item;
                remaining--;
            }

            throw new ArgumentOutOfRangeException("index");
        }
    }
}