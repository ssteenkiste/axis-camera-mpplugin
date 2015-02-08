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

namespace AxisCameras.Player
{
    /// <summary>
    /// Class describing the Axis VAPIX:
    /// http://www.axis.com/techsup/cam_servers/dev/cam_http_api_index.php
    /// </summary>
    internal static class Vapix
    {
        /// <summary>
        /// Class describing the second version o VAPIX. It is applicable to cameras with firmware
        /// version 5 and newer.
        /// </summary>
        internal static class Version2
        {
            /// <summary>
            /// The M-JPEG live view URL.
            /// </summary>
            internal const string MjpegVideoUrl = "axmphttp://{0}:{1}@{2}:{3}/axis-cgi/mjpg/video.cgi?camera={4}";

            /// <summary>
            /// The MPEG-4 live video URL.
            /// </summary>
            internal const string Mpeg4VideoUrl = "axrtsphttp://{0}:{1}@{2}:{3}/mpeg4/{4}/media.amp";
        }

        /// <summary>
        /// Class describing the third version o VAPIX. It is applicable to cameras with firmware
        /// older than version 5.
        /// </summary>
        internal static class Version3
        {
            /// <summary>
            /// The H.264 live video URL.
            /// </summary>
            internal const string H264VideoUrl = "axrtsphttp://{0}:{1}@{2}:{3}/axis-media/media.amp?videocodec=h264&camera={4}";
        }
    }
}