using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Bindable.Linq.Tests.MockObjectModel;
using Bindable.Linq.Tests.TestLanguage.Expectations;
using System.Collections.Generic;
using System;
using MbUnit.Framework;

namespace Bindable.Linq.Tests.TestLanguage.Helpers
{
    /// <summary>
    /// A base class for all test classes that can provide access to a variety of 
    /// helper properties and methods.
    /// </summary>
    public abstract class TestFixture
    {
        private List<WeakReference> _referencesToCheckForLeaks = new List<WeakReference>();

        protected Contact Brian = new Contact {Name = "Brian"};
        protected Contact Bubsy = new Contact {Name = "Bubsy"};
        protected Contact Clancy = new Contact { Name = "Clancy" };
        protected Contact Fred = new Contact { Name = "Fred" };
        protected Contact Gordon = new Contact {Name = "Gordon"};
        protected Contact Harry = new Contact {Name = "Harry"};
        protected Contact Jack = new Contact {Name = "Jack"};
        protected Contact Jake = new Contact {Name = "Jake"};
        protected Contact Jarryd = new Contact {Name = "Jarryd"};
        protected Contact John = new Contact {Name = "John"};
        protected Contact Lesley = new Contact {Name = "Lesley"};
        protected Contact Michael = new Contact {Name = "Michael"};
        protected Contact Michelle = new Contact {Name = "Michelle"};
        protected Contact Mick = new Contact {Name = "Mick"};
        protected Contact Mary = new Contact {Name = "Mary"};
        protected Contact Mike = new Contact {Name = "Mike"};
        protected Contact Paul = new Contact {Name = "Paul"};
        protected Contact Phil = new Contact {Name = "Phil"};
        protected Contact Rick = new Contact {Name = "Rick"};
        protected Contact Ryan = new Contact {Name = "Ryan"};
        protected Contact Sally = new Contact {Name = "Sally"};
        protected Contact Sam = new Contact {Name = "Sam"};
        protected Contact Simon = new Contact {Name = "Simon"};
        protected Contact Sue = new Contact {Name = "Sue"};
        protected Contact Tim = new Contact {Name = "Tim"};
        protected Contact Tom = new Contact {Name = "Tom"};
        protected Contact NoName = new Contact {Name = ""};

        protected static ObservableCollection<Contact> CreateObservableCollection(params Contact[] contacts)
        {
            var result = new ObservableCollection<Contact>();
            foreach (var item in contacts)
            {
                result.Add(item);
            }
            return result;
        }

        public void ShouldHaveSameItems(IEnumerable bindableLinq, IEnumerable standardLinq)
        {
            CompatibilityValidator.CompareWithLinq(CompatabilityLevel.FullyCompatible, bindableLinq, standardLinq);
        }

        public void ShouldRaiseAdd(object contact, int index, NotifyCollectionChangedEventArgs args)
        {
            Add.WithNew(contact).AtNew(index).CompareTo(args);
        }

        public void ShouldRaiseMove(object contact, int index, int newIndex, NotifyCollectionChangedEventArgs args)
        {
            Move.WithOld(contact).AtOld(index).AtNew(newIndex).CompareTo(args);
        }

        public void ShouldRaiseRemove(object contact, int index, NotifyCollectionChangedEventArgs args)
        {
            Remove.WithOld(contact).AtOld(index).CompareTo(args);
        }

        /// <summary>
        /// Shortcut to NotifyCollectionChangedAction.Add.
        /// </summary>
        protected static NotifyCollectionChangedAction Add
        {
            get { return NotifyCollectionChangedAction.Add; }
        }

        /// <summary>
        /// Shortcut to NotifyCollectionChangedAction.Remove.
        /// </summary>
        protected static NotifyCollectionChangedAction Remove
        {
            get { return NotifyCollectionChangedAction.Remove; }
        }

        /// <summary>
        /// Shortcut to NotifyCollectionChangedAction.Replace.
        /// </summary>
        protected static NotifyCollectionChangedAction Replace
        {
            get { return NotifyCollectionChangedAction.Replace; }
        }

        /// <summary>
        /// Shortcut to NotifyCollectionChangedAction.Move.
        /// </summary>
        protected static NotifyCollectionChangedAction Move
        {
            get { return NotifyCollectionChangedAction.Move; }
        }

        /// <summary>
        /// Shortcut to NotifyCollectionChangedAction.Reset.
        /// </summary>
        protected static NotifyCollectionChangedAction Reset
        {
            get { return NotifyCollectionChangedAction.Reset; }
        }

        /// <summary>
        /// A PropertyChanged event for the given property will be raised.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        protected static IExpectation PropertyChange(string propertyName)
        {
            return new RaisePropertyEventExpectation(propertyName);
        }

        protected void AddMemoryLeakCheck(object o)
        {
            _referencesToCheckForLeaks.Add(new WeakReference(o, true));
        }

        protected void CheckForMemoryLeaks()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            foreach (var reference in _referencesToCheckForLeaks)
            {
                if (reference.Target != null)
                {
                    Assert.Fail(string.Format("The object {0} should have been cleaned up, but was not.", reference.Target));
                }
            }
        }
    }
}