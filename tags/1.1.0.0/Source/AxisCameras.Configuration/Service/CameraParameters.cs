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

namespace AxisCameras.Configuration.Service
{
	/// <summary>
	/// Class containing parameters read from a camera.
	/// </summary>
	class CameraParameters
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CameraParameters"/>
		/// class.
		/// </summary>
		/// <param name="friendlyName">The friendly name.</param>
		/// <param name="firmwareVersion">The firmware version.</param>
		/// <param name="videoSourceCount">
		/// The video source count, i.e. the number of video source this camera (device actually) has.
		/// </param>
		public CameraParameters(
			string friendlyName,
			string firmwareVersion,
			int videoSourceCount)
		{
			FriendlyName = friendlyName;
			FirmwareVersion = firmwareVersion;
			VideoSourceCount = videoSourceCount;
		}


		/// <summary>
		/// Gets the friendly name.
		/// </summary>
		public string FriendlyName { get; private set; }


		/// <summary>
		/// Gets the firmware version.
		/// </summary>
		public string FirmwareVersion { get; private set; }


		/// <summary>
		/// Gets or sets the video source count, i.e. the number of video source this camera (device
		/// actually) has.
		/// </summary>
		public int VideoSourceCount { get; private set; }
	}
}
