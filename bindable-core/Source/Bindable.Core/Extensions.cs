using System;
using System.Collections.Generic;
using System.Text;
using Bindable.Core.Language;
using Bindable.Core.Dynamics;

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
        /// Converts a given positive integer into its English representation. For example, 1432 will return "one thousand, four hundred and thirty-two".
        /// </summary>
        /// <param name="i">The integer to convert into the English representation.</param>
        /// <returns>A string containing the English representation of the integer.</returns>
        public static string ToEnglish(this int i)
        {
            return new EnglishConverter().ToEnglish(i);
        }

        /// <summary>
        /// Compares two words using the SoundEx library to find similar words.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="otherWord">The other word.</param>
        /// <returns>True if the words have the same SoundEx value; otherwise false.</returns>
        public static bool SoundExEquals(this string word, string otherWord)
        {
            return SoundExEquals(word, otherWord, SoundExComparison.Default);
        }

        /// <summary>
        /// Compares two words using the SoundEx library to find similar words.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="otherWord">The other word.</param>
        /// <param name="comparsion">The SoundEx comparsion rules to use.</param>
        /// <returns>
        /// True if the words have the same SoundEx value; otherwise false.
        /// </returns>
        public static bool SoundExEquals(this string word, string otherWord, SoundExComparison comparsion)
        {
            return SoundEx.Evaluate(word).Equals(SoundEx.Evaluate(otherWord), comparsion);
        }

        /// <summary>
        /// Confirms whether the text contains a specified string, using a specific set of comparsion rules.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="search">The search.</param>
        /// <param name="comparison">The comparison.</param>
        public static bool Contains(this string text, string search, StringComparison comparison)
        {
            return text.IndexOf(search, comparison) >= 0;
        }

        /// <summary>
        /// Takes one item and returns an array containing that one item. It can then be unioned with other collections.
        /// </summary>
        /// <typeparam name="T">The type of the item and array.</typeparam>
        /// <param name="o">The item to create an array of.</param>
        /// <returns>An array containing the item.</returns>
        public static T[] IntoArray<T>(this T o)
        {
            return new T[] {o};
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
        /// Gets an item from a dictionary, or creates it if it does not exist.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="creator">The creator.</param>
        /// <returns></returns>
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> creator) where TValue : class
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary[key] = creator();
            }
            return dictionary[key];
        }
    }
}
