#region Copyright (C) 2005-2015 Team MediaPortal

// Copyright (C) 2005-2015 Team MediaPortal
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

using AxisCameras.Core.Contracts;
using AxisCameras.Data;
using NUnit.Framework;

namespace AxisCameras.DataTest
{
    [TestFixture]
    public class FirmwareVersionTest
    {
        [Test]
        public void InvalidConstructor()
        {
            // ASSERT
            Assert.Throws<RequiresException>(() => new FirmwareVersion(null));
            Assert.Throws<RequiresException>(() => new FirmwareVersion(string.Empty));
            Assert.Throws<RequiresException>(() => new FirmwareVersion("4"));
            Assert.Throws<RequiresException>(() => new FirmwareVersion("text"));
            Assert.Throws<RequiresException>(() => new FirmwareVersion("text4.40"));
            Assert.Throws<RequiresException>(() => new FirmwareVersion("text 4.40"));
        }

        [TestCase("4.1", 4, 1, 0, 0)]
        [TestCase("4.01", 4, 1, 0, 0)]
        [TestCase("4.01.10", 4, 1, 10, 0)]
        [TestCase("4.01.10.2", 4, 1, 10, 2)]
        [TestCase("4.01.10.2.3", 4, 1, 10, 2)]
        [TestCase("4.01.10.2.text", 4, 1, 10, 2)]
        [TestCase("4.01.10.2text", 4, 1, 10, 2)]
        [TestCase("4.01.10.2_text", 4, 1, 10, 2)]
        [TestCase("4.01.10.2-text", 4, 1, 10, 2)]
        [TestCase("4.01.10.2 text", 4, 1, 10, 2)]
        public void ValidConstructor(
            string version,
            int expectedMajor,
            int expectedMinor,
            int expectedBuild,
            int expectedRevision)
        {
            // ACT
            var firmwareVersion = new FirmwareVersion(version);
            
            // ASSERT
            Assert.That(firmwareVersion.Major, Is.EqualTo(expectedMajor));
            Assert.That(firmwareVersion.Minor, Is.EqualTo(expectedMinor));
            Assert.That(firmwareVersion.Build, Is.EqualTo(expectedBuild));
            Assert.That(firmwareVersion.Revision, Is.EqualTo(expectedRevision));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("4")]
        [TestCase("text")]
        [TestCase("text4.40")]
        [TestCase("text 4.40")]
        public void TryParseFailure(string input)
        {
            // ARRANGE
            FirmwareVersion firmwareVersion;

            // ASSERT
            Assert.That(FirmwareVersion.TryParse(input, out firmwareVersion), Is.False);
        }

        [TestCase("4.1", 4, 1, 0, 0)]
        [TestCase("4.01", 4, 1, 0, 0)]
        [TestCase("4.01.10", 4, 1, 10, 0)]
        [TestCase("4.01.10.2", 4, 1, 10, 2)]
        [TestCase("4.01.10.2.3", 4, 1, 10, 2)]
        [TestCase("4.01.10.2.text", 4, 1, 10, 2)]
        [TestCase("4.01.10.2text", 4, 1, 10, 2)]
        [TestCase("4.01.10.2_text", 4, 1, 10, 2)]
        [TestCase("4.01.10.2-text", 4, 1, 10, 2)]
        [TestCase("4.01.10.2 text", 4, 1, 10, 2)]
        public void TryParseSuccess(
            string input,
            int expectedMajor,
            int expectedMinor,
            int expectedBuild,
            int expectedRevision)
        {
            // ARRANGE
            FirmwareVersion firmwareVersion;

            // ASSERT
            Assert.That(FirmwareVersion.TryParse(input, out firmwareVersion), Is.True);
            Assert.That(firmwareVersion.Major, Is.EqualTo(expectedMajor));
            Assert.That(firmwareVersion.Minor, Is.EqualTo(expectedMinor));
            Assert.That(firmwareVersion.Build, Is.EqualTo(expectedBuild));
            Assert.That(firmwareVersion.Revision, Is.EqualTo(expectedRevision));
        }
    }
}