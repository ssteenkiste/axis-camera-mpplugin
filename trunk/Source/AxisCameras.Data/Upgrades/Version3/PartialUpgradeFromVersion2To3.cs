#region Copyright (C) 2005-2011 Team MediaPortal

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
using Version2Camera = AxisCameras.Data.Upgrades.Version2.Camera;

namespace AxisCameras.Data.Upgrades.Version3
{
    /// <summary>
    /// Partial upgrade from version 2 to 3.
    /// </summary>
    internal class PartialUpgradeFromVersion2To3 : PartialUpgrade
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartialUpgradeFromVersion2To3"/> class.
        /// </summary>
        /// <param name="settings">The MediaPortal settings.</param>
        public PartialUpgradeFromVersion2To3(ISettings settings)
            : base(settings)
        {
        }

        /// <summary>
        /// Gets the version after the upgrade.
        /// </summary>
        public override int ToVersion
        {
            get { return 3; }
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
                // Get version 2 cameras from file
                string serializedVersion2Cameras = GetValue(
                    DataPersistenceInformation.CameraSection.Name,
                    DataPersistenceInformation.CameraSection.CamerasEntry);

                // Deserialize version 2 cameras
                var version2Cameras = Deserialize<List<Version2Camera>>(serializedVersion2Cameras);

                // Convert version 2 cameras into version 3 cameras. New properties:
                //
                //   - VideoCapabilities    Calculate video capabilities based on firmware version.
                IEnumerable<Camera> version3Cameras = version2Cameras.Select(
                    version2Camera =>
                        new Camera
                        {
                            Id = version2Camera.Id,
                            Name = version2Camera.Name,
                            Address = version2Camera.Address,
                            Port = version2Camera.Port,
                            VideoSource = version2Camera.VideoSource,
                            VideoSourceCount = version2Camera.VideoSourceCount,
                            VideoCapabilities = GetVideoCapabilities(version2Camera.FirmwareVersion),
                            UserName = version2Camera.UserName,
                            Password = version2Camera.Password,
                            FirmwareVersion = version2Camera.FirmwareVersion
                        });

                // Serialize version 3 cameras
                string serializedVersion3Cameras = Serialize(version3Cameras.ToList());

                // Save version 3 cameras to file
                SetValue(
                    DataPersistenceInformation.CameraSection.Name,
                    DataPersistenceInformation.CameraSection.CamerasEntry,
                    serializedVersion3Cameras);
            }
            catch (Exception e)
            {
                Log.Error("Error when upgrading from version 2 to 3.", e);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines the video capabilities based on the firmware version.
        /// </summary>
        private static VideoCapabilities GetVideoCapabilities(string firmwareVersionText)
        {
            FirmwareVersion firmwareVersion;

            if (!FirmwareVersion.TryParse(firmwareVersionText, out firmwareVersion))
            {
                // If firmware version cannot be parsed, assume it is a new beta, i.e. firmware version 5.0
                firmwareVersion = new FirmwareVersion("5.0");
            }

            // Firmware version 5.0 or newer means that the camera supports H.264, firmware version
            // below 5.0 means that the camera supports MPEG-4
            return firmwareVersion.Major >= 5
                ? VideoCapabilities.H264
                : VideoCapabilities.Mpeg4;
        }
    }
}