using System.Collections.Generic;
using System.ComponentModel;
using Bindable.Core.Helpers;
using Bindable.Linq.Configuration;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Core.Threading;
using Bindable.Core;

namespace Bindable.Linq.Operators
{
    /// <summary>
    /// Serves as a base class for all operator functions. From Bindable LINQ's perspective, an Operator is a LINQ 
    /// operation which tranforms a single source items into single result item. This makes it different to an Iterator 
    /// which transforms a collection into another collection, or an Aggregator which transforms a collection into a 
    /// single element.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public abstract class Operator<TSource, TResult> : DispatcherBound, IBindable<TResult>, IAcceptsDependencies
    {
        private readonly List<IDependency> _dependencies = new List<IDependency>();
        private readonly IBindable<TSource> _source;
        private TResult _current;
        private bool _hasEvaluated;

        /// <summary>
        /// Initializes a new instance of the <see cref="Operator&lt;TSource, TResult&gt;"/> class.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <param name="source">The source.</param>
        protected Operator(IBindable<TSource> source, IDispatcher dispatcher)
            : base(dispatcher)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(dispatcher, "dispatcher");
            _source = source;
            _source.PropertyChanged += Weak.Event<PropertyChangedEventArgs>((sender, e) => Dispatcher.Dispatch(Refresh)).KeepAlive(InstanceLifetime).HandlerProxy.Handler;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the source.
        /// </summary>
        public IBindable<TSource> Source
        {
            get { return _source; }
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
        /// When overridden in a derived class, refreshes the operator.
        /// </summary>
        protected abstract void RefreshOverride();

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
            private set
            {
                AssertDispatcherThread();
                _hasEvaluated = value;
                OnPropertyChanged(CommonEventArgsCache.HasEvaluated);
            }
        }

        /// <summary>
        /// Refreshes the object.
        /// </summary>
        public void Refresh()
        {
            AssertDispatcherThread();

            HasEvaluated = false;
            Evaluate();
        }

        /// <summary>
        /// Sets a new dependency on a Bindable LINQ operation.
        /// </summary>
        /// <param name="definition">A definition of the dependency.</param>
        public void AcceptDependency(IDependencyDefinition definition)
        {
            Guard.NotNull(definition, "definition");
            if (definition.AppliesToSingleElement())
            {
                var dependency = definition.ConstructForElement(_source, BindingConfigurations.Default.CreatePathNavigator());
                dependency.SetReevaluateCallback(o => Refresh());
                _dependencies.Add(dependency);
            }
        }

        /// <summary>
        /// Evaluates the operator.
        /// </summary>
        public void Evaluate()
        {
            AssertDispatcherThread();
            Seal();

            if (!HasEvaluated)
            {
                HasEvaluated = true;
                RefreshOverride();
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            AssertDispatcherThread();
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
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