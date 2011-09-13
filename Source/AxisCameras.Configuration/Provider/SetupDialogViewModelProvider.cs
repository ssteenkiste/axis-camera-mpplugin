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
using AxisCameras.Configuration.Service;
using AxisCameras.Configuration.ViewModel;
using AxisCameras.Core;
using AxisCameras.Core.Contracts;
using AxisCameras.Mvvm.Services;

namespace AxisCameras.Configuration.Provider
{
	/// <summary>
	/// Class describing a provider that provides a ISetupDialogViewModel.
	/// </summary>
	class SetupDialogViewModelProvider : ISetupDialogViewModelProvider
	{
		private readonly IWindowService windowService;
		private readonly IIOService ioService;
		private readonly IProductInformation productInformation;
		private readonly ICameraViewModelProvider cameraProvider;
		private readonly WizardDialogViewModelProvider wizardProvider;


		/// <summary>
		/// Initializes a new instance of the <see cref="SetupDialogViewModelProvider"/> class.
		/// </summary>
		/// <param name="windowService">The window service.</param>
		/// <param name="ioService">The I/O service.</param>
		/// <param name="productInformation">The product information.</param>
		/// <param name="cameraProvider">The camera name view model provider.</param>
		/// <param name="wizardProvider">The wizard view model provider.</param>
		public SetupDialogViewModelProvider(
			IWindowService windowService,
			IIOService ioService,
			IProductInformation productInformation,
			ICameraViewModelProvider cameraProvider,
			WizardDialogViewModelProvider wizardProvider)
		{
			Requires.NotNull(windowService);
			Requires.NotNull(ioService);
			Requires.NotNull(productInformation);
			Requires.NotNull(cameraProvider);
			Requires.NotNull(wizardProvider);

			this.windowService = windowService;
			this.ioService = ioService;
			this.productInformation = productInformation;
			this.cameraProvider = cameraProvider;
			this.wizardProvider = wizardProvider;
		}


		/// <summary>
		/// Returns a ISetupDialogViewModel.
		/// </summary>
		/// <param name="cameras">The cameras.</param>
		public ISetupDialogViewModel Provide(IEnumerable<ICameraViewModel> cameras)
		{
			Requires.NotNull(cameras);

			Log.Debug("Provide a ISetupDialogViewModel");

			return new SetupDialogViewModel(
				windowService,
				ioService,
				productInformation,
				cameraProvider,
				wizardProvider,
				cameras);
		}
	}
}