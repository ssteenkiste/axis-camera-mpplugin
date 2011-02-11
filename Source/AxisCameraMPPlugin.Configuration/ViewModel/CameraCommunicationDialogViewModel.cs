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
using System.Windows.Input;
using Autofac.Features.OwnedInstances;
using AxisCameraMPPlugin.Configuration.Service;
using AxisCameraMPPlugin.Mvvm;

namespace AxisCameraMPPlugin.Configuration.ViewModel
{
	/// <summary>
	/// View model responsible for communicating with a camera.
	/// </summary>
	class CameraCommunicationDialogViewModel : DialogViewModelBase, ICameraCommunicationDialogViewModel
	{
		private readonly Owned<ICameraCommunicationService> cameraCommunicationService;
		private readonly NetworkEndpoint cameraEndpoint;


		/// <summary>
		/// Initializes a new instance of the <see cref="CameraCommunicationDialogViewModel"/> class.
		/// </summary>
		/// <param name="cameraCommunicationService">The camera communication service.</param>
		/// <param name="cameraEndpoint">The camera network endpoint.</param>
		public CameraCommunicationDialogViewModel(
			Owned<ICameraCommunicationService> cameraCommunicationService,
			NetworkEndpoint cameraEndpoint)
		{
			if (cameraCommunicationService == null) throw new ArgumentNullException("cameraCommunicationService");
			if (cameraEndpoint == null) throw new ArgumentNullException("cameraEndpoint");

			this.cameraCommunicationService = cameraCommunicationService;
			this.cameraEndpoint = cameraEndpoint;

			LoadedCommand = new RelayCommand(Loaded);
			CancelCommand = new RelayCommand(Cancel);
		}


		/// <summary>
		/// Gets the loaded command.
		/// </summary>
		public ICommand LoadedCommand
		{
			get { return Property(() => LoadedCommand); }
			private set { Property(() => LoadedCommand, value); }
		}


		/// <summary>
		/// Gets the command canceling the dialog.
		/// </summary>
		public ICommand CancelCommand
		{
			get { return Property(() => CancelCommand); }
			private set { Property(() => CancelCommand, value); }
		}


		/// <summary>
		/// Gets the friendly name of the camera.
		/// </summary>
		public string FriendlyName
		{
			get { return Property(() => FriendlyName); }
			private set { Property(() => FriendlyName, value); }
		}


		/// <summary>
		/// Gets the firmware version of a camera.
		/// </summary>
		public string FirmwareVersion
		{
			get { return Property(() => FirmwareVersion); }
			private set { Property(() => FirmwareVersion, value); }
		}


		/// <summary>
		/// Gets the camera snapshot.
		/// </summary>
		public IEnumerable<byte> Snapshot
		{
			get { return Property(() => Snapshot); }
			private set { Property(() => Snapshot, value); }
		}


		/// <summary>
		/// Executes the Loaded command.
		/// </summary>
		private void Loaded(object parameter)
		{
			EventHandler<GetInformationFromCameraCompletedEventArgs> completedHandler = null;
			completedHandler = (sender, e) =>
			{
				cameraCommunicationService.Value.GetInformationFromCameraCompleted -= completedHandler;
				cameraCommunicationService.Dispose();

				if (e.Cancelled || e.Error != null)
				{
					// Close dialog unsuccessfully
					DialogResultCommand.Execute(false);
				}
				else
				{
					FriendlyName = e.FriendlyName;
					FirmwareVersion = e.FirmwareVersion;
					Snapshot = e.Snapshot;

					// Close dialog successfully
					DialogResultCommand.Execute(true);
				}
			};

			cameraCommunicationService.Value.GetInformationFromCameraCompleted += completedHandler;
			cameraCommunicationService.Value.GetInformationFromCameraAsync(cameraEndpoint);
		}


		/// <summary>
		/// Executes the Cancel command.
		/// </summary>
		private void Cancel(object parameter)
		{
			cameraCommunicationService.Value.CancelAsync();
		}
	}
}
