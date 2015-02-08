#region Copyright (C) 2005-2015 Team MediaPortal

// Copyright (C) 2005-2015 Team MediaPortal
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

using System.Diagnostics.CodeAnalysis;
using AxisCameras.Data;

namespace AxisCameras.Player
{
    /// <summary>
    /// Interface capable of creating video URLs.
    /// </summary>
    public interface IVideoUrlBuilder
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
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "I am building a URL, but since g_Player doesn't accept URLs but instead " +
                            "wants a string, I don't see the point in temporary converting it to a URL.")]
        string BuildLiveVideoUrl(
            string address,
            int port,
            string userName,
            string password,
            VideoCapabilities videoCapabilities,
            int videoSource);
    }
}