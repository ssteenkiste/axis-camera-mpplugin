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

using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using AxisCameras.Configuration.Properties;
using AxisCameras.Configuration.Provider;
using AxisCameras.Configuration.Service;
using AxisCameras.Configuration.View;
using AxisCameras.Configuration.ViewModel.Data;
using AxisCameras.Configuration.ViewModel.ValidationRule;
using AxisCameras.Core.Contracts;
using AxisCameras.Data;
using AxisCameras.Mvvm.Services;

namespace AxisCameras.Configuration.ViewModel
{
    /// <summary>
    /// View model where camera network endpoint is entered.
    /// </summary>
    internal class WizardPageOneViewModel : WizardPageViewModel, IWizardPageOneViewModel
    {
        private readonly IWindowService windowService;
        private readonly ICameraParametersDialogViewModelProvider cameraParametersProvider;

        private DirtyState dirtyState;
        private string friendlyName;
        private string firmwareVersion;
        private VideoCapabilities videoCapabilities;
        private IEnumerable<byte> snapshot;
        private int videoSource;
        private int videoSourceCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardPageOneViewModel"/> class.
        /// </summary>
        /// <param name="windowService">The window service.</param>
        /// <param name="cameraParametersProvider">
        /// The camera parameters dialog view model provider.
        /// </param>
        public WizardPageOneViewModel(
            IWindowService windowService,
            ICameraParametersDialogViewModelProvider cameraParametersProvider)
        {
            Requires.NotNull(windowService);
            Requires.NotNull(cameraParametersProvider);

            this.windowService = windowService;
            this.cameraParametersProvider = cameraParametersProvider;

            AddValidators();
        }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string Address
        {
            get { return Property(() => Address); }
            set { Property(() => Address, value); }
        }

