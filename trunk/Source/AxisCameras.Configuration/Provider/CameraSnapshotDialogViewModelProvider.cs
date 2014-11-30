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
using AxisCameras.Configuration.Service;
using AxisCameras.Configuration.ViewModel;
using AxisCameras.Core;
using AxisCameras.Core.Contracts;

namespace AxisCameras.Configuration.Provider
{
    /// <summary>
    /// Class describing a provider that provides a ICameraSnapshotDialogViewModel.
    /// </summary>
    internal class CameraSnapshotDialogViewModelProvider : ICameraSnapshotDialogViewModelProvider
    {
        private readonly Func<ICameraCommunicationService> cameraCommunicationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraSnapshotDialogViewModelProvider"/> class. 
        /// </summary>
        /// <param name="cameraCommunicationService">
        /// The camera communication service.
        /// </param>
        public CameraSnapshotDialogViewModelProvider(
            Func<ICameraCommunicationService> cameraCommunicationService)
        {
            Requires.NotNull(cameraCommunicationService);

            this.cameraCommunicationService = cameraCommunicationService;
        }

        /// <summary>
        /// Returns a ICameraSnapshotDialogViewModel from specified network endpoint.
        /// </summary>
        /// <param name="cameraEndpoint">The camera network endpoint.</param>
        /// <param name="videoSource">The video source to get snapshot from.</param>
        public ICameraSnapshotDialogViewModel Provide(NetworkEndpoint cameraEndpoint, int videoSource)
        {
            Requires.NotNull(cameraEndpoint);
            Requires.True(videoSource >= 1, "Video source must be 1 or greater.");

            Log.Debug("Provide a ICameraSnapshotDialogViewModel");

            return new CameraSnapshotDialogViewModel(
                cameraCommunicationService(),
                cameraEndpoint,
                videoSource);
        }
    }
}