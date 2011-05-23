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
using System.Web;
using AxisCameras.Core.Contracts;
using AxisCameras.Data;

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
		/// <param name="videoCapabilities">The video capabilities.</param>
		/// <param name="videoSource">
		/// The video source of the camera. This property should always be 1 on cameras, but can be
		/// other than 1 on video servers with multiple camera inputs.
		/// </param>
		/// <returns>The live video URL used when getting live video from the camera.</returns>
		public string BuildLiveVideoUrl(
			string address,
			int port,
			string userName,
			string password,
			VideoCapabilities videoCapabilities,
			int videoSource)
		{
			Requires.IsNotNullOrEmpty(address);
			Requires.IsTrue(port >= 0 && port < 65536, "Port must be between 0-65535");
			Requires.IsNotNullOrEmpty(userName);
			Requires.IsNotNullOrEmpty(password);
			Requires.IsTrue(videoSource >= 1, "Video source must be at least 1.");

			string urlFormat = GetUrlFormat(videoCapabilities);

			return urlFormat.InvariantFormat(
				HttpUtility.UrlEncode(userName),
				HttpUtility.UrlEncode(password),
				address,
				port,
				videoSource);
		}


		/// <summary>
		/// Gets the video URL format based on specified video capabilities.
		/// </summary>
		/// <param name="videoCapabilities">The video capabilities.</param>
		/// <returns>The video URL format based on specified video capabilities.</returns>
		private static string GetUrlFormat(VideoCapabilities videoCapabilities)
		{
			if (HasFlag(VideoCapabilities.H264, videoCapabilities))
			{
				return Vapix.Version3.H264VideoUrl;
			}

			if (HasFlag(VideoCapabilities.Mpeg4, videoCapabilities))
			{
				return Vapix.Version2.Mpeg4VideoUrl;
			}

			if (HasFlag(VideoCapabilities.Mjpeg, videoCapabilities))
			{
				return Vapix.Version2.MjpegVideoUrl;
			}

			return Requires.Throw<string>("Unsupported video capability: " + videoCapabilities);
		}


		/// <summary>
		/// Determines whether specified video capability is among the supported video capabilities.
		/// </summary>
		/// <param name="videoCapability">The video capability to investigate support for.</param>
		/// <param name="supportedCapabilities">The supported video capabilities.</param>
		/// <returns>true if video capability is among the supported video capabilities.</returns>
		private static bool HasFlag(
			VideoCapabilities videoCapability,
			VideoCapabilities supportedCapabilities)
		{
			return (videoCapability & supportedCapabilities) == videoCapability;
		}
	}
}