        /// <summary>
        /// Gets or sets the HTTP port.
        /// </summary>
        public string Port
        {
            get { return Property(() => Port); }
            set { Property(() => Port, value); }
        }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName
        {
            get { return Property(() => UserName); }
            set { Property(() => UserName, value); }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password
        {
            get { return Property(() => Password); }
            set { Property(() => Password, value); }
        }

        /// <summary>
        /// Gets the header of the wizard page.
        /// </summary>
        public override string Header
        {
            get { return Resources.WizardPageOne_Header; }
        }

        /// <summary>
        /// Gets the description of the wizard page.
        /// </summary>
        public override string Description
        {
            get { return Resources.WizardPageOne_Description; }
        }

        /// <summary>
        /// Loads page properties from specified camera.
        /// </summary>
        /// <param name="camera">The camera to load page properties from.</param>
        public override void Load(ConfigurableCamera camera)
        {
            Requires.NotNull(camera);

            friendlyName = camera.Name;
            firmwareVersion = camera.FirmwareVersion;
            snapshot = camera.Snapshot;
            videoSource = camera.VideoSource;
            videoSourceCount = camera.VideoSourceCount;
            videoCapabilities = camera.VideoCapabilities;

            Address = camera.Address;
            Port = camera.Port.ToString(CultureInfo.CurrentCulture);
            UserName = camera.UserName;
            Password = camera.Password;

            dirtyState = new DirtyState(
                Address,
                Port,
                UserName,
                Password);
        }

        /// <summary>
        /// Saves page properties to specified camera.
        /// </summary>
        /// <param name="camera">The camera to save page properties to.</param>
        public override void Save(ConfigurableCamera camera)
        {
            Requires.NotNull(camera);

            camera.Name = friendlyName;
            camera.FirmwareVersion = firmwareVersion;
            camera.Snapshot = snapshot;
            camera.VideoSource = videoSource;
            camera.VideoSourceCount = videoSourceCount;
            camera.VideoCapabilities = videoCapabilities;

            camera.Address = Address;
            camera.Port = int.Parse(Port, CultureInfo.CurrentCulture);
            camera.UserName = UserName;
            camera.Password = Password;
        }

        /// <summary>
        /// Validates all added validation rules.
        /// </summary>
        /// <returns>true if validation succeeds; otherwise false.</returns>
        public override bool Validate()
        {
            // Determine if view model is valid
            bool isValid = base.Validate();

            // If view model is valid and camera properties are dirty, communicate with camera
            if (isValid && dirtyState.IsDirty(Address, Port, UserName, Password))
            {
                NetworkEndpoint cameraEndpoint = new NetworkEndpoint(
                    Address,
                    int.Parse(Port, CultureInfo.CurrentCulture),
                    UserName,
                    Password);

                using (ICameraParametersDialogViewModel cameraParametersDialogViewModel =
                    cameraParametersProvider.Provide(cameraEndpoint))
                {
                    // Communicate with camera
                    bool? success = windowService.ShowDialog<ProgressDialog>(
                        cameraParametersDialogViewModel,
                        this);

                    // Was communication with camera successful?
                    if (success == true)
                    {
                        // Only update the following values if user redirected camera to another address or
                        // port, i.e. don't update if only new credentials were entered
                        if (dirtyState.IsNetworkSettingsDirty(Address, Port))
                        {
                            friendlyName = cameraParametersDialogViewModel.FriendlyName;
                            videoSource = 1;
                            snapshot = null;
                        }

                        // Update the following parameters regardless, it won't hurt
                        firmwareVersion = cameraParametersDialogViewModel.FirmwareVersion;
                        videoSourceCount = cameraParametersDialogViewModel.VideoSourceCount;
                        videoCapabilities = cameraParametersDialogViewModel.VideoCapabilities;
                    }
                    else
                    {
                        isValid = false;

                        windowService.ShowMessageBox(
                            this,
                            Resources.CameraCommunicationError,
                            Resources.CameraCommunicationError_Title,
                            icon: MessageBoxImage.Error);
                    }
                }
            }

            return isValid;
        }

        /// <summary>
        /// Adds the validators.
        /// </summary>
        private void AddValidators()
        {
            AddValidator(
                () => Address,
                new NotEmptyStringValidationRule
                {
                    ErrorMessage = Resources.Validation_Failed_Address
                });
            AddValidator(
                () => Port,
                new PortValidationRule());
            AddValidator(
                () => UserName,
                new UserNameValidationRule());
            AddValidator(
                () => Password,
                new NotEmptyStringValidationRule
                {
                    ErrorMessage = Resources.Validation_Failed_Password
                });
        }

        /// <summary>
        /// Class determining whether properties are dirty, i.e. a user has changed them.
        /// </summary>
        private class DirtyState
        {
            private readonly string oldAddress;
            private readonly string oldPort;
            private readonly string oldUserName;
            private readonly string oldPassword;

            /// <summary>
            /// Initializes a new instance of the <see cref="DirtyState"/> class.
            /// </summary>
            /// <param name="oldAddress">The old address.</param>
            /// <param name="oldPort">The old port.</param>
            /// <param name="oldUserName">The old user name.</param>
            /// <param name="oldPassword">The old password.</param>
            public DirtyState(string oldAddress, string oldPort, string oldUserName, string oldPassword)
            {
                this.oldAddress = oldAddress;
                this.oldPort = oldPort;
                this.oldUserName = oldUserName;
                this.oldPassword = oldPassword;
            }

            /// <summary>
            /// Determines whether any of the properties are dirty.
            /// </summary>
            /// <param name="newAddress">The new address.</param>
            /// <param name="newPort">The new port.</param>
            /// <param name="newUserName">The new user name.</param>
            /// <param name="newPassword">The new password.</param>
            /// <returns>true if any of the properties are dirty; otherwise false.</returns>
            public bool IsDirty(string newAddress, string newPort, string newUserName, string newPassword)
            {
                return
                    oldAddress != newAddress ||
                    oldPort != newPort ||
                    oldUserName != newUserName ||
                    oldPassword != newPassword;
            }

            /// <summary>
            /// Determines whether any of the network properties, i.e. address or port, are dirty.
            /// </summary>
            /// <param name="newAddress">The new address.</param>
            /// <param name="newPort">The new port.</param>
            /// <returns>true if any of the network properties are dirty; otherwise false.</returns>
            public bool IsNetworkSettingsDirty(string newAddress, string newPort)
            {
                return
                    oldAddress != newAddress ||
                    oldPort != newPort;
            }
        }
    }
}