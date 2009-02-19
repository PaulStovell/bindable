using System;
using Bindable.Core.Helpers;
using MbUnit.Framework;

namespace Bindable.Core.Tests.Core.Helpers
{
    /// <summary>
    /// This class contains tests for the Bindable LINQ Weak Event implementation.
    /// </summary>
    [TestFixture]
    public sealed class WeakEventProxyTests
    {
        #region Test Helpers
        
        private sealed class EventPublisher
        {
            private event EventHandler _somethingHappened;

            public int Subscribers { get; set; }
            
            public event EventHandler SomethingHappened
            {
                add
                {
                    Subscribers++;
                    _somethingHappened += value;
                }
                remove
                {
                    Subscribers--;
                    _somethingHappened -= value;
                }
            }
        }

        private sealed class StandardEventSubscriber
        {
            private readonly EventPublisher _publisher;

            public StandardEventSubscriber(EventPublisher publisher)
            {
                _publisher = publisher;
                _publisher.SomethingHappened += Publisher_EventRaised;
            }

            ~StandardEventSubscriber()
            {
                _publisher.SomethingHappened -= Publisher_EventRaised;
            }

            private void Publisher_EventRaised(object sender, EventArgs e)
            {
                
            }
        }

        private sealed class WeakEventSubscriber
        {
            private readonly EventHandler<EventArgs> _eventHandler;
            private readonly EventPublisher _publisher;
            private readonly WeakEventProxy<EventArgs> _WeakEventProxy;

            public WeakEventSubscriber(EventPublisher publisher)
            {
                _publisher = publisher;

                // Create the event handlers. Note that these must be kept as member-level references,
                // so that they are coupled to the class lifetime rather than the current scope - or else
                // no one would reference the event handler (since the WeakEventProxy just keeps 
                // a weak reference to it)!
                _publisher.SomethingHappened += Weak.Event<EventArgs>(EventPublisher_EventRaised).HandlerProxy.Handler;
            }

            private void EventPublisher_EventRaised(object sender, EventArgs e) { }

            ~WeakEventSubscriber()
            {
                // Unhook the event. The publisher will then no longer reference the WeakEventProxy
                // either, and so that object will also be cleared - the end result is that we have cleaned 
                // after ourselves completely. NB: In a standard implementation you should also 
                // put this call in a Dispose() method.
                _publisher.SomethingHappened -= _WeakEventProxy.Handler;
            }
        }

        private static WeakReference CreateEventSubscriber(EventPublisher publisher)
        {
            var subscriber = new StandardEventSubscriber(publisher);
            Assert.AreEqual(1, publisher.Subscribers);
            return new WeakReference(subscriber);
        }

        private static WeakReference CreateWeakEventSubscriber(EventPublisher publisher)
        {
            var subscriber = new WeakEventSubscriber(publisher);
            Assert.AreEqual(1, publisher.Subscribers);
            return new WeakReference(subscriber);
        }

        #endregion

        /// <summary>
        /// Tests that standard .NET events do indeed cause referencing issues between 
        /// short-lived objects subscribing to events on long-lived objects.
        /// </summary>
        [Test]
        public void AssumptionWeakEventProxyStandardEventIntroducesLeaks()
        {
            // Create an event publisher (a long-lived object) and an event subscriber
            // using standard .NET events. We only have a weak reference to the subscriber, 
            // so the only other GC reference will be the publisher - this is what introduces
            // memory issues in binding and WPF applications.
            var publisher = new EventPublisher();
            var subscriberReference = CreateEventSubscriber(publisher);
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Prove that the subscriber is being kept alive by the publisher's reference back to it,
            // and that it is still subscribed (even though it unsubscribes in the finalizer - so it can't 
            // have been finalized).
            Assert.IsNotNull(subscriberReference.Target);
            Assert.AreEqual(1, publisher.Subscribers);
        }

        /// <summary>
        /// Tests that the Weak Event Reference implementation in Bindable LINQ solves the problem 
        /// shown in the above test.
        /// </summary>
        [Test]
        public void WeakEventProxyWeakEventsFixLeaks()
        {
            // Create an event publisher (a long-lived object) and an event subscriber
            // using our custom weak event handler code. We will only have a weak reference 
            // to the created subscriber, and since the weak event handler should remove the 
            // reference from the publisher to the subscriber, the subscriber should be 
            // marked for collection and finalized.
            var publisher = new EventPublisher();
            var subscriberReference = CreateWeakEventSubscriber(publisher);
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Prove that the weak event reference worked - the subscriber should not have been 
            // referenced from the publishers' event handler, and since we have no other normal 
            // references to it, it should have been collected and the finalizer should have 
            // unhooked the event handler.
            Assert.IsNull(subscriberReference.Target);
            Assert.AreEqual(0, publisher.Subscribers);
        }
    }
}