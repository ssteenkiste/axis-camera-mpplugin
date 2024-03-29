﻿#region Copyright (C) 2005-2015 Team MediaPortal

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

namespace AxisCameras.Data
{
    /// <summary>
    /// Enum describing the video capability of a <see cref="Camera"/>.
    /// </summary>
    [Flags]
    public enum VideoCapabilities
    {
        /// <summary>
        /// Camera has no video capabilities.
        /// </summary>
        None = 0,

        /// <summary>
        /// Camera has support for M-JPEG.
        /// </summary>
        Mjpeg = 1,

        /// <summary>
        /// Camera has support for MPEG-4.
        /// </summary>
        Mpeg4 = 2,

        /// <summary>
        /// Camera has support for h.264.
        /// </summary>
        H264 = 4
    }
}