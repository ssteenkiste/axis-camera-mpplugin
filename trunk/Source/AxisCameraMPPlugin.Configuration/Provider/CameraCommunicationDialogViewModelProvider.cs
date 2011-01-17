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
using AxisCameraMPPlugin.Core;

namespace AxisCameraMPPlugin.Configuration.Provider
{
	/// <summary>
	/// Class describing a provider that provides a ICameraCommunicationDialogViewModel.
	/// </summary>
	class CameraCommunicationDialogViewModelProvider : ICameraCommunicationDialogViewModelProvider
	{
		private readonly ICameraCommunicationService cameraCommunicationService;


		/// <summary>
		/// Initializes a new instance of the <see cref="CameraCommunicationDialogViewModelProvider"/>
		/// class.
		/// </summary>
		/// <param name="cameraCommunicationService">The camera communication service.</param>
		public CameraCommunicationDialogViewModelProvider(
			ICameraCommunicationService cameraCommunicationService)
		{
			if (cameraCommunicationService == null) throw new ArgumentNullException("cameraCommunicationService");

			this.cameraCommunicationService = cameraCommunicationService;
		}


		/// <summary>
		/// Returns a ICameraCommunicationDialogViewModel from specified network endpoint.
		/// </summary>
		/// <param name="cameraEndpoint">The camera network endpoint.</param>
		public ICameraCommunicationDialogViewModel Provide(NetworkEndpoint cameraEndpoint)
		{
			if (cameraEndpoint == null) throw new ArgumentNullException("cameraEndpoint");

			Log.Debug("Provide a ICameraCommunicationDialogViewModel");

			return new CameraCommunicationDialogViewModel(
				cameraCommunicationService,
				cameraEndpoint);
		}
	}
}
