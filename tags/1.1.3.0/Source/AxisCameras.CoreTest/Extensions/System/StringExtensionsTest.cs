using System;
using System.Globalization;
using NUnit.Framework;

namespace AxisCameras.CoreTest.Extensions.System
{
    [TestFixture]
    public class StringExtensionsTest
    {
        private const string Format = "Current time: {0}";
        private readonly DateTime currentTime = DateTime.Now;

        [Test]
        public void InvariantFormat()
        {
            // ACT
            string formattedText = Format.InvariantFormat(currentTime);

            // ASSERT
            Assert.That(formattedText, Is.EqualTo(string.Format(CultureInfo.InvariantCulture, Format, currentTime)));
        }

        [Test]
        public void CurrentFormat()
        {
            // ACT
            string formattedText = Format.CurrentFormat(currentTime);

            // ASSERT
            Assert.That(formattedText, Is.EqualTo(string.Format(CultureInfo.CurrentCulture, Format, currentTime)));
        }
    }
}