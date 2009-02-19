using System;
using Bindable.Core.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Core.Threading;

namespace Bindable.Linq.Aggregators
{    
    /// <summary>
    /// Aggregates a source collection using a custom accumulator callback provided by the caller.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TAccumulate">The type of the accumulate.</typeparam>
    internal sealed class CustomAggregator<TSource, TAccumulate> : Aggregator<TSource, TAccumulate>
    {
        private readonly Func<IBindableCollection<TSource>, TAccumulate> _aggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAggregator&lt;TSource, TAccumulate&gt;"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="aggregator">The aggregator.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        public CustomAggregator(IBindableCollection<TSource> source, Func<IBindableCollection<TSource>, TAccumulate> aggregator, IDispatcher dispatcher)
            : base(source, dispatcher)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(aggregator, "aggregator");
            Guard.NotNull(dispatcher, "dispatcher");
            
            _aggregator = aggregator;
        }

        /// <summary>
        /// When overridden in a derived class, provides the aggregator the opportunity to calculate the
        /// value.
        /// </summary>
        protected override void RefreshOverride()
        {
            Current = _aggregator(SourceCollection);
        }
    }
}