using System;
using Bindable.Core.Helpers;
using MbUnit.Framework;

namespace Bindable.Core.Tests.Core.Helpers
{
    /// <summary>
    /// This class contains unit tests for the <see cref="StateScope" />.
    /// </summary>
    [TestFixture]
    public sealed class StateScopeTests
    {
        /// <summary>
        /// Tests that entering a scope multiple times puts it into the state.
        /// </summary>
        [Test]
        public void StateScopeEntranceTest()
        {
            var scope = new StateScope();
            Assert.AreEqual(false, scope.IsWithin);
            using (scope.Enter())
            {
                Assert.AreEqual(true, scope.IsWithin);
                using (scope.Enter())
                {
                    Assert.AreEqual(true, scope.IsWithin);
                }
                Assert.AreEqual(true, scope.IsWithin);
            }
            Assert.AreEqual(false, scope.IsWithin);
        }

        /// <summary>
        /// Tests that entering a scope multiple times raises events.
        /// </summary>
        [Test]
        public void StateScopeEntranceTriggersCallback()
        {
            var eventsRaised = 0;
            Action callback = () => eventsRaised++;

            var scope = new StateScope(callback);
            Assert.AreEqual(0, eventsRaised);
            using (scope.Enter())
            {
                // Went from false to true - so it should have raised
                Assert.AreEqual(1, eventsRaised);
                using (scope.Enter())
                {
                    // Already in - no raise
                    Assert.AreEqual(1, eventsRaised);
                }
                // Still in - no raise
                Assert.AreEqual(1, eventsRaised);
            }
            // Left - raise
            Assert.AreEqual(2, eventsRaised);
        }
    }
}