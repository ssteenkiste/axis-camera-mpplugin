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
using AxisCameras.Mvvm.Services;

namespace AxisCameras.Configuration.Provider
{
	/// <summary>
	/// Class describing a provider that provides a sequence of IWizardPageViewModels.
	/// </summary>
	class WizardPageViewModelsProvider : IWizardPageViewModelsProvider
	{
		private readonly IWindowService windowService;
		private readonly ICameraParametersDialogViewModelProvider cameraParametersProvider;
		private readonly ICameraSnapshotDialogViewModelProvider cameraSnapshotProvider;
		private readonly IResourceService resourceService;
		private readonly IDispatcherService dispatcherService;


		/// <summary>
		/// Initializes a new instance of the <see cref="WizardPageViewModelsProvider"/> class.
		/// </summary>
		/// <param name="windowService">The window service.</param>
		/// <param name="cameraParametersProvider">
		/// The camera parameters dialog view model provider.
		/// </param>
		/// <param name="cameraSnapshotProvider">
		/// The camera snapshot dialog view model provider.
		/// </param>
		/// <param name="resourceService">The resource service.</param>
		/// <param name="dispatcherService">The dispatcher service.</param>
		public WizardPageViewModelsProvider(
			IWindowService windowService,
			ICameraParametersDialogViewModelProvider cameraParametersProvider,
			ICameraSnapshotDialogViewModelProvider cameraSnapshotProvider,
			IResourceService resourceService,
			IDispatcherService dispatcherService)
		{
			if (windowService == null) throw new ArgumentNullException("windowService");
			if (cameraParametersProvider == null) throw new ArgumentNullException("cameraParametersProvider");
			if (cameraSnapshotProvider == null) throw new ArgumentNullException("cameraSnapshotProvider");
			if (resourceService == null) throw new ArgumentNullException("resourceService");
			if (dispatcherService == null) throw new ArgumentNullException("dispatcherService");

			this.windowService = windowService;
			this.cameraParametersProvider = cameraParametersProvider;
			this.cameraSnapshotProvider = cameraSnapshotProvider;
			this.resourceService = resourceService;
			this.dispatcherService = dispatcherService;
		}


		/// <summary>
		/// Returns a collection of IWizardPageViewModels.
		/// </summary>
		public IEnumerable<IWizardPageViewModel> Provide()
		{
			IWizardPageViewModel pageOne = new WizardPageOneViewModel(
				windowService,
				cameraParametersProvider);

			IWizardPageViewModel pageTwo = new WizardPageTwoViewModel();

			IWizardPageViewModel pageThree = new WizardPageThreeViewModel(
				windowService,
				resourceService,
				dispatcherService,
				cameraSnapshotProvider);

			Log.Debug("Provide a sequence of IWizardPageViewModels");

			return new IWizardPageViewModel[]
			{
				pageOne,
				pageTwo,
				pageThree
			};
		}
	}
}
