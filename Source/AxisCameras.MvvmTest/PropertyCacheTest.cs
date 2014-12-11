#region Copyright (C) 2005-2014 Team MediaPortal

// Copyright (C) 2005-2014 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

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