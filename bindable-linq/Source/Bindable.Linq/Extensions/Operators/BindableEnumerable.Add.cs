using Bindable.Linq.Dependencies;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>
        /// Adds a bindable value to the current value and automatically wires up any dependencies.
        /// </summary>
        /// <param name="source">The source value to add.</param>
        /// <param name="toAdd">To item to add.</param>
        /// <returns>
        /// A bindable value representing the sum of the two items.
        /// </returns>
        public static IBindable<int> Add(this IBindable<int> source, IBindable<int> toAdd)
        {
            return source.Project(s => s + toAdd.Current, DependencyDiscovery.Enabled);
        }

        /// <summary>
        /// Adds a bindable value to the current value and automatically wires up any dependencies.
        /// </summary>
        /// <param name="source">The source value to add.</param>
        /// <param name="toAdd">To item to add.</param>
        /// <returns>
        /// A bindable value representing the sum of the two items.
        /// </returns>
        public static IBindable<float> Add(this IBindable<float> source, IBindable<float> toAdd)
        {
            return source.Project(s => s + toAdd.Current, DependencyDiscovery.Enabled);
        }

        /// <summary>
        /// Adds a bindable value to the current value and automatically wires up any dependencies.
        /// </summary>
        /// <param name="source">The source value to add.</param>
        /// <param name="toAdd">To item to add.</param>
        /// <returns>
        /// A bindable value representing the sum of the two items.
        /// </returns>
        public static IBindable<decimal> Add(this IBindable<decimal> source, IBindable<decimal> toAdd)
        {
            return source.Project(s => s + toAdd.Current, DependencyDiscovery.Enabled);
        }

        /// <summary>
        /// Adds a bindable value to the current value and automatically wires up any dependencies.
        /// </summary>
        /// <param name="source">The source value to add.</param>
        /// <param name="toAdd">To item to add.</param>
        /// <returns>
        /// A bindable value representing the sum of the two items.
        /// </returns>
        public static IBindable<double> Add(this IBindable<double> source, IBindable<double> toAdd)
        {
            return source.Project(s => s + toAdd.Current, DependencyDiscovery.Enabled);
        }

        /// <summary>
        /// Adds a bindable value to the current value and automatically wires up any dependencies.
        /// </summary>
        /// <param name="source">The source value to add.</param>
        /// <param name="toAdd">To item to add.</param>
        /// <returns>
        /// A bindable value representing the sum of the two items.
        /// </returns>
        public static IBindable<long> Add(this IBindable<long> source, IBindable<long> toAdd)
        {
            return source.Project(s => s + toAdd.Current, DependencyDiscovery.Enabled);
        }
	}
}
