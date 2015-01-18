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

using System.Threading.Tasks;
using AxisCameras.Configuration.Service;
using AxisCameras.Core;
using AxisCameras.Core.Contracts;
using AxisCameras.Data;
using AxisCameras.Mvvm.Behaviors;

namespace AxisCameras.Configuration.ViewModel
{
    /// <summary>
    /// Class responsible for reading parameters from a camera.
    /// </summary>
    internal class CameraParametersDialogViewModel : ProgressDialogViewModel, ICameraParametersDialogViewModel
    {
        private readonly ICameraCommunicationService cameraCommunicationService;
        private readonly NetworkEndpoint cameraEndpoint;
        private readonly TaskScheduler uiScheduler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraParametersDialogViewModel"/> class.
        /// </summary>
        /// <param name="cameraCommunicationService">The camera communication service.</param>
        /// <param name="cameraEndpoint">The camera endpoint.</param>
        public CameraParametersDialogViewModel(
            ICameraCommunicationService cameraCommunicationService,
            NetworkEndpoint cameraEndpoint)
        {
            Requires.NotNull(cameraCommunicationService);
            Requires.NotNull(cameraEndpoint);

            this.cameraCommunicationService = cameraCommunicationService;
            this.cameraEndpoint = cameraEndpoint;
            uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        /// <summary>
        /// Gets the friendly name.
        /// </summary>
        public string FriendlyName
        {
            get { return GetValue<string>(); }
            private set { SetValue(value); }
        }

        /// <summary>
        /// Gets the firmware version.
        /// </summary>
        public string FirmwareVersion
        {
            get { return GetValue<string>(); }
            private set { SetValue(value); }
        }

        /// <summary>
        /// Gets the video source count, i.e. the number of video source this camera (device actually)
        /// has.
        /// </summary>
        public int VideoSourceCount
        {
            get { return GetValue<int>(); }
            private set { SetValue(value); }
        }

        /// <summary>
        /// Gets the video capabilities.
        /// </summary>
        public VideoCapabilities VideoCapabilities
        {
            get { return GetValue<VideoCapabilities>(); }
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

            Task<CameraParameters> task = cameraCommunicationService.GetCameraParametersAsync(
                cameraEndpoint,
                CancellationToken);

            task.ContinueWith(
                t =>
                {
                    if (t.IsFaulted)
                    {
// ReSharper disable once PossibleNullReferenceException
                        Log.Error("Error when getting parameters from camera.", t.Exception.InnerException);

                        // Close dialog unsuccessfully
                        DialogResultCommand.Execute(false);
                    }
                    else
                    {
                        FriendlyName = t.Result.FriendlyName;
                        FirmwareVersion = t.Result.FirmwareVersion;
                        VideoSourceCount = t.Result.VideoSourceCount;
                        VideoCapabilities = t.Result.VideoCapabilities;

                        // Close dialog successfully
                        DialogResultCommand.Execute(true);
                    }
                },
                uiScheduler);
        }
    }
}