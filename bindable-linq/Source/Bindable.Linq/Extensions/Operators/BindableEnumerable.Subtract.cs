using Bindable.Linq.Interfaces;

namespace Bindable.Linq
{
	public static partial class BindableEnumerable
	{
        /// <summary>
        /// Subtracts a bindable value from the current value and automatically wires up any dependencies.
        /// </summary>
        /// <param name="source">The source value to subtract.</param>
        /// <param name="toSubtract">To item to subtract.</param>
        /// <returns>
        /// A bindable value representing the result of the two items.
        /// </returns>
        public static IBindable<int> Subtract(this IBindable<int> source, IBindable<int> toSubtract)
        {
            return source.Project(s => s + toSubtract.Current, DependencyDiscovery.Enabled);
        }

        /// <summary>
        /// Subtracts a bindable value from the current value and automatically wires up any dependencies.
        /// </summary>
        /// <param name="source">The source value to subtract.</param>
        /// <param name="toSubtract">To item to subtract.</param>
        /// <returns>
        /// A bindable value representing the result of the two items.
        /// </returns>
        public static IBindable<float> Subtract(this IBindable<float> source, IBindable<float> toSubtract)
        {
            return source.Project(s => s + toSubtract.Current, DependencyDiscovery.Enabled);
        }

        /// <summary>
        /// Subtracts a bindable value from the current value and automatically wires up any dependencies.
        /// </summary>
        /// <param name="source">The source value to subtract.</param>
        /// <param name="toSubtract">To item to subtract.</param>
        /// <returns>
        /// A bindable value representing the result of the two items.
        /// </returns>
        public static IBindable<decimal> Subtract(this IBindable<decimal> source, IBindable<decimal> toSubtract)
        {
            return source.Project(s => s + toSubtract.Current, DependencyDiscovery.Enabled);
        }

        /// <summary>
        /// Subtracts a bindable value from the current value and automatically wires up any dependencies.
        /// </summary>
        /// <param name="source">The source value to subtract.</param>
        /// <param name="toSubtract">To item to subtract.</param>
        /// <returns>
        /// A bindable value representing the result of the two items.
        /// </returns>
        public static IBindable<double> Subtract(this IBindable<double> source, IBindable<double> toSubtract)
        {
            return source.Project(s => s + toSubtract.Current, DependencyDiscovery.Enabled);
        }

        /// <summary>
        /// Subtracts a bindable value from the current value and automatically wires up any dependencies.
        /// </summary>
        /// <param name="source">The source value to subtract.</param>
        /// <param name="toSubtract">To item to subtract.</param>
        /// <returns>
        /// A bindable value representing the result of the two items.
        /// </returns>
        public static IBindable<long> Subtract(this IBindable<long> source, IBindable<long> toSubtract)
        {
            return source.Project(s => s + toSubtract.Current, DependencyDiscovery.Enabled);
        }
	}
}
