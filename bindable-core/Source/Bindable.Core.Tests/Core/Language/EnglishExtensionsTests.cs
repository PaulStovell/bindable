using System;
using MbUnit.Framework;

namespace Bindable.Core.Tests.Core.Language
{
    [TestFixture]
    public class EnglishExtensionsTests
    {
        [Test]
        public void AllowedExamples()
        {
            Assert.AreEqual(0.ToEnglish(), "zero");
            Assert.AreEqual(1.ToEnglish(), "one");
            Assert.AreEqual(7.ToEnglish(), "seven");
            Assert.AreEqual(10.ToEnglish(), "ten");
            Assert.AreEqual(11.ToEnglish(), "eleven");
            Assert.AreEqual(20.ToEnglish(), "twenty");
            Assert.AreEqual(21.ToEnglish(), "twenty-one");
            Assert.AreEqual(84.ToEnglish(), "eighty-four");
            Assert.AreEqual(100.ToEnglish(), "one hundred");
            Assert.AreEqual(101.ToEnglish(), "one hundred and one");
            Assert.AreEqual(123.ToEnglish(), "one hundred and twenty-three");
            Assert.AreEqual(1000.ToEnglish(), "one thousand");
            Assert.AreEqual(1031.ToEnglish(), "one thousand and thirty-one");
            Assert.AreEqual(1231.ToEnglish(), "one thousand, two hundred and thirty-one");
            Assert.AreEqual(18947.ToEnglish(), "eighteen thousand, nine hundred and forty-seven");
            Assert.AreEqual(54947.ToEnglish(), "fifty-four thousand, nine hundred and forty-seven");
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NotAllowedExamples()
        {
            Assert.AreEqual((-1).ToEnglish(), "zero");
        }
    }
}