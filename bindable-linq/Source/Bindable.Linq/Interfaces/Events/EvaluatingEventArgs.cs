using System;
using System.Collections.Generic;

namespace Bindable.Linq.Interfaces.Events
{
    /// <summary>
    /// Handler for the <see cref="EvaluatingEventArgs{TElement}"/>.
    /// </summary>
    public delegate void EvaluatingEventHandler<TElement>(object sender, EvaluatingEventArgs<TElement> args);

    /// <summary>
    /// Event arguments raised when a collection is evaluated for the first time.
    /// </summary>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    public class EvaluatingEventArgs<TElement> : EventArgs
    {
        private readonly List<TElement> _itemsYeildedFromFirstEvaluation;

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluatingEventArgs&lt;TElement&gt;"/> class.
        /// </summary>
        /// <param name="itemsYielded">The items yielded.</param>
        public EvaluatingEventArgs(List<TElement> itemsYielded)
        {
            _itemsYeildedFromFirstEvaluation = itemsYielded;
        }

        /// <summary>
        /// Gets the items yielded from first evaluation.
        /// </summary>
        /// <value>The items yielded from first evaluation.</value>
        public List<TElement> ItemsYieldedFromEvaluation
        {
            get { return _itemsYeildedFromFirstEvaluation; }
        }
    }
}