using System.ComponentModel;
using Bindable.Core;
using Bindable.Core.Helpers;
using Bindable.Core.Threading;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq.Operators
{
    /// <summary>
    /// Represents a Switch statement that has been declared but no cases have been defined, and so the return type is not yet known.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    internal sealed class EmptySwitchDefinition<TSource> : DispatcherBound, ISwitchDeclaration<TSource>
    {
        private readonly IBindable<TSource> _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptySwitchDefinition&lt;TSource&gt;"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        public EmptySwitchDefinition(IBindable<TSource> source, IDispatcher dispatcher)
            : base(dispatcher)
        {
            Guard.NotNull(source, "source");
            _source = source;
        }

        /// <summary>
        /// Creates the type of for result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ISwitch<TSource, TResult> CreateForResultType<TResult>()
        {
            return new SwitchOperator<TSource, TResult>(_source, Dispatcher);
        }
    }
}