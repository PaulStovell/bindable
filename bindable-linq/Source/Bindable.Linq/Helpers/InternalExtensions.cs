using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace Bindable.Linq.Helpers
{
    /// <summary>
    /// Contains useful extension methods used only in Bindable LINQ.
    /// </summary>
    internal static class InternalExtensions
    {
        /// <summary>
        /// Checks that a given argument is not null.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        public static void ShouldNotBeNull(this object item, string argumentName)
        {
            if (item == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// Checks that a given argument is not null.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="item">The item to check.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="message">The message.</param>
        public static void ShouldBe<TItem>(this TItem item, Func<TItem, bool> condition, string message)
        {
            if (!condition(item))
            {
                throw new ArgumentException(message);
            }
        }

        /// <summary>
        /// Formats a string.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static string FormatWith(this string format, params object[] arguments)
        {
            return string.Format(format, arguments);
        }

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
        /// Collects multiple enumerables into one.
        /// </summary>
        /// <typeparam name="TElement">The type being enumerated.</typeparam>
        /// <param name="enumerables">The enumerables.</param>
        /// <returns></returns>
        public static IEnumerable<TElement> UnionAll<TElement>(this IEnumerable<IEnumerable<TElement>> enumerables)
        {
            if (enumerables != null)
            {
                foreach (var enumerable in enumerables)
                {
                    if (enumerable != null)
                    {
                        foreach (var item in enumerable)
                        {
                            yield return item;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Concatenates a collection of strings into one, using a specified seperator between them.
        /// </summary>
        /// <param name="strings">The strings.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string ConcatStrings(this IEnumerable<string> strings, string separator)
        {
            var result = new StringBuilder();
            var stringEnumerator = strings.GetEnumerator();
            var hasNext = stringEnumerator.MoveNext();
            while (hasNext)
            {
                result.Append(stringEnumerator.Current);
                hasNext = stringEnumerator.MoveNext();
                if (hasNext)
                {
                    result.Append(separator);
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Applies the given action to every element in the collection.
        /// </summary>
        public static void ForEach<TElement>(this IEnumerable<TElement> collection, Action<TElement> action)
        {
            if (collection != null)
            {
                foreach (var element in collection)
                {
                    if (element != null)
                    {
                        action(element);
                    }
                }
            }
        }

        /// <summary>
        /// Evaluates the specified collection.
        /// </summary>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="collection">The collection.</param>
        public static void Evaluate<TElement>(this IEnumerable<TElement> collection)
        {
            using (var enumerator = collection.GetEnumerator())
            {
                enumerator.MoveNext();
            }
        }

        /// <summary>
        /// Enables indexed item retrieval over IEnumerable. O(n).
        /// </summary>
        public static TElement Item<TElement>(this IEnumerable<TElement> collection, int index)
        {
            collection.ShouldNotBeNull("collection");

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