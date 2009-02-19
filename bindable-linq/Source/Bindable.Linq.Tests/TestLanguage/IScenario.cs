using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Bindable.Core.EventCatchers;
using Bindable.Linq.Tests.TestLanguage.Helpers;

namespace Bindable.Linq.Tests.TestLanguage
{
    /// <summary>
    /// Represents a scenario.
    /// </summary>
    public interface IScenario
    {
        object BindableLinqQuery { get; }
        object StandardLinqQuery { get; }
        string Title { get; }
        void Execute();
        IEventCatcher<PropertyChangedEventArgs> PropertyEventMonitor { get; }
        IEventCatcher<NotifyCollectionChangedEventArgs> CollectionEventMonitor { get; }
        CompatabilityLevel CompatabilityLevel { get; set; }
    }

    /// <summary>
    /// Represents a scenario.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    internal interface IScenario<TInput> : IScenario
    {
        ObservableCollection<TInput> Inputs { get; }
    }
}