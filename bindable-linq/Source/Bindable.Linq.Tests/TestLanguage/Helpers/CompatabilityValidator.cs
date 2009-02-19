using System.Collections;
using MbUnit.Framework;

namespace Bindable.Linq.Tests.TestLanguage.Helpers
{
    /// <summary>
    /// A helper class to compare Bindable LINQ queries with their LINQ to Objects counterpart.
    /// </summary>
    internal static class CompatibilityValidator
    {
        /// <summary>
        /// Compares the Bindable LINQ query with the equivalent LINQ query.
        /// </summary>
        /// <param name="expectations">The expectations.</param>
        /// <param name="bindableLinqQuery">The Bindable LINQ query.</param>
        /// <param name="standardLinqQuery">The LINQ query.</param>
        public static void CompareWithLinq(CompatabilityLevel expectations, IEnumerable bindableLinqQuery, IEnumerable standardLinqQuery)
        {
            switch (expectations)
            {
                case CompatabilityLevel.FullyCompatible:
                    CompareWithLinqOrdered(bindableLinqQuery, standardLinqQuery);
                    break;
                case CompatabilityLevel.FullyCompatibleExceptOrdering:
                    CompareWithLinqUnordered(bindableLinqQuery, standardLinqQuery);
                    break;
            }
        }

        /// <summary>
        /// Compares a Bindable LINQ query with a LINQ query.
        /// </summary>
        /// <param name="bindableLinqQuery">The sync linq collection.</param>
        /// <param name="standardLinqQuery">The linq query.</param>
        private static void CompareWithLinqOrdered(IEnumerable bindableLinqQuery, IEnumerable standardLinqQuery)
        {
            InnerCompareOrderedRecursively(bindableLinqQuery, standardLinqQuery);
        }

        /// <summary>
        /// Compares a Bindable LINQ query with a LINQ query.
        /// </summary>
        /// <param name="syncLinqCollection">The sync linq collection.</param>
        /// <param name="linqQuery">The linq query.</param>
        private static void CompareWithLinqUnordered(IEnumerable syncLinqCollection, IEnumerable linqQuery)
        {
            if (!InnerCompareUnorderedRecursively(syncLinqCollection, linqQuery))
            {
            }
        }

        private static void InnerCompareOrderedRecursively(IEnumerable left, IEnumerable right)
        {
            var leftEnumerator = left.GetEnumerator();
            var rightEnumerator = right.GetEnumerator();
            var index = 0;
            while (leftEnumerator.MoveNext() | rightEnumerator.MoveNext())
            {
                if (leftEnumerator.Current is IEnumerable)
                {
                    var leftChildIterator = leftEnumerator.Current as IEnumerable;
                    var rightChildIterator = rightEnumerator.Current as IEnumerable;
                    if (leftChildIterator != null && rightChildIterator != null)
                    {
                        InnerCompareOrderedRecursively(leftChildIterator, rightChildIterator);
                    }
                }
                else if (!AreEqual(leftEnumerator.Current, rightEnumerator.Current))
                {
                    Assert.Fail(string.Format("Error when comparing Iterator '{0}' with Iterator '{1}': Items at index {2} ('{3}' : '{4}') do not match.", left, right, index, leftEnumerator.Current, rightEnumerator.Current));
                }
                index++;
            }
        }

        private static bool InnerCompareUnorderedRecursively(IEnumerable left, IEnumerable right)
        {
            var equal = false;
            var leftList = new ArrayList();
            var rightList = new ArrayList();
            foreach (var o in left)
            {
                leftList.Add(o);
            }
            foreach (var o in right)
            {
                rightList.Add(o);
            }

            if (leftList.Count == rightList.Count)
            {
                equal = true;
                foreach (var leftItem in leftList)
                {
                    var leftItemFound = false;
                    if (leftItem is IEnumerable)
                    {
                        foreach (var rightItem in rightList)
                        {
                            if (InnerCompareUnorderedRecursively(leftItem as IEnumerable, rightItem as IEnumerable))
                            {
                                leftItemFound = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        var rightItem = FindEqualItem(leftItem, rightList);
                        if (rightItem != null && ContainsEqualItem(leftItem, rightList))
                        {
                            leftItemFound = true;
                        }
                    }
                    if (!leftItemFound)
                    {
                        equal = false;
                        break;
                    }
                }
            }
            return equal;
        }

        private static bool ContainsEqualItem(object find, IEnumerable list)
        {
            return FindEqualItem(find, list) != null;
        }

        private static object FindEqualItem(object find, IEnumerable list)
        {
            object result = null;
            foreach (var found in list)
            {
                if (AreEqual(find, found))
                {
                    result = found;
                    break;
                }
            }
            return result;
        }

        private static bool AreEqual(object left, object right)
        {
            var equal = true;
            if (left != right)
            {
                foreach (var leftProperty in left.GetType().GetProperties())
                {
                    var leftValue = leftProperty.GetValue(left, null);
                    var rightValue = right.GetType().GetProperty(leftProperty.Name).GetValue(right, null);
                    if (!((leftValue == null && rightValue == null) || leftValue.Equals(rightValue)))
                    {
                        equal = false;
                        break;
                    }
                }
            }
            return equal;
        }
    }

}
