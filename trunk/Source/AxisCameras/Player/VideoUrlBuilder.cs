﻿#region Copyright (C) 2005-2010 Team MediaPortal

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
using System.Diagnostics.CodeAnalysis;
using System.Web;
using AxisCameras.Core;

namespace AxisCameras.Player
{
	/// <summary>
	/// Class capable of creating video URLs.
	/// </summary>
	public class VideoUrlBuilder : IVideoUrlBuilder
	{
		/// <summary>
		/// Builds a live video URL.
		/// </summary>
		/// <param name="address">The network address.</param>
		/// <param name="port">The network port.</param>
		/// <param name="userName">The user name.</param>
		/// <param name="password">The password.</param>
		/// <param name="firmwareVersion">The firmware version.</param>
		/// <returns>The live video URL used when getting live video from the camera.</returns>
		public string BuildLiveVideoUrl(
			string address,
			int port,
			string userName,
			string password,
			string firmwareVersion)
		{
			if (string.IsNullOrEmpty(address)) throw new ArgumentNullException("address");
			if (port < 0 || port > 65535) throw new ArgumentException("Port must be between 0-65535", "port");
			if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException("userName");
			if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password");
			if (string.IsNullOrEmpty(firmwareVersion)) throw new ArgumentNullException("firmwareVersion");

			Version version = ParseFirmwareVersion(firmwareVersion);

			string urlFormat;
			if (version.Major >= 5)
			{
				// Firmware version 5.0 or newer means that the camera supports H.264
				Log.Debug("Camera firmware version is {0}, use H.264.", firmwareVersion);
				urlFormat = Vapix.Version3.H264VideoUrl;
			}
			else if (version.Major >= 4)
			{
				// Firmware version 4.0 or newer means that the camera supports MPEG-4
				Log.Debug("Camera firmware version is {0}, use MPEG-4.", firmwareVersion);
				urlFormat = Vapix.Version2.Mpeg4VideoUrl;
			}
			else
			{
				// Firmware version older than 4.0 means that the camera supports M-JPEG
				Log.Debug("Camera firmware version is {0}, use M-JPEG.", firmwareVersion);
				urlFormat = Vapix.Version2.MjpegVideoUrl;
			}

			return urlFormat.InvariantFormat(
				HttpUtility.UrlEncode(userName),
				HttpUtility.UrlEncode(password),
				address,
				port);
		}


		/// <summary>
		/// Parses the firmware version.
		/// </summary>
		/// <param name="firmwareVersion">The firmware version.</param>
		/// <returns>
		/// The firmware version if successfully parsed; otherwise defaulting to version 5.0, i.e.
		/// camera supports VAPIX 3.
		/// </returns>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Easier to catch general exception instead of specifying all unique.")]
		private static Version ParseFirmwareVersion(string firmwareVersion)
		{
			try
			{
				return new Version(firmwareVersion);
			}
			catch (Exception e)
			{
				Log.Error("VideoUrlBuilder - Unable to parse firmware version '{0}', defaulting to 5.0. {1}",
					firmwareVersion,
					e.ToString());

				// If firmware version cannot be parsed, assume it is a new beta, i.e. the VAPIX 3 live
				// video URL should be used
				return new Version(5, 0);
			}
		}
	}
}
