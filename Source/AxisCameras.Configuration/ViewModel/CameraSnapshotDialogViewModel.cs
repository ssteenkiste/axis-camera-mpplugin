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
using AxisCameras.Mvvm.Behaviors;

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
            get { return GetValue<byte[]>(); }
            private set { SetValue(value); }
        }

        /// <summary>
        /// Called when view is loaded.
        /// </summary>
        /// <remarks>
        /// Make sure view binds <see cref="WindowLifetimeBehaviors.LoadedProperty" /> or
        /// <see cref="LifetimeBehaviors.LoadedProperty" /> to <see cref="LoadedCommand" />
        /// in order for this method to be called.
        /// </remarks>
       protected override void OnLoaded()
        {
            base.OnLoaded();

            Task<byte[]> task = cameraCommunicationService.GetSnapshotAsync(
                cameraEndpoint,
                videoSource,
                CancellationToken);

            task.ContinueWith(
                t =>
                {
                    if (t.IsFaulted)
                    {
// ReSharper disable once PossibleNullReferenceException
                        Log.Error("Error when getting snapshot from camera.", t.Exception.InnerException);

                        // Close dialog unsuccessfully
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