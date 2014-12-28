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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using AxisCameras.Configuration.Properties;
using AxisCameras.Configuration.Provider;
using AxisCameras.Configuration.Service;
using AxisCameras.Configuration.View;
using AxisCameras.Configuration.ViewModel.Data;
using AxisCameras.Configuration.ViewModel.ValidationRule;
using AxisCameras.Core;
using AxisCameras.Core.Contracts;
using AxisCameras.Mvvm;
using AxisCameras.Mvvm.Behaviors;
using AxisCameras.Mvvm.Services;

namespace AxisCameras.Configuration.ViewModel
{
    /// <summary>
    /// View model where camera name is entered and snapshot displayed.
    /// </summary>
    internal class WizardPageThreeViewModel : WizardPageViewModel, IWizardPageThreeViewModel
    {
        private readonly IWindowService windowService;
        private readonly IDispatcherService dispatcherService;
        private readonly ICameraSnapshotDialogViewModelProvider cameraSnapshotProvider;
        private readonly IIOService ioService;
        private readonly ICommand refreshCommand;

        private NetworkEndpoint cameraEndpoint;
        private int videoSource;
        private byte[] defaultSnapshot;

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardPageThreeViewModel"/> class.
        /// </summary>
        /// <param name="windowService">The window service.</param>
        /// <param name="dispatcherService">The dispatcher service.</param>
        /// <param name="cameraSnapshotProvider">
        /// The camera snapshot dialog view model provider.
        /// </param>
        /// <param name="ioService">The I/O service.</param>
        public WizardPageThreeViewModel(
            IWindowService windowService,
            IDispatcherService dispatcherService,
            ICameraSnapshotDialogViewModelProvider cameraSnapshotProvider,
            IIOService ioService)
        {
            Requires.NotNull(windowService);
            Requires.NotNull(dispatcherService);
            Requires.NotNull(cameraSnapshotProvider);
            Requires.NotNull(ioService);

            this.windowService = windowService;
            this.dispatcherService = dispatcherService;
            this.cameraSnapshotProvider = cameraSnapshotProvider;
            this.ioService = ioService;

            refreshCommand = new RelayCommand(Refresh);

            AddValidators();
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets the snapshot.
        /// </summary>
        public IEnumerable<byte> Snapshot
        {
            get { return GetValue<IEnumerable<byte>>(); }
            private set { SetValue(value); }
        }

        /// <summary>
        /// Gets the command refreshing the snapshot.
        /// </summary>
        public ICommand RefreshCommand
        {
            get { return refreshCommand; }
        }

        /// <summary>
        /// Gets the header of the wizard page.
        /// </summary>
        public override string Header
        {
            get { return Resources.WizardPageThree_Header; }
        }

        /// <summary>
        /// Gets the description of the wizard page.
        /// </summary>
        public override string Description
        {
            get { return Resources.WizardPageThree_Description; }
        }

        /// <summary>
        /// Loads page properties from specified camera.
        /// </summary>
        /// <param name="camera">The camera to load page properties from.</param>
        public override void Load(ConfigurableCamera camera)
        {
            Requires.NotNull(camera);

            cameraEndpoint = new NetworkEndpoint(
                camera.Address,
                camera.Port,
                camera.UserName,
                camera.Password);
            videoSource = camera.VideoSource;

            Name = camera.Name;
            Snapshot = camera.Snapshot;
        }

        /// <summary>
        /// Saves page properties to specified camera.
        /// </summary>
        /// <param name="camera">The camera to save page properties to.</param>
        public override void Save(ConfigurableCamera camera)
        {
            Requires.NotNull(camera);

            camera.Name = Name;
            camera.Snapshot = Snapshot;
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

            // We got a little bit of synchronization problem here. This code cannot run before the view
            // has been registered, because then the progress dialog wouldn't be centered on the owner.
            // The problem we are facing is that both this method and the registering process in the
            // window service is depending on the Loaded event, and unfortunately this class registers
            // for the event before the window service does.
            // We can 'solve' this by letting the registering code run first by putting this code in the
            // event queue, making the dispatcher run it after the registering code.
            dispatcherService.BeginInvoke(
                new Action(
                    () =>
                    {
                        if (Snapshot == null)
                        {
                            // Set default snapshot while getting snapshot from camera
                            Snapshot = DefaultSnapshot;

                            Log.Debug("Start getting snapshot from camera.");
                            RefreshCommand.Execute(null);
                        }
                    }));
        }

        /// <summary>
        /// Refreshes the snapshot.
        /// </summary>
        private void Refresh(object parameter)
        {
            Log.Debug("Refreshing snapshot");

            using (ICameraSnapshotDialogViewModel cameraSnapshotDialogViewModel =
                cameraSnapshotProvider.Provide(cameraEndpoint, videoSource))
            {
                // Communicate with camera
                bool? success = windowService.ShowDialog<ProgressDialog>(
                    cameraSnapshotDialogViewModel,
                    this);

                // Was communication with camera successful?
                if (success == true)
                {
                    Snapshot = cameraSnapshotDialogViewModel.Snapshot;
                }
                else
                {
                    Snapshot = DefaultSnapshot;

                    windowService.ShowMessageBox(
                        this,
                        Resources.CameraCommunicationError,
                        Resources.CameraCommunicationError_Title,
                        icon: MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Gets the default snapshot.
        /// </summary>
        private IEnumerable<byte> DefaultSnapshot
        {
            get
            {
                if (defaultSnapshot == null)
                {
                    defaultSnapshot = ioService.GetDefaultThumb();
                }

                return defaultSnapshot;
            }
        }

        /// <summary>
        /// Adds the validators.
        /// </summary>
        private void AddValidators()
        {
            AddValidator(
                () => Name,
                new NotEmptyStringValidationRule
                {
                    ErrorMessage = Resources.Validation_Failed_CameraName
                });
        }
    }
}