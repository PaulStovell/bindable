using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using Bindable.Core.EventCatchers;
using Bindable.Core.Threading;
using Bindable.Linq.Tests.MockObjectModel;
using Bindable.Linq.Tests.TestLanguage.Helpers;
using MbUnit.Framework;

namespace Bindable.Linq.Tests.Behavior.Iterators
{
    [TestFixture]
    public class SelectBehavior : TestFixture
    {
        [Test]
        public void SelectSpecifications()
        {
            var contacts1 = CreateObservableCollection(Tom, Sam);
            var contacts2 = CreateObservableCollection(Tom, Sam);
            var contacts3 = CreateObservableCollection(Tom, Sam);
            var contacts4 = CreateObservableCollection(Tom, Sam);
            ExecuteSelectSpecification(contacts1, contact => contact);
            ExecuteSelectSpecification(contacts2, contact => new { BlahBlah = contact.Name });
            ExecuteSelectSpecification(contacts3, contact => new { DudeName = contact.Name.ToUpper() });
            ExecuteSelectSpecification(contacts4, contact => new ContactSummary() { Summary = contact.Name.ToUpper() });
            CheckForMemoryLeaks();
        }

        private void ExecuteSelectSpecification<TProjectionResult>(ObservableCollection<Contact> contacts, Expression<Func<Contact, TProjectionResult>> selector)
        {
            var project = selector.Compile();
            var bindableLinq = contacts.AsBindable(new TestDispatcher()).Select(selector);
            var standardLinq = contacts.Select(selector.Compile());
            var eventCatcher = new CollectionEventCatcher(bindableLinq);
            AddMemoryLeakCheck(bindableLinq);
            
            Assert.IsFalse(bindableLinq.HasEvaluated);
            Assert.AreEqual(0, eventCatcher.Count);

            bindableLinq.Evaluate();
            Assert.IsTrue(bindableLinq.HasEvaluated);
            Assert.AreEqual(2, bindableLinq.Count);
            Assert.AreEqual(0, eventCatcher.Count);
            
            contacts.Add(Tim);
            ShouldRaiseAdd(project(Tim), 2, eventCatcher.Dequeue().Arguments);
            ShouldHaveSameItems(bindableLinq, standardLinq);
            
            contacts.Insert(1, Simon);
            ShouldRaiseAdd(project(Simon), 1, eventCatcher.Dequeue().Arguments);
            ShouldHaveSameItems(bindableLinq, standardLinq);
            
            contacts.Insert(0, Fred);
            ShouldRaiseAdd(project(Fred), 0, eventCatcher.Dequeue().Arguments);
            ShouldHaveSameItems(bindableLinq, standardLinq);
            
            contacts.Move(0, 3);
            ShouldRaiseMove(project(Fred), 0, 3, eventCatcher.Dequeue().Arguments);
            ShouldHaveSameItems(bindableLinq, standardLinq);

            contacts.Remove(Tim);
            ShouldRaiseRemove(project(Tim), 4, eventCatcher.Dequeue().Arguments);
            ShouldHaveSameItems(bindableLinq, standardLinq);
        }
    }
}