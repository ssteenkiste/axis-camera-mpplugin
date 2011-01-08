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
using AxisCameraMPPlugin.Configuration.Service;
using AxisCameraMPPlugin.Configuration.ViewModel;

namespace AxisCameraMPPlugin.Configuration.Provider
{
	/// <summary>
	/// Class describing a provider that provides a ICameraCommunicationDialogViewModel.
	/// </summary>
	class CameraCommunicationDialogViewModelProvider : ICameraCommunicationDialogViewModelProvider
	{
		private readonly ICameraCommunicationService cameraCommunicationService;
		private readonly IIOService ioService;


		/// <summary>
		/// Initializes a new instance of the <see cref="CameraCommunicationDialogViewModelProvider"/>
		/// class.
		/// </summary>
		/// <param name="cameraCommunicationService">The camera communication service.</param>
		/// <param name="ioService">The I/O service.</param>
		public CameraCommunicationDialogViewModelProvider(
			ICameraCommunicationService cameraCommunicationService,
			IIOService ioService)
		{
			if (cameraCommunicationService == null) throw new ArgumentNullException("cameraCommunicationService");
			if (ioService == null) throw new ArgumentNullException("ioService");

			this.cameraCommunicationService = cameraCommunicationService;
			this.ioService = ioService;
		}


		/// <summary>
		/// Returns a ICameraCommunicationDialogViewModel from specified camera ID and network
		/// endpoint.
		/// </summary>
		/// <param name="cameraId">The camera ID.</param>
		/// <param name="cameraEndpoint">The camera network endpoint.</param>
		public ICameraCommunicationDialogViewModel Provide(
			Guid cameraId,
			NetworkEndpoint cameraEndpoint)
		{
			if (cameraId == null) throw new ArgumentNullException("cameraId");
			if (cameraEndpoint == null) throw new ArgumentNullException("cameraEndpoint");

			return new CameraCommunicationDialogViewModel(
				cameraCommunicationService,
				ioService,
				cameraId,
				cameraEndpoint);
		}
	}
}
