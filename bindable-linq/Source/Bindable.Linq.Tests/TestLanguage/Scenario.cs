using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Bindable.Core.EventCatchers;
using Bindable.Linq.Tests.TestLanguage.Helpers;
using Bindable.Linq.Tests.TestLanguage.Steps;
using MbUnit.Framework;

namespace Bindable.Linq.Tests.TestLanguage
{
    /// <summary>
    /// Represents a scenario that is defined as part of a specification.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    internal class Scenario<TInput> : IScenario<TInput>
    {
        private readonly string _title;
        private readonly ObservableCollection<TInput> _inputs;
        private object _bindableLinqQuery;
        private readonly object _standardLinqQuery;
        private readonly IEnumerable<Step> _steps;
        private readonly IEventCatcher<NotifyCollectionChangedEventArgs> _collectionEventMonitor;
        private readonly IEventCatcher<PropertyChangedEventArgs> _propertyEventMonitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scenario&lt;TInput&gt;"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="inputs">The inputs.</param>
        /// <param name="steps">The steps.</param>
        /// <param name="bindableLinqQuery">The bindable linq query.</param>
        /// <param name="standardLinqQuery">The standard linq query.</param>
        public Scenario(string title, ObservableCollection<TInput> inputs, IEnumerable<Step> steps, object bindableLinqQuery, object standardLinqQuery)
        {
            _title = title;
            _inputs = inputs;
            _steps = steps.ToList();
            _bindableLinqQuery = bindableLinqQuery;
            _standardLinqQuery = standardLinqQuery;
            
            _propertyEventMonitor = new PropertyEventCatcher((INotifyPropertyChanged)_bindableLinqQuery);
            if (_bindableLinqQuery is INotifyCollectionChanged) _collectionEventMonitor = new CollectionEventCatcher((INotifyCollectionChanged)_bindableLinqQuery);

            foreach (var step in _steps)
            {
                step.Scenario = this;
            }
        }

        /// <summary>
        /// Gets or sets the compatability level.
        /// </summary>
        /// <value>The compatability level.</value>
        public CompatabilityLevel CompatabilityLevel { get; set; }

        /// <summary>
        /// Gets the inputs to the scenario.
        /// </summary>
        public ObservableCollection<TInput> Inputs
        {
            get { return _inputs; }
        }

        /// <summary>
        /// Gets the instance of the Bindable LINQ query.
        /// </summary>
        public object BindableLinqQuery
        {
            get { return _bindableLinqQuery; }
        }

        /// <summary>
        /// Gets the instance of the standard LINQ query.
        /// </summary>
        public object StandardLinqQuery
        {
            get { return _standardLinqQuery; }
        }

        /// <summary>
        /// Gets the title of the specification.
        /// </summary>
        public string Title
        {
            get { return _title; }
        }

        /// <summary>
        /// Gets an event monitor attached to the query.
        /// </summary>
        public IEventCatcher<NotifyCollectionChangedEventArgs> CollectionEventMonitor
        {
            get { return _collectionEventMonitor; }
        }

        /// <summary>
        /// Gets an event monitor attached to the query.
        /// </summary>
        public IEventCatcher<PropertyChangedEventArgs> PropertyEventMonitor
        {
            get { return _propertyEventMonitor; }
        }

        /// <summary>
        /// Executes the scenario and verifies the expectations attached to it.
        /// </summary>
        public void Execute()
        {
            Tracer.WriteLine("Validating scenario: {0}", this.Title);
            var stepCount = 1;
            using (Tracer.Indent())
            {
                foreach (var step in _steps)
                {
                    Tracer.Write("Step {0} ", stepCount);
                    try
                    {
                        step.Execute();
                        Tracer.WriteLine("");
                    }
                    catch
                    {
                        Tracer.WriteLine("failed");
                        Tracer.WriteLine("");
                        Tracer.WriteLine("Exception details:");
                        throw;
                    }
                    stepCount++;
                }

                // Ensure no additional events are in the queue
                Tracer.WriteLine("Finalizing: Verifying additional events were not raised.");
                if (CollectionEventMonitor != null)
                {
                    Assert.IsNull(CollectionEventMonitor.Dequeue(), "The test has completed, but there are still events in the queue that were not expected.");
                }

                if (BindableLinqQuery is IEnumerable)
                {
                    // Compare with LINQ to Objects
                    Tracer.WriteLine("Finalizing: Comparing final results to LINQ to Objects");
                    CompatibilityValidator.CompareWithLinq(CompatabilityLevel, (IEnumerable)BindableLinqQuery, (IEnumerable)StandardLinqQuery);
                }

                // Forget all references to the query and ensure it is garbage collected
                Tracer.WriteLine("Finalizing: Detecting memory leaks and event handlers that have not been unhooked.");
                var bindableQueryReference = new WeakReference(BindableLinqQuery, false);
                _bindableLinqQuery = null;
                if (CollectionEventMonitor != null) CollectionEventMonitor.Dispose();
                PropertyEventMonitor.Dispose();

                GC.Collect();
                GC.WaitForPendingFinalizers();
                Assert.IsFalse(bindableQueryReference.IsAlive, "There should be no live references to the bindable query at this point. This may indicate that the query or items within the query have event handlers that have not been unhooked.");
            }
        }
    }
}
