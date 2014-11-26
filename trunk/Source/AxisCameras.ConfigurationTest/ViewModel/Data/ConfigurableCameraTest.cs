#region Copyright (C) 2005-2011 Team MediaPortal

// Copyright (C) 2005-2011 Team MediaPortal
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

using System;
using System.Reflection;
using AxisCameras.Configuration.ViewModel.Data;
using AxisCameras.Data;
using NUnit.Framework;

namespace AxisCameras.ConfigurationTest.ViewModel.Data
{
    [TestFixture]
    public class ConfigurableCameraTest
    {
        /// <summary>
        /// Tests that the properties of a cloned ConfigurableCamera are equal to the ones on the
        /// source.
        /// </summary>
        [Test]
        public void Clone()
        {
            var camera = new ConfigurableCamera(Guid.NewGuid())
            {
                Name = "Name",
                Address = "192.168.0.90",
                Port = 80,
                VideoSource = 1,
                VideoSourceCount = 1,
                VideoCapabilities = VideoCapabilities.H264 | VideoCapabilities.Mpeg4,
                UserName = "root",
                Password = "pass",
                FirmwareVersion = "5.0",
                Snapshot = new byte[]
                {
                    0, 1, 2, 3
                }
            };

            // Assert that all properties are set, it is so easy to forget to update the test when adding
            // a new property
            foreach (var propertyInfo in GetProperties(typeof(ConfigurableCamera)))
            {
                var cameraValue = propertyInfo.GetValue(camera, null);
                var defaultValue = GetDefault(propertyInfo.PropertyType);

                Assert.That(cameraValue, Is.Not.EqualTo(defaultValue));
            }

            // Assert that all properties are cloned
            ConfigurableCamera clone = camera.Clone();
            foreach (var propertyInfo in GetProperties(typeof(ConfigurableCamera)))
            {
                var clonedValue = propertyInfo.GetValue(clone, null);
                var originalValue = propertyInfo.GetValue(camera, null);

                Assert.That(clonedValue, Is.EqualTo(originalValue));
            }
        }

        /// <summary>
        /// Gets all public and internal properties of specified type.
        /// </summary>
        private static PropertyInfo[] GetProperties(Type type)
        {
            return type.GetProperties(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance);
        }

        /// <summary>
        /// Gets the default value of the specified type.
        /// </summary>
        private static object GetDefault(Type type)
        {
            return type.IsValueType
                ? Activator.CreateInstance(type)
                : null;
        }
    }
}