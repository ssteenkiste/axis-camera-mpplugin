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
using System.Globalization;
using System.Web;
using AxisCameraMPPlugin.Data;
using MediaPortal.Player;

namespace AxisCameraMPPlugin
{
	/// <summary>
	/// Class responsible for playing video.
	/// </summary>
	class Player : IPlayer
	{
		private const string LiveVideoUrl = "axrtpu://{0}:{1}@{2}:{3}/mpeg4/media.amp";


		/// <summary>
		/// Plays live video from specified camera.
		/// </summary>
		/// <param name="camera">The camera.</param>
		public void PlayLiveVideo(Camera camera)
		{
			if (camera == null) throw new ArgumentNullException("camera");

			string url = LiveVideoUrl.InvariantFormat(
				HttpUtility.UrlEncode(camera.UserName),
				HttpUtility.UrlEncode(camera.Password),
				camera.Address,
				camera.Port);

			// Stop player if already playing
			if (g_Player.Playing)
			{
				g_Player.Stop();
			}

			// Play live view in full screen
			g_Player.PlayVideoStream(url, camera.Name);
			g_Player.ShowFullScreenWindow();
		}
	}
}
