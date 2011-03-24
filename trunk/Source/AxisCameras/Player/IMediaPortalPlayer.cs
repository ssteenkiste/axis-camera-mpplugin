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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MediaPortal.Playlists;

namespace AxisCameras.Player
{
	/// <summary>
	/// Interface describing the player used in MediaPortal.
	/// </summary>
	public interface IMediaPortalPlayer
	{
		/// <summary>
		/// Starts playing a video from specified URL in full screen.
		/// </summary>
		/// <param name="url">The URL of the video.</param>
		/// <param name="name">The name of the video, will be displayed inside MediaPortal.</param>
		/// <returns>true if playback started successfully; otherwise false.</returns>
		[SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#",
			Justification = "I am building a URL, but since g_Player doesn't accept URLs but instead " +
			"wants a string, I don't see the point in temporary converting it to a URL.")]
		bool PlayVideoStreamInFullScreen(string url, string name);


		/// <summary>
		/// Starts playing the first video stream in the specified sequence of playlist items.
		/// </summary>
		/// <param name="playlist">The sequence of playlist items to play.</param>
		/// <param name="playlistName">The name of the playlist.</param>
		/// <returns>true if playback started successfully; otherwise false.</returns>
		bool PlayVideoStreamsInFullScreen(
			IEnumerable<PlayListItem> playlistItems,
			string playlistName);
	}
}
