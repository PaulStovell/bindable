using Bindable.Aspects.Parameters;
using Bindable.Core.Threading;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq.Aggregators
{
    /// <summary>
    /// An aggregator that finds the element at a given index within a collection.
    /// </summary>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    internal sealed class ElementAtAggregator<TElement> : Aggregator<TElement, TElement>
    {
        private readonly TElement _default;
        private readonly int _index;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementAtAggregator&lt;TElement&gt;"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="index">The index.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        public ElementAtAggregator([NotNull]IBindableCollection<TElement> source, int index, [NotNull]IDispatcher dispatcher)
            : base(source, dispatcher)
        {
            _index = index;
            _default = default(TElement);
        }

        /// <summary>
        /// When overridden in a derived class, provides the aggregator the opportunity to calculate the
        /// value.
        /// </summary>
        protected override void RefreshOverride()
        {
            var currentIndex = 0;
            var found = false;
            var result = _default;
            foreach (var element in SourceCollection)
            {
                result = element;
                if (currentIndex == _index)
                {
                    found = true;
                    break;
                }
            }
            if (!found && currentIndex == -1)
            {
                result = _default;
            }
            Current = result;
        }
    }
}