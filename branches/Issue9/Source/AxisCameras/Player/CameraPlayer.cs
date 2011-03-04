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
		private readonly IVideoUrlBuilder videoUrlBuilder;


		/// <summary>
		/// Initializes a new instance of the <see cref="CameraPlayer"/> class.
		/// </summary>
		/// <param name="mediaPortalPlayer">The played used in MediaPortal.</param>
		/// <param name="videoUrlBuilder">The video URL builder.</param>
		public CameraPlayer(
			IMediaPortalPlayer mediaPortalPlayer,
			IVideoUrlBuilder videoUrlBuilder)
		{
			if (mediaPortalPlayer == null) throw new ArgumentNullException("mediaPortalPlayer");
			if (videoUrlBuilder == null) throw new ArgumentNullException("videoUrlBuilder");

			this.mediaPortalPlayer = mediaPortalPlayer;
			this.videoUrlBuilder = videoUrlBuilder;
		}


		/// <summary>
		/// Plays live video from specified camera.
		/// </summary>
		/// <param name="camera">The camera.</param>
		public void PlayLiveVideo(Camera camera)
		{
			if (camera == null) throw new ArgumentNullException("camera");

			Log.Info("Play live view from {0}", camera.Name);

			string url = videoUrlBuilder.BuildLiveVideoUrl(
				camera.Address,
				camera.Port,
				camera.UserName,
				camera.Password,
				camera.FirmwareVersion);

			// Play live view in full screen
			mediaPortalPlayer.PlayVideoStreamInFullScreen(url, camera.Name);
		}
	}
}
