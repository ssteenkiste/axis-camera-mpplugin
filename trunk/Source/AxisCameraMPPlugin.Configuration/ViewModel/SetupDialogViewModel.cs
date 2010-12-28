#region Copyright (C) 2005-2010 Team MediaPortal

// Copyright (C) 2005-2010 Team MediaPortal
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
using AxisCameraMPPlugin.Configuration.Properties;
using AxisCameraMPPlugin.Configuration.Provider;
using AxisCameraMPPlugin.Configuration.View;
using AxisCameraMPPlugin.Data;
using AxisCameraMPPlugin.Mvvm;
using AxisCameraMPPlugin.Mvvm.Services;

namespace AxisCameraMPPlugin.Configuration.ViewModel
{
	/// <summary>
	/// View model capable of adding, editing and removing cameras.
	/// </summary>
	class SetupDialogViewModel : DialogViewModelBase, ISetupDialogViewModel
	{
		private readonly IWindowService windowService;
		private readonly ICameraNameViewModelProvider cameraProvider;
		private readonly Func<string, Camera, IWizardDialogViewModel> wizardProvider;


		/// <summary>
		/// Initializes a new instance of the <see cref="SetupDialogViewModel"/> class.
		/// </summary>
		/// <param name="windowService">The window service.</param>
		/// <param name="cameraProvider">The camera name view model provider.</param>
		/// <param name="wizardProvider">The wizard view model provider.</param>
		/// <param name="cameras">The cameras.</param>
		public SetupDialogViewModel(
			IWindowService windowService,
			ICameraNameViewModelProvider cameraProvider,
			Func<string, Camera, IWizardDialogViewModel> wizardProvider,
			IEnumerable<ICameraNameViewModel> cameras)
		{
			if (windowService == null) throw new ArgumentNullException("windowService");
			if (cameraProvider == null) throw new ArgumentNullException("cameraProvider");
			if (wizardProvider == null) throw new ArgumentNullException("wizardProvider");
			if (cameras == null) throw new ArgumentNullException("cameras");

			this.windowService = windowService;
			this.cameraProvider = cameraProvider;
			this.wizardProvider = wizardProvider;

			Cameras = new ObservableCollection<ICameraNameViewModel>(cameras);
			SelectedItems = new ObservableCollection<object>();

			AddCommand = new RelayCommand(Add);
			EditCommand = new RelayCommand(Edit, CanEdit);
			RemoveCommand = new RelayCommand(Remove, CanRemove);
		}


		/// <summary>
		/// Gets the cameras.
		/// </summary>
		public ObservableCollection<ICameraNameViewModel> Cameras
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
			Camera camera = new Camera
			{
				Id = Guid.NewGuid(),
				Port = 80,
				UserName = "root"
			};

			IWizardDialogViewModel wizard = wizardProvider(
				Resources.AddCamera_Title,
				camera);

			if (windowService.ShowDialog<WizardDialog>(wizard, this) == true)
			{
				Cameras.Add(cameraProvider.Provide(wizard.Camera));
			}
		}


		/// <summary>
		/// Edits a camera.
		/// </summary>
		private void Edit(object parameter)
		{
			CameraNameViewModel selectedViewModel = (CameraNameViewModel)SelectedItems.Single();

			IWizardDialogViewModel wizard = wizardProvider(
				Resources.EditCamera_Title,
				selectedViewModel.Camera.Clone());

			if (windowService.ShowDialog<WizardDialog>(wizard, this) == true)
			{
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
				Cameras.Remove((ICameraNameViewModel)SelectedItems.Single());
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
