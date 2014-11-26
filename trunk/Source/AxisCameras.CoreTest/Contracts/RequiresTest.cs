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

using AxisCameras.Core.Contracts;
using NUnit.Framework;

namespace AxisCameras.CoreTest.Contracts
{
    /// <summary>
    /// Tests for <see cref="Requires"/>.
    /// </summary>
    [TestFixture]
    public class RequiresTest
    {
        [Test]
        public void True()
        {
            Requires.True(true);
            Requires.True(true, null);
            Requires.True(true, "Error message");

            Assert.Throws<RequiresException>(() => Requires.True(false));
            Assert.Throws<RequiresException>(() => Requires.True(false, null));
            Assert.Throws<RequiresException>(() => Requires.True(false, "Error message"));
        }

        [Test]
        public void NotNullOrEmpty()
        {
            Requires.NotNullOrEmpty("t");
            Requires.NotNullOrEmpty("text");

            Assert.Throws<RequiresException>(() => Requires.NotNullOrEmpty(null));
            Assert.Throws<RequiresException>(() => Requires.NotNullOrEmpty(string.Empty));
        }

        [Test]
        public void NotNull()
        {
            Requires.NotNull("t");
            Requires.NotNull(new object());

            object value = null;
            Assert.Throws<RequiresException>(() => Requires.NotNull(value));
        }

        [Test]
        public void Fail()
        {
            Assert.Throws<RequiresException>(() => Requires.Fail());
            Assert.Throws<RequiresException>(() => Requires.Fail("Error message"));
        }
    }
}