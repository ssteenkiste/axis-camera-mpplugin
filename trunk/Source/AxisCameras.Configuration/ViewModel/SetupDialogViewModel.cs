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
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AxisCameras.Configuration.Properties;
using AxisCameras.Configuration.Provider;
using AxisCameras.Configuration.Service;
using AxisCameras.Configuration.View;
using AxisCameras.Configuration.ViewModel.Data;
using AxisCameras.Core;
using AxisCameras.Core.Contracts;
using AxisCameras.Mvvm;
using AxisCameras.Mvvm.Services;

namespace AxisCameras.Configuration.ViewModel
{
    /// <summary>
    /// View model capable of adding, editing and removing cameras.
    /// </summary>
    internal class SetupDialogViewModel : DialogViewModelBase, ISetupDialogViewModel
    {
        private readonly IWindowService windowService;
        private readonly IIOService ioService;
        private readonly ICameraViewModelProvider cameraProvider;
        private readonly IWizardDialogViewModelProvider wizardProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupDialogViewModel"/> class.
        /// </summary>
        /// <param name="windowService">The window service.</param>
        /// <param name="ioService">The I/O service.</param>
        /// <param name="productInformation">The product information.</param>
        /// <param name="cameraProvider">The camera name view model provider.</param>
        /// <param name="wizardProvider">The wizard view model provider.</param>
        /// <param name="cameras">The cameras.</param>
        public SetupDialogViewModel(
            IWindowService windowService,
            IIOService ioService,
            IProductInformation productInformation,
            ICameraViewModelProvider cameraProvider,
            IWizardDialogViewModelProvider wizardProvider,
            IEnumerable<ICameraViewModel> cameras)
        {
            Requires.NotNull(windowService);
            Requires.NotNull(ioService);
            Requires.NotNull(productInformation);
            Requires.NotNull(cameraProvider);
            Requires.NotNull(wizardProvider);
            Requires.NotNull(cameras);

            this.windowService = windowService;
            this.ioService = ioService;
            this.cameraProvider = cameraProvider;
            this.wizardProvider = wizardProvider;

            Title = Resources.SetupDialogViewModel_Title.CurrentFormat(productInformation.Version);

            Cameras = new ObservableCollection<ICameraViewModel>(cameras);
            SelectedItems = new ObservableCollection<object>();

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit, CanEdit);
            RemoveCommand = new RelayCommand(Remove, CanRemove);
        }

        /// <summary>
        /// Gets the title of the dialog.
        /// </summary>
        public string Title
        {
            get { return Property(() => Title); }
            private set { Property(() => Title, value); }
        }

        /// <summary>
        /// Gets the cameras.
        /// </summary>
        public ObservableCollection<ICameraViewModel> Cameras
        {
            get { return Property(() => Cameras); }
            private set { Property(() => Cameras, value); }
        }

        /// <summary>
        /// Gets the selected items .
        /// </summary>
        public ObservableCollection<object> SelectedItems
        {
            get { return Property(() => SelectedItems); }
            private set { Property(() => SelectedItems, value); }
        }

        /// <summary>
        /// Gets the command adding a camera.
        /// </summary>
        public ICommand AddCommand
        {
            get { return Property(() => AddCommand); }
            private set { Property(() => AddCommand, value); }
        }

        /// <summary>
        /// Gets the command editing a camera.
        /// </summary>
        public ICommand EditCommand
        {
            get { return Property(() => EditCommand); }
            private set { Property(() => EditCommand, value); }
        }

        /// <summary>
        /// Gets the command removing a camera.
        /// </summary>
        public ICommand RemoveCommand
        {
            get { return Property(() => RemoveCommand); }
            private set { Property(() => RemoveCommand, value); }
        }

        /// <summary>
        /// Adds a new camera.
        /// </summary>
        private void Add(object parameter)
        {
            ConfigurableCamera camera = new ConfigurableCamera(Guid.NewGuid())
            {
                Port = 80,
                UserName = "root",
                VideoSource = 1
            };

            IWizardDialogViewModel wizard = wizardProvider.Provide(
                Resources.AddCamera_Title,
                camera);

            if (windowService.ShowDialog<WizardDialog>(wizard, this) == true)
            {
                Log.Debug("Added camera {0}", wizard.Camera.Name);

                Cameras.Add(cameraProvider.Provide(wizard.Camera));
            }
        }

        /// <summary>
        /// Edits a camera.
        /// </summary>
        private void Edit(object parameter)
        {
            CameraViewModel selectedViewModel = (CameraViewModel)SelectedItems.Single();

            IWizardDialogViewModel wizard = wizardProvider.Provide(
                Resources.EditCamera_Title,
                selectedViewModel.Camera.Clone());

            if (windowService.ShowDialog<WizardDialog>(wizard, this) == true)
            {
                Log.Debug("Edited camera {0}", wizard.Camera.Name);

                selectedViewModel.Camera = wizard.Camera;
            }
        }

        /// <summary>
        /// Determines whether a camera can be edited.
        /// </summary>
        /// <returns>True if a camera can be edited; otherwise false.</returns>
        private bool CanEdit(object parameter)
        {
            return SelectedItems.Count == 1;
        }

        /// <summary>
        /// Removes a camera.
        /// </summary>
        private void Remove(object parameter)
        {
            MessageBoxResult result = windowService.ShowMessageBox(
                this,
                Resources.RemoveCameraWarning,
                Resources.RemoveCameraWarning_Title,
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                ICameraViewModel camera = (ICameraViewModel)SelectedItems.Single();

                Log.Debug("Removed camera {0}", camera.Camera.Name);

                Cameras.Remove(camera);
                ioService.DeleteThumb(camera.Camera.Id);
            }
        }

        /// <summary>
        /// Determines whether a camera can be removed.
        /// </summary>
        /// <returns>True if a camera can be removed; otherwise false.</returns>
        private bool CanRemove(object parameter)
        {
            return SelectedItems.Count == 1;
        }
    }
}