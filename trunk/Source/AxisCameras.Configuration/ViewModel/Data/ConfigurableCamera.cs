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

using System;
using System.Collections.Generic;
using System.Linq;
using AxisCameras.Core.Contracts;
using AxisCameras.Data;

namespace AxisCameras.Configuration.ViewModel.Data
{
    /// <summary>
    /// Class describing a configurable camera.
    /// </summary>
    public class ConfigurableCamera
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurableCamera"/> class.
        /// </summary>
        /// <param name="id">The camera id.</param>
        public ConfigurableCamera(Guid id)
        {
            Requires.True(id != Guid.Empty, "ID cannot be Guid.Empty");

            Id = id;
        }

        /// <summary>
        /// Gets the Id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the HTTP port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the video source. This property will always be 1 on cameras, but can be other
        /// than 1 on video servers with multiple camera inputs.
        /// </summary>
        public int VideoSource { get; set; }

        /// <summary>
        /// Gets or sets the video source count, i.e. the number of video source this camera (device
        /// actually) has.
        /// </summary>
        public int VideoSourceCount { get; set; }

        /// <summary>
        /// Gets or sets the video capabilities.
        /// </summary>
        public VideoCapabilities VideoCapabilities { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the firmware version.
        /// </summary>
        public string FirmwareVersion { get; set; }

        /// <summary>
        /// Gets or sets the snapshot.
        /// </summary>
        public IEnumerable<byte> Snapshot { get; set; }

        /// <summary>
        /// Creates a new object that is a deep copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a deep copy of this instance.
        /// </returns>
        public ConfigurableCamera Clone()
        {
            return new ConfigurableCamera(Id)
            {
                Name = Name,
                Address = Address,
                Port = Port,
                VideoSource = VideoSource,
                VideoSourceCount = VideoSourceCount,
                VideoCapabilities = VideoCapabilities,
                UserName = UserName,
                Password = Password,
                FirmwareVersion = FirmwareVersion,
                // Use ToList() to evaluate the Linq expression now, rather than when it is used for the
                // first time. One expects the clone method to clone the data when the method is executed,
                // not anytime later.
                Snapshot = Snapshot.Select(data => data).ToList()
            };
        }
    }
}