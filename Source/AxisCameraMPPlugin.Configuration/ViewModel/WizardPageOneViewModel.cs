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
using System.Globalization;
using System.Windows;
using AxisCameraMPPlugin.Configuration.Properties;
using AxisCameraMPPlugin.Configuration.Provider;
using AxisCameraMPPlugin.Configuration.Service;
using AxisCameraMPPlugin.Configuration.View;
using AxisCameraMPPlugin.Configuration.ViewModel.ValidationRule;
using AxisCameraMPPlugin.Data;
using AxisCameraMPPlugin.Mvvm.Services;

namespace AxisCameraMPPlugin.Configuration.ViewModel
{
	/// <summary>
	/// View model where camera network endpoint is entered.
	/// </summary>
	class WizardPageOneViewModel : WizardPageViewModel, IWizardPageOneViewModel
	{
		private readonly IWindowService windowService;
		private readonly ICameraCommunicationDialogViewModelProvider communicationProvider;

		private Guid cameraId;
		private string friendlyName;
		private string snapshotPath;


		/// <summary>
		/// Initializes a new instance of the <see cref="WizardPageOneViewModel"/> class.
		/// </summary>
		/// <param name="windowService">The window service.</param>
		/// <param name="communicationProvider">
		/// The camera communication dialog view model provider.
		/// </param>
		public WizardPageOneViewModel(
			IWindowService windowService,
			ICameraCommunicationDialogViewModelProvider communicationProvider)
		{
			if (windowService == null) throw new ArgumentNullException("windowService");
			if (communicationProvider == null) throw new ArgumentNullException("communicationProvider");

			this.windowService = windowService;
			this.communicationProvider = communicationProvider;

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
		public override void Load(Camera camera)
		{
			if (camera == null) throw new ArgumentNullException("camera");

			cameraId = camera.Id;
			friendlyName = camera.Name;
			snapshotPath = camera.SnapshotPath;

			Address = camera.Address;
			Port = camera.Port.ToString(CultureInfo.CurrentCulture);
			UserName = camera.UserName;
			Password = camera.Password;
		}


		/// <summary>
		/// Saves page properties to specified camera.
		/// </summary>
		/// <param name="camera">The camera to save page properties to.</param>
		public override void Save(Camera camera)
		{
			if (camera == null) throw new ArgumentNullException("camera");

			camera.Name = friendlyName;
			camera.SnapshotPath = snapshotPath;

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

			// If view model is valid and friendly name and snapshot is unknown, get them from camera
			if (isValid && friendlyName == null && snapshotPath == null)
			{
				NetworkEndpoint cameraEndpoint = new NetworkEndpoint(
					Address,
					int.Parse(Port, CultureInfo.CurrentCulture),
					UserName,
					Password);

				ICameraCommunicationDialogViewModel communicationViewModel =
					communicationProvider.Provide(cameraId, cameraEndpoint);

				// Communicate with camera
				bool? success = windowService.ShowDialog<CameraCommunicationDialog>(
					communicationViewModel,
					this);

				friendlyName = communicationViewModel.FriendlyName;
				snapshotPath = communicationViewModel.SnapshotPath;

				// Was communication with camera unsuccessful?
				if (success != true)
				{
					isValid = false;

					windowService.ShowMessageBox(
						this,
						Resources.CameraCommunicationError,
						Resources.CameraCommunicationError_Title,
						icon: MessageBoxImage.Error);
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
				new NotEmptyStringValidationRule { ErrorMessage = Resources.Validation_Failed_Address });
			AddValidator(
				() => Port,
				new PortValidationRule());
			AddValidator(
				() => UserName,
				new UserNameValidationRule());
			AddValidator(
				() => Password,
				new NotEmptyStringValidationRule { ErrorMessage = Resources.Validation_Failed_Password });
		}
	}
}
