﻿#region Copyright (C) 2005-2011 Team MediaPortal

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
using System.IO;
using System.Reflection;
using Autofac;
using AxisCameras.Data;
using AxisCameras.Data.MediaPortal;
using NUnit.Framework;

namespace AxisCameras.DataTest
{
	/// <summary>
	/// This class does not contain the usual unit tests, the name of the file does not even match
	/// any class in the AxisCameras.Data namespace. It is containing function tests, verifying
	/// that the data is upgraded correctly between the very real versions of the plugin.
	/// </summary>
	[TestFixture]
	public class UpgradeTest
	{
		private IContainer container;


		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			Assembly assembly = Assembly.Load("AxisCameras.Data");

			ContainerBuilder builder = new ContainerBuilder();
			builder
				.RegisterAssemblyTypes(assembly);
			builder
				.RegisterAssemblyTypes(assembly)
				.AsImplementedInterfaces();

			container = builder.Build();

			DataPersistenceInformation.FileName = Path.Combine(OutputPath, "AxisCameras.xml");
		}


		[TearDown]
		public void TearDown()
		{
			File.Delete(DataPersistenceInformation.FileName);
		}


		[Test]
		public void UpgradeFromVersion1To2()
		{
			string version1FileName = Path.Combine(OutputPath, @"Versions\Version1\AxisCameras.xml");
			string version2FileName = Path.Combine(OutputPath, @"Versions\Version2\AxisCameras.xml");

			// Copy file of version 1
			File.Copy(version1FileName, DataPersistenceInformation.FileName);

			UpgradeData upgradeData = container.Resolve<UpgradeData>();
			ISettings settings = container.Resolve<ISettings>();

			// Upgrade from version 1 to 2	
			Assert.That(upgradeData.IsUpgradeRequired, Is.True);
			Assert.That(upgradeData.Upgrade(), Is.True);

			// Save settings to disk
			settings.Save();

			// Upgrade is now finished, validate with file known to be correct
			Assert.That(
				File.ReadAllText(DataPersistenceInformation.FileName),
				Is.EqualTo(File.ReadAllText(version2FileName)));
		}


		private static string OutputPath
		{
			get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
		}
	}
}
