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
using AxisCameraMPPlugin.Core;
using AxisCameraMPPlugin.Data;
using MediaPortal.Player;

namespace AxisCameraMPPlugin
{
	/// <summary>
	/// Class responsible for playing video.
	/// </summary>
	public class Player : IPlayer
	{
		/// <summary>
		/// VAPIX 2 version of live video URL. The video format received is MPEG4.
		/// </summary>
		private const string Vapix2LiveVideoUrl = "axrtsphttp://{0}:{1}@{2}:{3}/mpeg4/media.amp";


		/// <summary>
		/// VAPIX 3 version of live video URL. The video format received is H.264.
		/// </summary>
		private const string Vapix3LiveVideoUrl = "axrtsphttp://{0}:{1}@{2}:{3}/axis-media/media.amp?videocodec=h264";


		/// <summary>
		/// Plays live video from specified camera.
		/// </summary>
		/// <param name="camera">The camera.</param>
		public void PlayLiveVideo(Camera camera)
		{
			if (camera == null) throw new ArgumentNullException("camera");

			Log.Info("Play live view from {0}", camera.Name);

			string url = GetLiveVideoUrl(camera);

			// Stop player if already playing
			if (g_Player.Playing)
			{
				g_Player.Stop();
			}

			// Play live view in full screen
			g_Player.PlayVideoStream(url, camera.Name);
			g_Player.ShowFullScreenWindow();
		}


		/// <summary>
		/// Gets the live video video URL based on specified camera.
		/// </summary>
		/// <param name="camera">The camera.</param>
		/// <returns>The live video video URL based on specified camera.</returns>
		private static string GetLiveVideoUrl(Camera camera)
		{
			// Try to parse firmware version
			Version firmwareVersion;
			try
			{
				firmwareVersion = new Version(camera.FirmwareVersion);
			}
			catch
			{
				// If firmware version cannot be parsed, assume it is a beta of LFP, i.e. the VAPIX 3 live
				// video URL should be used
				firmwareVersion = new Version(5, 0);
			}

			// If firmware version is below 5.0 use VAPIX 2; otherwise use VAPIX 3
			string urlFormat = firmwareVersion < new Version(5, 0) ?
				Vapix2LiveVideoUrl : Vapix3LiveVideoUrl;

			return urlFormat.InvariantFormat(
				HttpUtility.UrlEncode(camera.UserName),
				HttpUtility.UrlEncode(camera.Password),
				camera.Address,
				camera.Port);
		}
	}
}
