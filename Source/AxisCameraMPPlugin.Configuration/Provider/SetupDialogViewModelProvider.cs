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
using AxisCameraMPPlugin.Configuration.ViewModel;
using AxisCameraMPPlugin.Data;
using AxisCameraMPPlugin.Mvvm.Services;

namespace AxisCameraMPPlugin.Configuration.Provider
{
	/// <summary>
	/// Class describing a provider that provides a ISetupDialogViewModel.
	/// </summary>
	class SetupDialogViewModelProvider : ISetupDialogViewModelProvider
	{
		private readonly IWindowService windowService;
		private readonly ICameraNameViewModelProvider cameraProvider;
		private readonly Func<string, Camera, IWizardDialogViewModel> wizardProvider;


		/// <summary>
		/// Initializes a new instance of the <see cref="SetupDialogViewModelProvider"/> class.
		/// </summary>
		/// <param name="windowService">The window service.</param>
		/// <param name="cameraProvider">The camera name view model provider.</param>
		/// <param name="wizardProvider">The wizard view model provider.</param>
		public SetupDialogViewModelProvider(
			IWindowService windowService,
			ICameraNameViewModelProvider cameraProvider,
			Func<string, Camera, IWizardDialogViewModel> wizardProvider)
		{
			if (windowService == null) throw new ArgumentNullException("windowService");
			if (cameraProvider == null) throw new ArgumentNullException("cameraProvider");
			if (wizardProvider == null) throw new ArgumentNullException("wizardProvider");

			this.windowService = windowService;
			this.cameraProvider = cameraProvider;
			this.wizardProvider = wizardProvider;
		}


		/// <summary>
		/// Returns a ISetupDialogViewModel.
		/// </summary>
		/// <param name="cameras">The cameras.</param>
		public ISetupDialogViewModel Provide(IEnumerable<ICameraNameViewModel> cameras)
		{
			if (cameras == null) throw new ArgumentNullException("cameras");

			return new SetupDialogViewModel(
				windowService,
				cameraProvider,
				wizardProvider,
				cameras);
		}
	}
}
