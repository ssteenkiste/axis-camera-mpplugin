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
using System.Windows.Input;
using AxisCameraMPPlugin.Configuration.Service;
using AxisCameraMPPlugin.Data;
using AxisCameraMPPlugin.Mvvm;

namespace AxisCameraMPPlugin.Configuration.ViewModel
{
	/// <summary>
	/// View model responsible for communicating with a camera.
	/// </summary>
	class CameraCommunicationDialogViewModel : DialogViewModelBase, ICameraCommunicationDialogViewModel
	{
		private readonly ICameraCommunicationService cameraCommunicationService;
		private readonly IIOService ioService;
		private readonly Camera camera;


		/// <summary>
		/// Initializes a new instance of the <see cref="CameraCommunicationDialogViewModel"/> class.
		/// </summary>
		/// <param name="cameraCommunicationService">The camera communication service.</param>
		/// <param name="ioService">The I/O service.</param>
		/// <param name="camera">The camera.</param>
		public CameraCommunicationDialogViewModel(
			ICameraCommunicationService cameraCommunicationService,
			IIOService ioService,
			Camera camera)
		{
			if (cameraCommunicationService == null) throw new ArgumentNullException("cameraCommunicationService");
			if (ioService == null) throw new ArgumentNullException("ioService");
			if (camera == null) throw new ArgumentNullException("camera");

			this.cameraCommunicationService = cameraCommunicationService;
			this.ioService = ioService;
			this.camera = camera;

			LoadedCommand = new RelayCommand(Loaded);
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
		/// Method called when dialog is loaded.
		/// </summary>
		private void Loaded(object parameter)
		{
			EventHandler<GetInformationFromCameraCompletedEventArgs> completedHandler = null;
			completedHandler = (sender, e) =>
			{
				cameraCommunicationService.GetInformationFromCameraCompleted -= completedHandler;

				if (e.Cancelled || e.Error != null)
				{
					// Close dialog unsuccessfully
					DialogResultCommand.Execute(false);
				}
				else
				{
					camera.Name = e.FriendlyName;
					camera.SnapshotPath = ioService.SaveThumb(camera.Id, e.Snapshot);

					// Close dialog successfully
					DialogResultCommand.Execute(true);
				}
			};

			cameraCommunicationService.GetInformationFromCameraCompleted += completedHandler;
			cameraCommunicationService.GetInformationFromCameraAsync(camera);
		}
	}
}
