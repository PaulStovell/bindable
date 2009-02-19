using System.Collections.ObjectModel;
using Bindable.Linq.Tests.TestLanguage.Helpers;

namespace Bindable.Linq.Tests.TestLanguage
{
    /// <summary>
    /// Provides an easy way to construct collections of test inputs.
    /// </summary>
    internal class With
    {
        /// <summary>
        /// Creates a set of test inputs that will be passed to each scenario in the test.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <param name="items">The items.</param>
        /// <returns>A collection of inputs.</returns>
        public static ObservableCollection<TInput> Inputs<TInput>(params TInput[] items)
        {
            var results = new ObservableCollection<TInput>();
            results.AddRange(items);
            return results;
        }
    }
}
