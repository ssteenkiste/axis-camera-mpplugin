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
	/// Class describing a provider that provides a sequence of IWizardPageViewModels.
	/// </summary>
	class WizardPageViewModelsProvider : IWizardPageViewModelsProvider
	{
		private readonly IWindowService windowService;
		private readonly ICameraCommunicationDialogViewModelProvider cameraCommunicationProvider;


		/// <summary>
		/// Initializes a new instance of the <see cref="WizardPageViewModelsProvider"/> class.
		/// </summary>
		/// <param name="windowService">The window service.</param>
		public WizardPageViewModelsProvider(
			IWindowService windowService,
			ICameraCommunicationDialogViewModelProvider cameraCommunicationProvider)
		{
			if (windowService == null) throw new ArgumentNullException("windowService");
			if (cameraCommunicationProvider == null) throw new ArgumentNullException("cameraCommunicationProvider");

			this.windowService = windowService;
			this.cameraCommunicationProvider = cameraCommunicationProvider;
		}




		/// <summary>
		/// Returns a collection of IWizardPageViewModels.
		/// </summary>
		/// <param name="camera">The camera configured by the wizard pages.</param>
		public IEnumerable<IWizardPageViewModel> Provide(Camera camera)
		{
			if (camera == null) throw new ArgumentNullException("camera");

			IWizardPageViewModel pageOne = new WizardPageOneViewModel(
				windowService,
				cameraCommunicationProvider);
			pageOne.Load(camera);

			IWizardPageViewModel pageTwo = new WizardPageTwoViewModel();
			pageTwo.Load(camera);

			return new IWizardPageViewModel[]
			{
				pageOne,
				pageTwo
			};
		}
	}
}
