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
		private readonly ICameraCommunicationDialogViewModelProvider communicationProvider;


		/// <summary>
		/// Initializes a new instance of the <see cref="WizardPageViewModelsProvider"/> class.
		/// </summary>
		/// <param name="windowService">The window service.</param>
		/// <param name="communicationProvider">
		/// The camera communication dialog view model provider.
		/// </param>
		public WizardPageViewModelsProvider(
			IWindowService windowService,
			ICameraCommunicationDialogViewModelProvider communicationProvider)
		{
			if (windowService == null) throw new ArgumentNullException("windowService");
			if (communicationProvider == null) throw new ArgumentNullException("communicationProvider");

			this.windowService = windowService;
			this.communicationProvider = communicationProvider;
		}


		/// <summary>
		/// Returns a collection of IWizardPageViewModels.
		/// </summary>
		public IEnumerable<IWizardPageViewModel> Provide()
		{
			IWizardPageViewModel pageOne = new WizardPageOneViewModel(
				windowService,
				communicationProvider);

			IWizardPageViewModel pageTwo = new WizardPageTwoViewModel();

			IWizardPageViewModel pageThree = new WizardPageThreeViewModel(
				windowService,
				communicationProvider);

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
