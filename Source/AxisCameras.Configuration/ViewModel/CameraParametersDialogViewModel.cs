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
using AxisCameras.Data;

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
            get { return Property(() => FriendlyName); }
            private set { Property(() => FriendlyName, value); }
        }

        /// <summary>
        /// Gets the firmware version.
        /// </summary>
        public string FirmwareVersion
        {
            get { return Property(() => FirmwareVersion); }
            private set { Property(() => FirmwareVersion, value); }
        }

        /// <summary>
        /// Gets the video source count, i.e. the number of video source this camera (device actually)
        /// has.
        /// </summary>
        public int VideoSourceCount
        {
            get { return Property(() => VideoSourceCount); }
            private set { Property(() => VideoSourceCount, value); }
        }

        /// <summary>
        /// Gets the video capabilities.
        /// </summary>
        public VideoCapabilities VideoCapabilities
        {
            get { return Property(() => VideoCapabilities); }
            private set { Property(() => VideoCapabilities, value); }
        }

        /// <summary>
        /// Executes the Loaded command.
        /// </summary>
        protected override void Loaded(object parameter)
        {
            Task<CameraParameters> task = cameraCommunicationService.GetCameraParametersAsync(
                cameraEndpoint,
                CancellationToken);

            task.ContinueWith(
                t =>
                {
                    if (t.IsFaulted)
                    {
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