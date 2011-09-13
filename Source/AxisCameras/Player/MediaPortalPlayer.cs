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
using System.Collections.Generic;
using System.Linq;
using AxisCameras.Core.Contracts;
using MediaPortal.Player;
using MediaPortal.Playlists;

namespace AxisCameras.Player
{
	/// <summary>
	/// Class describing the player used in MediaPortal.
	/// </summary>
	public class MediaPortalPlayer : IMediaPortalPlayer
	{
		/// <summary>
		/// Starts playing a video from specified URL in full screen.
		/// </summary>
		/// <param name="url">The URL of the video.</param>
		/// <param name="name">The name of the video, will be displayed inside MediaPortal.</param>
		/// <returns>true if playback started successfully; otherwise false.</returns>
		public bool PlayVideoStreamInFullScreen(string url, string name)
		{
			Requires.NotNullOrEmpty(url);

			// Stop player if already playing
			if (g_Player.Playing)
			{
				g_Player.Stop();
			}

			if (g_Player.PlayVideoStream(url, name))
			{
				g_Player.ShowFullScreenWindow();
				return true;
			}

			return false;
		}


		/// <summary>
		/// Starts playing the first video stream in the specified sequence of playlist items.
		/// </summary>
		/// <param name="playlistItems">The sequence of playlist items to play.</param>
		/// <param name="playlistName">The name of the playlist.</param>
		/// <returns>true if playback started successfully; otherwise false.</returns>
		public bool PlayVideoStreamsInFullScreen(
			IEnumerable<PlayListItem> playlistItems,
			string playlistName)
		{
			Requires.NotNull(playlistItems);
			Requires.True(playlistItems.Any(), "Playlist must contain at least one item.");

			// By using the music video playlist we are forcing MediaPortal to play the playlist as a
			// video stream, which is the preferred way of playing live video from cameras. The code 
			// can be found in PlayListPlayer.Play(int iSong).
			//
			// The obvious choice would of course be to use PLAYLIST_VIDEO type but then I either need to
			// add the amp-file extension (live video stream URL ends with media.amp) during installation
			// or make sure the MediaInfoWrapper is capable of parsing the video stream, which I don't
			// think it is capable of.
			PlayListType playlistType = PlayListType.PLAYLIST_MUSIC_VIDEO;

			PlayListPlayer player = PlayListPlayer.SingletonPlayer;
			PlayList playlist = player.GetPlaylist(playlistType);

			// Setup playlist
			playlist.Clear();
			playlist.Name = playlistName;

			foreach (PlayListItem playlistItem in playlistItems)
			{
				playlist.Add(playlistItem);
			}

			// Setup player
			player.Reset();
			player.RepeatPlaylist = true;
			player.CurrentPlaylistType = playlistType;

			// Play the first item
			if (player.Play(0))
			{
				g_Player.ShowFullScreenWindow();
				return true;
			}

			return false;
		}
	}
}
