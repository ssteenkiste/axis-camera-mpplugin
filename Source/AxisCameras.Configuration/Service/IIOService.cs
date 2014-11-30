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

using System;

namespace AxisCameras.Configuration.Service
{
    /// <summary>
    /// Interface responsible for I/O related operations.
    /// </summary>
    public interface IIOService
    {
        /// <summary>
        /// Gets the file name of the camera icon.
        /// </summary>
        /// <returns>The file name of the camera icon.</returns>
        string CameraIconFileName { get; }

        /// <summary>
        /// Gets the file name of the default thumb.
        /// </summary>
        /// <returns>The file name of the default thumb.</returns>
        string DefaultThumbFileName { get; }

        /// <summary>
        /// Gets the file name of the thumb for specified camera.
        /// </summary>
        /// <param name="cameraId">The camera id.</param>
        /// <returns>
        /// The file name of the thumb if existing on disk, otherwise the generic thumb.
        /// </returns>
        string GetThumbFileName(Guid cameraId);

        /// <summary>
        /// Gets the default thumb.
        /// </summary>
        /// <returns>The default thumb.</returns>
        byte[] GetDefaultThumb();

        /// <summary>
        /// Gets the thumb for specified camera.
        /// </summary>
        /// <param name="cameraId">The camera id.</param>
        /// <returns>The thumb if existing; otherwise the default thumb.</returns>
        byte[] GetThumb(Guid cameraId);

        /// <summary>
        /// Saves a thumb for specified camera.
        /// </summary>
        /// <param name="cameraId">The camera id.</param>
        /// <param name="image">The image thumb.</param>
        /// <returns>The file name where the thumb is saved.</returns>
        string SaveThumb(Guid cameraId, byte[] image);

        /// <summary>
        /// Deletes the thumb belonging to specified camera.
        /// </summary>
        /// <param name="cameraId">The camera id.</param>
        void DeleteThumb(Guid cameraId);
    }
}