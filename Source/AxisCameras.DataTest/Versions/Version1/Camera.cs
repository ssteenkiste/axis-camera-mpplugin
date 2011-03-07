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

namespace AxisCameras.DataTest.Versions.Version1
{
	/// <summary>
	/// Class describing the properties of a camera.
	/// </summary>
	public class Camera
	{
		/// <summary>
		/// Gets or sets the Id.
		/// </summary>
		public Guid Id { get; set; }


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
		/// Gets or sets the snapshot path.
		/// </summary>
		public string SnapshotPath { get; set; }
	}
}
