#region Copyright (C) 2005-2010 Team MediaPortal

// Copyright (C) 2005-2010 Team MediaPortal
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
using AxisCameras.Data;
using NUnit.Framework;

namespace AxisCameras.DataTest
{
	[TestFixture]
	public class FirmwareVersionTest
	{
		[Test]
		public void Ctor()
		{
			Assert.Throws<ArgumentNullException>(() => new FirmwareVersion(null));
			Assert.Throws<ArgumentException>(() => new FirmwareVersion(string.Empty));
			Assert.Throws<ArgumentException>(() => new FirmwareVersion("4"));
			Assert.Throws<ArgumentException>(() => new FirmwareVersion("text"));
			Assert.Throws<ArgumentException>(() => new FirmwareVersion("text4.40"));
			Assert.Throws<ArgumentException>(() => new FirmwareVersion("text 4.40"));

			FirmwareVersion firmwareVersion = new FirmwareVersion("4.1");
			Assert.That(firmwareVersion.Major, Is.EqualTo(4));
			Assert.That(firmwareVersion.Minor, Is.EqualTo(1));
			Assert.That(firmwareVersion.Build, Is.EqualTo(0));
			Assert.That(firmwareVersion.Revision, Is.EqualTo(0));

			firmwareVersion = new FirmwareVersion("4.01");
			Assert.That(firmwareVersion.Major, Is.EqualTo(4));
			Assert.That(firmwareVersion.Minor, Is.EqualTo(1));
			Assert.That(firmwareVersion.Build, Is.EqualTo(0));
			Assert.That(firmwareVersion.Revision, Is.EqualTo(0));

			firmwareVersion = new FirmwareVersion("4.01.10");
			Assert.That(firmwareVersion.Major, Is.EqualTo(4));
			Assert.That(firmwareVersion.Minor, Is.EqualTo(1));
			Assert.That(firmwareVersion.Build, Is.EqualTo(10));
			Assert.That(firmwareVersion.Revision, Is.EqualTo(0));

			firmwareVersion = new FirmwareVersion("4.01.10.2");
			Assert.That(firmwareVersion.Major, Is.EqualTo(4));
			Assert.That(firmwareVersion.Minor, Is.EqualTo(1));
			Assert.That(firmwareVersion.Build, Is.EqualTo(10));
			Assert.That(firmwareVersion.Revision, Is.EqualTo(2));

			firmwareVersion = new FirmwareVersion("4.01.10.2.3");
			Assert.That(firmwareVersion.Major, Is.EqualTo(4));
			Assert.That(firmwareVersion.Minor, Is.EqualTo(1));
			Assert.That(firmwareVersion.Build, Is.EqualTo(10));
			Assert.That(firmwareVersion.Revision, Is.EqualTo(2));

			firmwareVersion = new FirmwareVersion("4.01.10.2.text");
			Assert.That(firmwareVersion.Major, Is.EqualTo(4));
			Assert.That(firmwareVersion.Minor, Is.EqualTo(1));
			Assert.That(firmwareVersion.Build, Is.EqualTo(10));
			Assert.That(firmwareVersion.Revision, Is.EqualTo(2));

			firmwareVersion = new FirmwareVersion("4.01.10.2text");
			Assert.That(firmwareVersion.Major, Is.EqualTo(4));
			Assert.That(firmwareVersion.Minor, Is.EqualTo(1));
			Assert.That(firmwareVersion.Build, Is.EqualTo(10));
			Assert.That(firmwareVersion.Revision, Is.EqualTo(2));

			firmwareVersion = new FirmwareVersion("4.01.10.2_text");
			Assert.That(firmwareVersion.Major, Is.EqualTo(4));
			Assert.That(firmwareVersion.Minor, Is.EqualTo(1));
			Assert.That(firmwareVersion.Build, Is.EqualTo(10));
			Assert.That(firmwareVersion.Revision, Is.EqualTo(2));

			firmwareVersion = new FirmwareVersion("4.01.10.2-text");
			Assert.That(firmwareVersion.Major, Is.EqualTo(4));
			Assert.That(firmwareVersion.Minor, Is.EqualTo(1));
			Assert.That(firmwareVersion.Build, Is.EqualTo(10));
			Assert.That(firmwareVersion.Revision, Is.EqualTo(2));

			firmwareVersion = new FirmwareVersion("4.01.10.2 text");
			Assert.That(firmwareVersion.Major, Is.EqualTo(4));
			Assert.That(firmwareVersion.Minor, Is.EqualTo(1));
			Assert.That(firmwareVersion.Build, Is.EqualTo(10));
			Assert.That(firmwareVersion.Revision, Is.EqualTo(2));
		}


