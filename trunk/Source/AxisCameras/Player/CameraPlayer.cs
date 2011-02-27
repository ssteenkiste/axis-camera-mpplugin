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
using System.Diagnostics.CodeAnalysis;
using System.Web;
using AxisCameras.Core;
using AxisCameras.Data;

namespace AxisCameras.Player
{
	/// <summary>
	/// Class responsible for playing video.
	/// </summary>
	public class CameraPlayer : ICameraPlayer
	{
		private readonly IMediaPortalPlayer mediaPortalPlayer;


		/// <summary>
		/// Initializes a new instance of the <see cref="CameraPlayer"/> class.
		/// </summary>
		/// <param name="mediaPortalPlayer">The played used in MediaPortal.</param>
		public CameraPlayer(IMediaPortalPlayer mediaPortalPlayer)
		{
			if (mediaPortalPlayer == null) throw new ArgumentNullException("mediaPortalPlayer");

			this.mediaPortalPlayer = mediaPortalPlayer;
		}


		/// <summary>
		/// Plays live video from specified camera.
		/// </summary>
		/// <param name="camera">The camera.</param>
		public void PlayLiveVideo(Camera camera)
		{
			if (camera == null) throw new ArgumentNullException("camera");

			Log.Info("Play live view from {0}", camera.Name);

			string url = GetLiveVideoUrl(camera);

			// Play live view in full screen
			mediaPortalPlayer.PlayVideoStreamInFullScreen(url, camera.Name);
		}


		/// <summary>
		/// Gets the live video video URL based on specified camera.
		/// </summary>
		/// <param name="camera">The camera.</param>
		/// <returns>The live video video URL based on specified camera.</returns>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Easier for me to catch all exceptions, instead of specifying all possible exception types.")]
		private static string GetLiveVideoUrl(Camera camera)
		{
			// Try to parse firmware version
			Version firmwareVersion;
			try
			{
				firmwareVersion = new Version(camera.FirmwareVersion);
			}
			catch (Exception e)
			{
				Log.Error("Player - Unable to parse firmware version {0}, defaulting to 5.0. {1}",
					camera.FirmwareVersion,
					e.ToString());

				// If firmware version cannot be parsed, assume it is a beta of LFP, i.e. the VAPIX 3 live
				// video URL should be used
				firmwareVersion = new Version(5, 0);
			}

			// If firmware version is below 5.0 use VAPIX 2; otherwise use VAPIX 3
			string urlFormat;
			if (firmwareVersion < new Version(5, 0))
			{
				Log.Debug("Camera firmware version is {0}, use VAPIX 2 CGI.", firmwareVersion);
				urlFormat = Vapix.Version2.LiveVideoUrl;
			}
			else
			{
				Log.Debug("Camera firmware version is {0}, use VAPIX 3 CGI.", firmwareVersion);
				urlFormat = Vapix.Version3.LiveVideoUrl;
			}

			return urlFormat.InvariantFormat(
				HttpUtility.UrlEncode(camera.UserName),
				HttpUtility.UrlEncode(camera.Password),
				camera.Address,
				camera.Port);
		}
	}
}
