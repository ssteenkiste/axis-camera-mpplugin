#region Copyright (C) 2005-2011 Team MediaPortal

// Copyright (C) 2005-2011 Team MediaPortal
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
using AxisCameras.Data;

namespace AxisCameras.Player
{
    /// <summary>
    /// Interface responsible for playing video.
    /// </summary>
    public interface ICameraPlayer
    {
        /// <summary>
        /// Plays live video from specified camera.
        /// </summary>
        /// <param name="camera">The camera.</param>
        void PlayLiveVideo(Camera camera);

        /// <summary>
        /// Plays live video from specified cameras.
        /// </summary>
        /// <param name="cameras">The cameras.</param>
        /// <remarks>
        /// The cameras are placed in a playlist which means that it is possible to navigate forward
        /// and backward, i.e. previous camera and next camera, during playback.
        /// </remarks>
        void PlayLiveVideo(IEnumerable<Camera> cameras);
    }
}