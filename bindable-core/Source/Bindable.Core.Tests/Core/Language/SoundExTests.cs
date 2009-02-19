using Bindable.Core.Language;
using MbUnit.Framework;

namespace Bindable.Core.Tests.Core.Language
{
    [TestFixture]
    public class SoundExTests
    {
        /// <remarks>
        /// For more information on the algorithm, see:
        /// http://www.archives.gov/publications/general-info-leaflets/55.html
        /// </remarks>
        [Test]
        public void EvaluationExamples()
        {
            // Similar looking words
            Assert.AreEqual(SoundEx.Evaluate("Hello"), "H400");
            Assert.AreEqual(SoundEx.Evaluate("Hallo"), "H400");
            Assert.AreEqual(SoundEx.Evaluate("Halle"), "H400");
            Assert.AreEqual(SoundEx.Evaluate("Hellloooo"), "H400");
            Assert.AreEqual(SoundEx.Evaluate("Hell"), "H400");
            Assert.AreEqual(SoundEx.Evaluate("H@#@#  ell4303@#$@^#$#$ o"), "H400");

            // Edge cases outlined in the link above
            Assert.AreEqual(SoundEx.Evaluate("Gutierrez"), "G362");
            Assert.AreEqual(SoundEx.Evaluate("Pfister"), "P236");
            Assert.AreEqual(SoundEx.Evaluate("Jackson"), "J250");
            Assert.AreEqual(SoundEx.Evaluate("Tymczak"), "T522");
            Assert.AreEqual(SoundEx.Evaluate("Tymczak", 20), "T522");
            Assert.AreEqual(SoundEx.Evaluate("Ashcraft"), "A261");
            Assert.AreEqual(SoundEx.Evaluate("Ashcraft", 20), "A2613");
        }

        [Test]
        public void ComparisonExamples()
        {
            Assert.IsTrue(SoundEx.Evaluate("Hello") == SoundEx.Evaluate("Helo"));
            Assert.IsTrue(SoundEx.Evaluate("Hello") != SoundEx.Evaluate("Jelko"));
            Assert.IsTrue(SoundEx.Evaluate("Hello").Equals("Helo"));
            Assert.IsTrue(SoundEx.Evaluate("Hello").Equals(SoundEx.Evaluate("Helo")));
            Assert.IsFalse(SoundEx.Evaluate("Hello").Equals(SoundEx.Evaluate("Jelo")));
            Assert.IsTrue(SoundEx.Evaluate("Kello").Equals("Jello", SoundExComparison.IncludeSimilarFirstLetter));
            Assert.IsTrue(SoundEx.Evaluate("Kello").Equals(SoundEx.Evaluate("Jello"), SoundExComparison.IncludeSimilarFirstLetter));
            Assert.IsFalse(SoundEx.Evaluate("Kello").Equals(SoundEx.Evaluate("Hello983989c8sdddsdij")));
            Assert.IsTrue(SoundEx.Evaluate("Hello").Equals(SoundEx.Evaluate("Hello983989c8sdddsdij"), SoundExComparison.IncludeLongerWords));
        }
    }
}