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
using AxisCameraMPPlugin.Configuration.ViewModel;
using AxisCameraMPPlugin.Configuration.ViewModel.Data;

namespace AxisCameraMPPlugin.Configuration.Provider
{
	/// <summary>
	/// Class describing a provider that provides a IWizardDialogViewModel.
	/// </summary>
	class WizardDialogViewModelProvider : IWizardDialogViewModelProvider
	{
		private readonly IWizardPageViewModelsProvider wizardPagesProvider;


		/// <summary>
		/// Initializes a new instance of the <see cref="WizardDialogViewModelProvider"/> class.
		/// </summary>
		/// <param name="wizardPagesProvider">The wizard pages provider.</param>
		public WizardDialogViewModelProvider(IWizardPageViewModelsProvider wizardPagesProvider)
		{
			if (wizardPagesProvider == null) throw new ArgumentNullException("wizardPagesProvider");

			this.wizardPagesProvider = wizardPagesProvider;
		}


		/// <summary>
		/// Returns a IWizardDialogViewModel.
		/// </summary>
		/// <param name="title">The title of the dialog.</param>
		/// <param name="camera">The camera configured by the wizard.</param>
		public IWizardDialogViewModel Provide(
			string title,
			ConfigurableCamera camera)
		{
			if (title == null) throw new ArgumentNullException("title");
			if (camera == null) throw new ArgumentNullException("camera");

			return new WizardDialogViewModel(
				title,
				camera,
				wizardPagesProvider);
		}
	}
}
