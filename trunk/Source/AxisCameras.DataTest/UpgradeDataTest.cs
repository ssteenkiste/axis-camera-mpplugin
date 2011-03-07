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
using AxisCameras.Data;
using AxisCameras.Data.IO;
using AxisCameras.Data.MediaPortal;
using AxisCameras.Data.Upgrades;
using Moq;
using NUnit.Framework;

namespace AxisCameras.DataTest
{
	[TestFixture]
	public class UpgradeDataTest
	{
		private Mock<ISettings> settings;
		private Mock<IPartialUpgrade> upgradeToSecondVersion;
		private Mock<IPartialUpgrade> upgradeToThirdVersion;
		private Mock<IIOService> ioService;


		[SetUp]
		public void SetUp()
		{
			settings = new Mock<ISettings>();
			upgradeToSecondVersion = new Mock<IPartialUpgrade>();
			upgradeToThirdVersion = new Mock<IPartialUpgrade>();
			ioService = new Mock<IIOService>();

			// Partial upgrade from version 1 -> 2
			upgradeToSecondVersion
				.Setup(pu => pu.FromVersion)
				.Returns(1);
			upgradeToSecondVersion
				.Setup(pu => pu.ToVersion)
				.Returns(2);
			upgradeToSecondVersion
				.Setup(pu => pu.Upgrade())
				.Returns(true);

			// Partial upgrade from version 2 -> 3
			upgradeToThirdVersion
				.Setup(pu => pu.FromVersion)
				.Returns(2);
			upgradeToThirdVersion
				.Setup(pu => pu.ToVersion)
				.Returns(3);
			upgradeToThirdVersion
				.Setup(pu => pu.Upgrade())
				.Returns(true);
		}


		[Test]
		public void IsUpgradeRequiredWhenNeverInstalled()
		{
			ioService
				.Setup(ios => ios.FileExists(DataPersistenceInformation.FileName))
				.Returns(false);

			IUpgradeData upgradeData = new UpgradeData(
				settings.Object,
				new[] { upgradeToSecondVersion.Object, upgradeToThirdVersion.Object },
				ioService.Object);

			Assert.That(upgradeData.IsUpgradeRequired, Is.False);
		}


		[Test]
		public void IsUpgradeRequiredWhenOnSameVersion()
		{
			int currentVersion = 3;

			ioService
				.Setup(ios => ios.FileExists(DataPersistenceInformation.FileName))
				.Returns(true);

			settings
				.Setup(s => s.GetValueAsInt(
					DataPersistenceInformation.DatabaseSection.Name,
					DataPersistenceInformation.DatabaseSection.VersionEntry,
					1))
				.Returns(currentVersion);

			IUpgradeData upgradeData = new UpgradeData(
				settings.Object,
				new[] { upgradeToSecondVersion.Object, upgradeToThirdVersion.Object },
				ioService.Object);

			Assert.That(upgradeData.IsUpgradeRequired, Is.False);
		}


		[Test]
		public void IsUpgradeRequiredOnOlderVersion()
		{
			int currentVersion = 2;

			ioService
				.Setup(ios => ios.FileExists(DataPersistenceInformation.FileName))
				.Returns(true);

			settings
				.Setup(s => s.GetValueAsInt(
					DataPersistenceInformation.DatabaseSection.Name,
					DataPersistenceInformation.DatabaseSection.VersionEntry,
					1))
				.Returns(currentVersion);

			IUpgradeData upgradeData = new UpgradeData(
				settings.Object,
				new[] { upgradeToSecondVersion.Object, upgradeToThirdVersion.Object },
				ioService.Object);

			Assert.That(upgradeData.IsUpgradeRequired, Is.True);
		}


		[Test]
		public void UpgradeFromVersion1To3()
		{
			int currentVersion = 1;

			ioService
				.Setup(ios => ios.FileExists(DataPersistenceInformation.FileName))
				.Returns(true);

			settings
				.Setup(s => s.GetValueAsInt(
					DataPersistenceInformation.DatabaseSection.Name,
					DataPersistenceInformation.DatabaseSection.VersionEntry,
					1))
				.Returns(currentVersion);

			IUpgradeData upgradeData = new UpgradeData(
				settings.Object,
				new[] { upgradeToSecondVersion.Object, upgradeToThirdVersion.Object },
				ioService.Object);

			upgradeData.Upgrade();

			upgradeToSecondVersion.Verify(pu => pu.Upgrade(), Times.Once());
			upgradeToThirdVersion.Verify(pu => pu.Upgrade(), Times.Once());
		}


		[Test]
		public void UpgradeFromVersion2To3()
		{
			int currentVersion = 2;

			ioService
				.Setup(ios => ios.FileExists(DataPersistenceInformation.FileName))
				.Returns(true);

			settings
				.Setup(s => s.GetValueAsInt(
					DataPersistenceInformation.DatabaseSection.Name,
					DataPersistenceInformation.DatabaseSection.VersionEntry,
					1))
				.Returns(currentVersion);

			IUpgradeData upgradeData = new UpgradeData(
				settings.Object,
				new[] { upgradeToSecondVersion.Object, upgradeToThirdVersion.Object },
				ioService.Object);

			upgradeData.Upgrade();

			upgradeToSecondVersion.Verify(pu => pu.Upgrade(), Times.Never());
			upgradeToThirdVersion.Verify(pu => pu.Upgrade(), Times.Once());
		}


		[Test]
		public void UpgradeFromVersion3To3()
		{
			int currentVersion = 3;

			ioService
				.Setup(ios => ios.FileExists(DataPersistenceInformation.FileName))
				.Returns(true);

			settings
				.Setup(s => s.GetValueAsInt(
					DataPersistenceInformation.DatabaseSection.Name,
					DataPersistenceInformation.DatabaseSection.VersionEntry,
					1))
				.Returns(currentVersion);

			IUpgradeData upgradeData = new UpgradeData(
				settings.Object,
				new[] { upgradeToSecondVersion.Object, upgradeToThirdVersion.Object },
				ioService.Object);

			upgradeData.Upgrade();

			upgradeToSecondVersion.Verify(pu => pu.Upgrade(), Times.Never());
			upgradeToThirdVersion.Verify(pu => pu.Upgrade(), Times.Never());
		}
	}
}
