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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AxisCameras.Core;
using AxisCameras.Data.MediaPortal;
using Version1Camera = AxisCameras.Data.Upgrades.Version1.Camera;

namespace AxisCameras.Data.Upgrades.Version2
{
	/// <summary>
	/// Partial upgrade from version 1 to 2.
	/// </summary>
	class PartialUpgradeFromVersion1To2 : PartialUpgrade
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PartialUpgradeFromVersion1To2"/> class.
		/// </summary>
		/// <param name="settings">The MediaPortal settings.</param>
		public PartialUpgradeFromVersion1To2(ISettings settings)
			: base(settings)
		{
		}


		/// <summary>
		/// Gets the version after the upgrade.
		/// </summary>
		public override int ToVersion
		{
			get { return 2; }
		}


		/// <summary>
		/// Upgrades the data.
		/// </summary>
		/// <returns>true if upgrade was successful; otherwise false.</returns>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "I am lazy and unwilling to find out what exceptions types that are thrown.")]
		public override bool Upgrade()
		{
			try
			{
				// Get version 1 cameras from file
				string serializedVersion1Cameras = GetValue(
					DataPersistenceInformation.CameraSection.Name,
					DataPersistenceInformation.CameraSection.CamerasEntry);

				// Deserialize version 1 cameras
				List<Version1Camera> version1Cameras = Deserialize<List<Version1Camera>>(
					serializedVersion1Cameras);

				// Convert version 1 cameras into version 2 cameras. A new property 'VideoSource' has been
				// added and the default value of that property is 1.
				IEnumerable<Camera> version2Cameras = version1Cameras.Select(version1Camera =>
					new Camera
					{
						Id = version1Camera.Id,
						Name = version1Camera.Name,
						Address = version1Camera.Address,
						Port = version1Camera.Port,
						VideoSource = 1,
						UserName = version1Camera.UserName,
						Password = version1Camera.Password,
						FirmwareVersion = version1Camera.FirmwareVersion,
						SnapshotPath = version1Camera.SnapshotPath
					});

				// Serialize version 2 cameras
				string serializedVersion2Cameras = Serialize<List<Camera>>(version2Cameras.ToList());

				// Save version 2 cameras to file
				SetValue(
					DataPersistenceInformation.CameraSection.Name,
					DataPersistenceInformation.CameraSection.CamerasEntry,
					serializedVersion2Cameras);
			}
			catch (Exception e)
			{
				Log.Error("Error when upgrading from version 1 to 2. {0}", e.ToString());
				return false;
			}

			return true;
		}
	}
}
