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

using System.Threading.Tasks;
using AxisCameras.Configuration.Service;
using AxisCameras.Core;
using AxisCameras.Core.Contracts;

namespace AxisCameras.Configuration.ViewModel
{
    /// <summary>
    /// Class responsible for reading parameters from a camera.
    /// </summary>
    internal class CameraSnapshotDialogViewModel : ProgressDialogViewModel, ICameraSnapshotDialogViewModel
    {
        private readonly ICameraCommunicationService cameraCommunicationService;
        private readonly NetworkEndpoint cameraEndpoint;
        private readonly int videoSource;
        private readonly TaskScheduler uiScheduler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraSnapshotDialogViewModel"/> class.
        /// </summary>
        /// <param name="cameraCommunicationService">The camera communication service.</param>
        /// <param name="cameraEndpoint">The camera endpoint.</param>
        /// <param name="videoSource">The video source to get snapshot from.</param>
        public CameraSnapshotDialogViewModel(
            ICameraCommunicationService cameraCommunicationService,
            NetworkEndpoint cameraEndpoint,
            int videoSource)
        {
            Requires.NotNull(cameraCommunicationService);
            Requires.NotNull(cameraEndpoint);
            Requires.True(videoSource >= 1, "Video source must be 1 or greater.");

            this.cameraCommunicationService = cameraCommunicationService;
            this.cameraEndpoint = cameraEndpoint;
            this.videoSource = videoSource;
            uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        /// <summary>
        /// Gets the camera snapshot.
        /// </summary>
        public byte[] Snapshot
        {
            get { return Property(() => Snapshot); }
            private set { Property(() => Snapshot, value); }
        }

        /// <summary>
        /// Executes the Loaded command.
        /// </summary>
        protected override void Loaded(object parameter)
        {
            Task<byte[]> task = cameraCommunicationService.GetSnapshotAsync(
                cameraEndpoint,
                videoSource,
                CancellationToken);

            task.ContinueWith(
                t =>
                {
                    if (t.IsFaulted)
                    {
                        Log.Error("Error when getting snapshot from camera.", t.Exception.InnerException);

                        /// Close dialog unsuccessfully
                        DialogResultCommand.Execute(false);
                    }
                    else
                    {
                        Snapshot = t.Result;

                        // Close dialog successfully
                        DialogResultCommand.Execute(true);
                    }
                },
                uiScheduler);
        }
    }
}