		[Test]
		public void TryParse()
		{
			FirmwareVersion firmwareVersion;

			Assert.That(FirmwareVersion.TryParse(null, out firmwareVersion), Is.False);
			Assert.That(FirmwareVersion.TryParse(string.Empty, out firmwareVersion), Is.False);
			Assert.That(FirmwareVersion.TryParse("4", out firmwareVersion), Is.False);
			Assert.That(FirmwareVersion.TryParse("text", out firmwareVersion), Is.False);
			Assert.That(FirmwareVersion.TryParse("text4.40", out firmwareVersion), Is.False);
			Assert.That(FirmwareVersion.TryParse("text 4.40", out firmwareVersion), Is.False);

			Assert.That(FirmwareVersion.TryParse("4.1", out firmwareVersion), Is.True);
			Assert.That(firmwareVersion.Major, Is.EqualTo(4));
			Assert.That(firmwareVersion.Minor, Is.EqualTo(1));
			Assert.That(firmwareVersion.Build, Is.EqualTo(0));
			Assert.That(firmwareVersion.Revision, Is.EqualTo(0));

			Assert.That(FirmwareVersion.TryParse("4.01", out firmwareVersion), Is.True);
			Assert.That(firmwareVersion.Major, Is.EqualTo(4));
			Assert.That(firmwareVersion.Minor, Is.EqualTo(1));
			Assert.That(firmwareVersion.Build, Is.EqualTo(0));
			Assert.That(firmwareVersion.Revision, Is.EqualTo(0));

			Assert.That(FirmwareVersion.TryParse("4.01.10", out firmwareVersion), Is.True);
			Assert.That(firmwareVersion.Major, Is.EqualTo(4));
			Assert.That(firmwareVersion.Minor, Is.EqualTo(1));
			Assert.That(firmwareVersion.Build, Is.EqualTo(10));
			Assert.That(firmwareVersion.Revision, Is.EqualTo(0));

			Assert.That(FirmwareVersion.TryParse("4.01.10.2", out firmwareVersion), Is.True);
			Assert.That(firmwareVersion.Major, Is.EqualTo(4));
			Assert.That(firmwareVersion.Minor, Is.EqualTo(1));
			Assert.That(firmwareVersion.Build, Is.EqualTo(10));
			Assert.That(firmwareVersion.Revision, Is.EqualTo(2));

			Assert.That(FirmwareVersion.TryParse("4.01.10.2.3", out firmwareVersion), Is.True);
			Assert.That(firmwareVersion.Major, Is.EqualTo(4));
			Assert.That(firmwareVersion.Minor, Is.EqualTo(1));
			Assert.That(firmwareVersion.Build, Is.EqualTo(10));
			Assert.That(firmwareVersion.Revision, Is.EqualTo(2));

			Assert.That(FirmwareVersion.TryParse("4.01.10.2.text", out firmwareVersion), Is.True);
			Assert.That(firmwareVersion.Major, Is.EqualTo(4));
			Assert.That(firmwareVersion.Minor, Is.EqualTo(1));
			Assert.That(firmwareVersion.Build, Is.EqualTo(10));
			Assert.That(firmwareVersion.Revision, Is.EqualTo(2));

			Assert.That(FirmwareVersion.TryParse("4.01.10.2text", out firmwareVersion), Is.True);
			Assert.That(firmwareVersion.Major, Is.EqualTo(4));
			Assert.That(firmwareVersion.Minor, Is.EqualTo(1));
			Assert.That(firmwareVersion.Build, Is.EqualTo(10));
			Assert.That(firmwareVersion.Revision, Is.EqualTo(2));

			Assert.That(FirmwareVersion.TryParse("4.01.10.2_text", out firmwareVersion), Is.True);
			Assert.That(firmwareVersion.Major, Is.EqualTo(4));
			Assert.That(firmwareVersion.Minor, Is.EqualTo(1));
			Assert.That(firmwareVersion.Build, Is.EqualTo(10));
			Assert.That(firmwareVersion.Revision, Is.EqualTo(2));

			Assert.That(FirmwareVersion.TryParse("4.01.10.2-text", out firmwareVersion), Is.True);
			Assert.That(firmwareVersion.Major, Is.EqualTo(4));
			Assert.That(firmwareVersion.Minor, Is.EqualTo(1));
			Assert.That(firmwareVersion.Build, Is.EqualTo(10));
			Assert.That(firmwareVersion.Revision, Is.EqualTo(2));

			Assert.That(FirmwareVersion.TryParse("4.01.10.2 text", out firmwareVersion), Is.True);
			Assert.That(firmwareVersion.Major, Is.EqualTo(4));
			Assert.That(firmwareVersion.Minor, Is.EqualTo(1));
			Assert.That(firmwareVersion.Build, Is.EqualTo(10));
			Assert.That(firmwareVersion.Revision, Is.EqualTo(2));
		}
	}
}
