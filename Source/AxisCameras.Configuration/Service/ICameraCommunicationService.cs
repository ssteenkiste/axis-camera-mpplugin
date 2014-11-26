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

using System.Threading;
using System.Threading.Tasks;

namespace AxisCameras.Configuration.Service
{
    /// <summary>
    /// Interface responsible for communicating with a camera.
    /// </summary>
    internal interface ICameraCommunicationService
    {
        /// <summary>
        /// Gets camera parameters from specified camera network endpoint asynchronously.
        /// </summary>
        /// <param name="networkEndpoint">The network endpoint.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task getting camera parameters.</returns>
        Task<CameraParameters> GetCameraParametersAsync(
            NetworkEndpoint networkEndpoint,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets camera snapshot from specified camera network endpoint asynchronously.
        /// </summary>
        /// <param name="networkEndpoint">The network endpoint.</param>
        /// <param name="videoSource">The video source to get snapshot from.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task getting a camera snapshot.</returns>
        Task<byte[]> GetSnapshotAsync(
            NetworkEndpoint networkEndpoint,
            int videoSource,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}