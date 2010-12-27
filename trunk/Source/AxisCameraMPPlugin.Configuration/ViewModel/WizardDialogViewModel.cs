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
using System.Linq;
using System.Windows.Input;
using AxisCameraMPPlugin.Configuration.Provider;
using AxisCameraMPPlugin.Mvvm;

namespace AxisCameraMPPlugin.Configuration.ViewModel
{
	/// <summary>
	/// View model capable of adding or editing a camera.
	/// </summary>
	class WizardDialogViewModel : DialogViewModelBase, IWizardDialogViewModel
	{
		private readonly IList<IWizardPageViewModel> pages;


		/// <summary>
		/// Initializes a new instance of the <see cref="WizardDialogViewModel"/> class.
		/// </summary>
		/// <param name="title">The title of the dialog.</param>
		/// <param name="wizardPageViewModelsProvider">The wizard page view models provider.</param>
		public WizardDialogViewModel(
			string title,
			IWizardPageViewModelsProvider wizardPageViewModelsProvider)
		{
			if (title == null) throw new ArgumentNullException("title");
			if (wizardPageViewModelsProvider == null) throw new ArgumentNullException("wizardPageViewModelsProvider");

			pages = new List<IWizardPageViewModel>(wizardPageViewModelsProvider.Provide());
			
			Title = title;
			CurrentWizardPage = pages.First();
			PreviousCommand = new RelayCommand(Previous, CanPrevious);
			NextCommand = new RelayCommand(Next, CanNext);
			FinishCommand = new RelayCommand(Finish, CanFinish);
		}


		/// <summary>
		/// Gets the title of the dialog.
		/// </summary>
		public string Title
		{
			get { return Property(() => Title); }
			private set { Property(() => Title, value); }
		}


		/// <summary>
		/// Gets the current wizard page.
		/// </summary>
		public IWizardPageViewModel CurrentWizardPage
		{
			get { return Property(() => CurrentWizardPage); }
			private set
			{
				Property(() => CurrentWizardPage, value);
				OnPropertyChanged(() => Header);
				OnPropertyChanged(() => Description);
			}
		}


		/// <summary>
		/// Gets the header of the currently displayed wizard page.
		/// </summary>
		public string Header
		{
			get { return CurrentWizardPage.Header; }
		}


		/// <summary>
		/// Gets the description of the currently displayed wizard page.
		/// </summary>
		public string Description
		{
			get { return CurrentWizardPage.Description; }
		}


		/// <summary>
		/// Gets the command moving to the previous page in the wizard.
		/// </summary>
		public ICommand PreviousCommand
		{
			get { return Property(() => PreviousCommand); }
			private set { Property(() => PreviousCommand, value); }
		}


		/// <summary>
		/// Gets the command moving to the next page in the wizard.
		/// </summary>
		public ICommand NextCommand
		{
			get { return Property(() => NextCommand); }
			private set { Property(() => NextCommand, value); }
		}


		/// <summary>
		/// Gets the command finishing the wizard.
		/// </summary>
		public ICommand FinishCommand
		{
			get { return Property(() => FinishCommand); }
			private set { Property(() => FinishCommand, value); }
		}


		/// <summary>
		/// Moving to the previous page in the wizard.
		/// </summary>
		private void Previous(object parameter)
		{
			int currentPageIndex = pages.IndexOf(CurrentWizardPage);
			CurrentWizardPage = pages[currentPageIndex - 1];
		}


		/// <summary>
		/// Determines whether the previous page in the wizard can be shown. It can be shown if current
		/// page isn't the first.
		/// </summary>
		private bool CanPrevious(object parameter)
		{
			return CurrentWizardPage != pages.First();
		}


		/// <summary>
		/// Moving to the next page in the wizard.
		/// </summary>
		private void Next(object parameter)
		{
			int currentPageIndex = pages.IndexOf(CurrentWizardPage);
			CurrentWizardPage = pages[currentPageIndex + 1];
		}


		/// <summary>
		/// Determines whether the next page in the wizard can be shown. It can be shown if the current
		/// page is valid, and it isn't the last.
		/// </summary>
		private bool CanNext(object parameter)
		{
			return CurrentWizardPage.Validate() && CurrentWizardPage != pages.Last();
		}


		/// <summary>
		/// Finishes the wizard.
		/// </summary>
		private void Finish(object parameter)
		{
			DialogResultCommand.Execute(true);
		}


		/// <summary>
		/// Determines whether the wizard can be finished. Can be shown if current page is the last,
		/// and it is valid.
		/// </summary>
		private bool CanFinish(object parameter)
		{
			return CurrentWizardPage == pages.Last() && CurrentWizardPage.Validate();
		}
	}
}
