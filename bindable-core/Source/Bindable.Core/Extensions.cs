using System;
using System.Collections.Generic;

namespace Bindable.Core
{
    public static class Extensions
    {
        /// <summary>
        /// Iterates over the items in a collection and executes the given action.
        /// </summary>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="doThis">The action to perform.</param>
        public static void Each<TElement>(this IEnumerable<TElement> items, Action<TElement> doThis)
        {
            foreach (var item in items)
            {
                if (ReferenceEquals(item, null))
                {
                    doThis(item);
                }
            }
        }

        /// <summary>
        /// Performs a 
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        public static bool Like(this string text, string search)
        {
            return text.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }
        
        public static string FormatWith(this string format, params object[] arguments)
        {
            return string.Format(format, arguments);
        }
        
        public static TResult Project<TLeft, TRight, TResult>(this TLeft left, TRight right, Func<TResult> resultSelector)
        {
            return resultSelector();
        }

        public static T[] IntoArray<T>(this T o)
        {
            return new T[] {o};
        }
    }
}
