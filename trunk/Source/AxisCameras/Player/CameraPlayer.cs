#region Copyright (C) 2005-2014 Team MediaPortal

// Copyright (C) 2005-2014 Team MediaPortal
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
using System.Linq;
using AxisCameras.Core;
using AxisCameras.Core.Contracts;
using AxisCameras.Data;
using AxisCameras.Properties;
using MediaPortal.Playlists;

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
            Requires.NotNull(mediaPortalPlayer);
            Requires.NotNull(videoUrlBuilder);

            this.mediaPortalPlayer = mediaPortalPlayer;
            this.videoUrlBuilder = videoUrlBuilder;
        }

        /// <summary>
        /// Plays live video from specified camera.
        /// </summary>
        /// <param name="camera">The camera.</param>
        public void PlayLiveVideo(Camera camera)
        {
            Requires.NotNull(camera);

            Log.Info("Play live video from {0}", camera.Name);

            string url = BuildLiveVideoUrl(camera);

            // Play live view in full screen
            mediaPortalPlayer.PlayVideoStreamInFullScreen(url, camera.Name);
        }

        /// <summary>
        /// Plays live video from specified cameras.
        /// </summary>
        /// <param name="cameras">The cameras.</param>
        /// <remarks>
        /// The cameras are placed in a playlist which means that it is possible to navigate forward
        /// and backward, i.e. previous camera and next camera, during playback.
        /// </remarks>
        public void PlayLiveVideo(IEnumerable<Camera> cameras)
        {
            Requires.NotNull(cameras);
            Requires.True(cameras.Any());

            if (cameras.Count() == 1)
            {
                PlayLiveVideo(cameras.First());
            }
            else
            {
                Log.Debug("Play live video from {0} cameras", cameras.Count());

                IEnumerable<PlayListItem> playlistItems =
                    from camera in cameras
                    select new PlayListItem(camera.Name, BuildLiveVideoUrl(camera))
                    {
                        Description = camera.Name,
                        Type = PlayListItem.PlayListItemType.VideoStream
                    };

                mediaPortalPlayer.PlayVideoStreamsInFullScreen(
                    playlistItems.ToList(),
                    Resources.Playlist_DefaultName);
            }
        }

        /// <summary>
        /// Builds a live video URL from specified camera.
        /// </summary>
        /// <param name="camera">The camera.</param>
        /// <returns>A live video URL from specified camera.</returns>
        private string BuildLiveVideoUrl(Camera camera)
        {
            return videoUrlBuilder.BuildLiveVideoUrl(
                camera.Address,
                camera.Port,
                camera.UserName,
                camera.Password,
                camera.VideoCapabilities,
                camera.VideoSource);
        }
    }
}