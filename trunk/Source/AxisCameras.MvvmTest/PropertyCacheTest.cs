using AxisCameras.Mvvm;
using NUnit.Framework;

namespace AxisCameras.MvvmTest
{
    [TestFixture]
    public class PropertyCacheTest
    {
        private PropertyCache propertyCache;

        [SetUp]
        public void SetUp()
        {
            propertyCache = new PropertyCache();
        }

        [Test]
        public void GetUnknownValue()
        {
            // ACT
            var value = propertyCache.GetValue<int>("Unknown");

            // ASSERT
            Assert.That(value, Is.EqualTo(default(int)));
        }

        [Test]
        public void GetKnownValue()
        {
            // ARRANGE
            propertyCache.SetValue("Known", 42);

            // ACT
            var value = propertyCache.GetValue<int>("Known");

            // ASSERT
            Assert.That(value, Is.EqualTo(42));
        }

        [Test]
        public void SetNewValue()
        {
            // ARRANGE
            propertyCache.SetValue("Known", 1);

            // ACT
            bool success = propertyCache.SetValue("Known", 2);

            // ASSERT
            Assert.That(success, Is.True);
            Assert.That(propertyCache.GetValue<int>("Known"), Is.EqualTo(2));
        }

        [Test]
        public void SetOldValue()
        {
            // ARRANGE
            propertyCache.SetValue("Known", 1);

            // ACT
            bool success = propertyCache.SetValue("Known", 1);

            // ASSERT
            Assert.That(success, Is.False);
            Assert.That(propertyCache.GetValue<int>("Known"), Is.EqualTo(1));
        }
    }
}