using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Bindable.Aspects.Parameters;
using Bindable.Core.Helpers;
using Bindable.Linq.Configuration;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Core.Threading;
using Bindable.Core;

namespace Bindable.Linq.Aggregators
{    
    /// <summary>
    /// Serves as a base class for all aggregate functions. From Bindable LINQ's perspective, an Aggregator is a LINQ operation which 
    /// tranforms a collection of items into an item. This makes it different to an Iterator, which transforms a collection into another 
    /// collection, or an Operator which transforms one item into another.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public abstract class Aggregator<TSource, TResult> : DispatcherBound, IBindable<TResult>, IAcceptsDependencies
    {
        private readonly List<IDependency> _dependencies = new List<IDependency>();
        private readonly IBindableCollection<TSource> _sourceCollection;
        private TResult _current;
        private bool _hasEvaluated;
        private bool _isCurrentResultStillValid;

        /// <summary>
        /// Initializes a new instance of the <see cref="Aggregator&lt;TSource, TResult&gt;"/> class.
        /// </summary>
        /// <param name="sourceCollection">The source collection.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        protected Aggregator([NotNull]IBindableCollection<TSource> sourceCollection, [NotNull]IDispatcher dispatcher)
            : base(dispatcher)
        {
            _sourceCollection = sourceCollection;
            _sourceCollection.CollectionChanged += Weak.Event<NotifyCollectionChangedEventArgs>((sender, e) => Dispatcher.Dispatch(ReEvaluate)).KeepAlive(InstanceLifetime).HandlerProxy.Handler;
        }

        /// <summary>
        /// Invalidates and re-evaluates the aggregate.
        /// </summary>
        private void ReEvaluate()
        {
            Invalidate();
            Evaluate();
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the source collections that this aggregate is aggregating.
        /// </summary>
        protected IBindableCollection<TSource> SourceCollection
        {
            get { return _sourceCollection; }
        }

        /// <summary>
        /// The resulting value. Rather than being returned directly, the value is housed
        /// within the <see cref="IBindable{TElement}"/> container so that it can be updated when
        /// the source it was created from changes.
        /// </summary>
        /// <value></value>
        public TResult Current
        {
            get
            {
                AssertDispatcherThread();
                Evaluate();
                return _current;
            }
            protected set
            {
                AssertDispatcherThread();
                _current = value;
                OnPropertyChanged(CommonEventArgsCache.Current);
            }
        }

        /// <summary>
        /// The resulting value. Rather than being returned directly, the value is housed
        /// within the <see cref="IBindable{TValue}"/> container so that it can be updated when
        /// the source it was created from changes.
        /// </summary>
        /// <value></value>
        object IBindable.Current { get { return Current; } }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has evaluated.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has evaluated; otherwise, <c>false</c>.
        /// </value>
        public bool HasEvaluated
        {
            get
            {
                return _hasEvaluated;
            }
            set
            {
                AssertDispatcherThread();
                _hasEvaluated = value;
                OnPropertyChanged(CommonEventArgsCache.HasEvaluated);
            }
        }

        /// <summary>
        /// Sets a new dependency on a Bindable LINQ operation.
        /// </summary>
        /// <param name="definition">A definition of the dependency.</param>
        public void AcceptDependency([NotNull]IDependencyDefinition definition)
        {
            if (definition.AppliesToCollections())
            {
                var dependency = definition.ConstructForCollection(_sourceCollection, BindingConfigurations.Default.CreatePathNavigator());
                dependency.SetReevaluateCallback(o => { Invalidate(); Evaluate(); });
                _dependencies.Add(dependency);
            }
        }

        /// <summary>
        /// Evaluates this instance.
        /// </summary>
        public void Evaluate()
        {
            AssertDispatcherThread();
            Seal();

            if (!HasEvaluated)
            {
                HasEvaluated = true;
            }

            if (!_isCurrentResultStillValid)
            {
                _isCurrentResultStillValid = true;
                RefreshOverride();
            }
        }

        /// <summary>
        /// Refreshes the value by forcing it to be recalculated or reconsidered.
        /// </summary>
        public void Refresh()
        {
            AssertDispatcherThread();
            HasEvaluated = false;
            Evaluate();
        }

        /// <summary>
        /// Invalidates the last evaluation of this query so that the next time the result is required it is re-calculated.
        /// </summary>
        protected void Invalidate()
        {
            _isCurrentResultStillValid = false;
        }

        /// <summary>
        /// When overridden in a derived class, provides the aggregator the opportunity to calculate the 
        /// value.
        /// </summary>
        protected abstract void RefreshOverride();

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            AssertDispatcherThread();
            var handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Called just before the object is disposed and all event subscriptions are released.
        /// </summary>
        protected override void BeforeDisposeOverride()
        {
            base.BeforeDisposeOverride();
            foreach (var dependency in _dependencies)
                dependency.Dispose();
        }
    }
}