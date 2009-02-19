using System;
using System.Collections.Specialized;
using Bindable.Linq.Tests.TestLanguage.Expectations;

namespace Bindable.Linq.Tests.TestLanguage
{
    /// <summary>
    /// Extension methods that can be applied to <see cref="NotifyCollectionChangedAction"/> instances.
    /// </summary>
    internal static class EventSpecifications
    {
        public static RaiseCollectionEventExpectation AsEvent(this NotifyCollectionChangedAction action)
        {
            var result = new RaiseCollectionEventExpectation() { Action = action };
            return result;
        }

        public static RaiseCollectionEventExpectation WithNew(this RaiseCollectionEventExpectation spec, params object[] items)
        {
            spec.NewItems = items;
            return spec;
        }

        public static RaiseCollectionEventExpectation WithOld(this RaiseCollectionEventExpectation spec, params object[] items)
        {
            spec.OldItems = items;
            return spec;
        }

        public static RaiseCollectionEventExpectation AtNew(this RaiseCollectionEventExpectation spec, int index)
        {
            spec.NewIndex = index;
            return spec;
        }

        public static RaiseCollectionEventExpectation AtOld(this RaiseCollectionEventExpectation spec, int index)
        {
            spec.OldIndex = index;
            return spec;
        }

        public static RaiseCollectionEventExpectation WithNewCount(this RaiseCollectionEventExpectation spec, int count)
        {
            spec.NewItemsCount = count;
            return spec;
        }

        public static RaiseCollectionEventExpectation WithOldCount(this RaiseCollectionEventExpectation spec, int count)
        {
            spec.OldItemsCount = count;
            return spec;
        }

        public static RaiseCollectionEventExpectation With(this RaiseCollectionEventExpectation spec, params object[] items)
        {
            switch (spec.Action.Value)
            {
                case NotifyCollectionChangedAction.Add:
                    return spec.WithNew(items);
                case NotifyCollectionChangedAction.Remove:
                    return spec.WithOld(items);
                case NotifyCollectionChangedAction.Move:
                    return spec.WithOld(items);
                default:
                    throw new NotSupportedException("Invalid event specification: Both New and Old items must be specified explicitly when not using Add, Remove or Move event specifications.");
            }
        }

        public static RaiseCollectionEventExpectation With(this NotifyCollectionChangedAction action, params object[] items)
        {
            return action.AsEvent().With(items);
        }

        public static RaiseCollectionEventExpectation WithNew(this NotifyCollectionChangedAction action, params object[] items)
        {
            return action.AsEvent().WithNew(items);
        }

        public static RaiseCollectionEventExpectation WithNewCount(this NotifyCollectionChangedAction action, int count)
        {
            return action.AsEvent().WithNewCount(count);
        }

        public static RaiseCollectionEventExpectation WithOldCount(this NotifyCollectionChangedAction action, int count)
        {
            return action.AsEvent().WithOldCount(count);
        }

        public static RaiseCollectionEventExpectation WithOld(this NotifyCollectionChangedAction action, params object[] items)
        {
            return action.AsEvent().WithOld(items);
        }

        public static RaiseCollectionEventExpectation AtNew(this NotifyCollectionChangedAction action, int index)
        {
            return action.AsEvent().AtNew(index);
        }

        public static RaiseCollectionEventExpectation AtOld(this NotifyCollectionChangedAction action, int index)
        {
            return action.AsEvent().AtOld(index);
        }

        public static RaiseCollectionEventExpectation At(this NotifyCollectionChangedAction action, int index)
        {
            return action.AsEvent().At(index);
        }

        public static RaiseCollectionEventExpectation At(this RaiseCollectionEventExpectation spec, int index)
        {
            switch (spec.Action.Value)
            {
                case NotifyCollectionChangedAction.Add:
                    return spec.AtNew(index);
                case NotifyCollectionChangedAction.Remove:
                    return spec.AtNew(index);
                default:
                    throw new NotSupportedException("Invalid event specification: Both New and Old indexes must be specified explicitly when not using Add or Remove event specifications.");
            }
        }
    }
}