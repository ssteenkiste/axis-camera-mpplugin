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
using System.Windows;
using System.Windows.Input;
using AxisCameraMPPlugin.Configuration.Properties;
using AxisCameraMPPlugin.Configuration.Provider;
using AxisCameraMPPlugin.Configuration.Service;
using AxisCameraMPPlugin.Configuration.View;
using AxisCameraMPPlugin.Configuration.ViewModel.Data;
using AxisCameraMPPlugin.Configuration.ViewModel.ValidationRule;
using AxisCameraMPPlugin.Mvvm;
using AxisCameraMPPlugin.Mvvm.Services;

namespace AxisCameraMPPlugin.Configuration.ViewModel
{
	/// <summary>
	/// View model where camera name is entered and snapshot displayed.
	/// </summary>
	class WizardPageTwoViewModel : WizardPageViewModel, IWizardPageTwoViewModel
	{
		private readonly IWindowService windowService;
		private readonly ICameraCommunicationDialogViewModelProvider communicationProvider;

		private NetworkEndpoint cameraEndpoint;


		/// <summary>
		/// Initializes a new instance of the <see cref="WizardPageTwoViewModel"/> class.
		/// </summary>
		/// <param name="windowService">The window service.</param>
		/// <param name="communicationProvider">
		/// The camera communication dialog view model provider.
		/// </param>
		public WizardPageTwoViewModel(
			IWindowService windowService,
			ICameraCommunicationDialogViewModelProvider communicationProvider)
		{
			if (windowService == null) throw new ArgumentNullException("windowService");
			if (communicationProvider == null) throw new ArgumentNullException("communicationProvider");

			this.windowService = windowService;
			this.communicationProvider = communicationProvider;

			RefreshCommand = new RelayCommand(Refresh);

			AddValidators();
		}


		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name
		{
			get { return Property(() => Name); }
			set { Property(() => Name, value); }
		}


		/// <summary>
		/// Gets the snapshot.
		/// </summary>
		public byte[] Snapshot
		{
			get { return Property(() => Snapshot); }
			private set { Property(() => Snapshot, value); }
		}


		/// <summary>
		/// Gets the command refreshing the snapshot.
		/// </summary>
		public ICommand RefreshCommand
		{
			get { return Property(() => RefreshCommand); }
			private set { Property(() => RefreshCommand, value); }
		}


		/// <summary>
		/// Gets the header of the wizard page.
		/// </summary>
		public override string Header
		{
			get { return Resources.WizardPageTwo_Header; }
		}


		/// <summary>
		/// Gets the description of the wizard page.
		/// </summary>
		public override string Description
		{
			get { return Resources.WizardPageTwo_Description; }
		}


		/// <summary>
		/// Loads page properties from specified camera.
		/// </summary>
		/// <param name="camera">The camera to load page properties from.</param>
		public override void Load(ConfigurableCamera camera)
		{
			if (camera == null) throw new ArgumentNullException("camera");

			cameraEndpoint = new NetworkEndpoint(
				camera.Address,
				camera.Port,
				camera.UserName,
				camera.Password);

			Name = camera.Name;
			Snapshot = camera.Snapshot;
		}


		/// <summary>
		/// Saves page properties to specified camera.
		/// </summary>
		/// <param name="camera">The camera to save page properties to.</param>
		public override void Save(ConfigurableCamera camera)
		{
			if (camera == null) throw new ArgumentNullException("camera");

			camera.Name = Name;
			camera.Snapshot = Snapshot;
		}


		/// <summary>
		/// Refreshes the snapshot.
		/// </summary>
		private void Refresh(object parameter)
		{
			ICameraCommunicationDialogViewModel communicationViewModel =
				communicationProvider.Provide(cameraEndpoint);

			// Communicate with camera
			bool? success = windowService.ShowDialog<CameraCommunicationDialog>(
				communicationViewModel,
				this);

			// Was communication with camera successful?
			if (success == true)
			{
				Snapshot = communicationViewModel.Snapshot;
			}
			else
			{
				windowService.ShowMessageBox(
					this,
					Resources.CameraCommunicationError,
					Resources.CameraCommunicationError_Title,
					icon: MessageBoxImage.Error);
			}
		}


		/// <summary>
		/// Adds the validators.
		/// </summary>
		private void AddValidators()
		{
			AddValidator(
				() => Name,
				new NotEmptyStringValidationRule { ErrorMessage = Resources.Validation_Failed_CameraName });
		}
	}
}
