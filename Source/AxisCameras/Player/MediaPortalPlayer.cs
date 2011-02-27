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
using MediaPortal.Player;

namespace AxisCameras.Player
{
	/// <summary>
	/// Class describing the player used in MediaPortal.
	/// </summary>
	class MediaPortalPlayer : IMediaPortalPlayer
	{
		/// <summary>
		/// Starts playing a video from specified URL.
		/// </summary>
		/// <param name="url">The URL of the video.</param>
		/// <param name="name">The name of the video, will be displayed inside MediaPortal.</param>
		/// <returns>
		/// true if playback started successfully; otherwise false.
		/// </returns>
		public bool PlayVideoStream(string url, string name)
		{
			return g_Player.PlayVideoStream(url, name);
		}


		/// <summary>
		/// Stop playing current media.
		/// </summary>
		public void Stop()
		{
			g_Player.Stop();
		}


		/// <summary>
		/// Gets a value indicating whether any media is being played.
		/// </summary>
		public bool Playing
		{
			get { return g_Player.Playing; }
		}


		/// <summary>
		/// Is showing the played media in full screen.
		/// </summary>
		public void ShowFullScreenWindow()
		{
			g_Player.ShowFullScreenWindow();
		}
	}
}
