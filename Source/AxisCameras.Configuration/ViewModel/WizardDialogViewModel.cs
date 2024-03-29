﻿#region Copyright (C) 2005-2015 Team MediaPortal

// Copyright (C) 2005-2015 Team MediaPortal
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
using AxisCameras.Configuration.Provider;
using AxisCameras.Configuration.ViewModel.Data;
using AxisCameras.Core.Contracts;
using AxisCameras.Mvvm;

namespace AxisCameras.Configuration.ViewModel
{
    /// <summary>
    /// View model capable of adding or editing a camera.
    /// </summary>
    internal class WizardDialogViewModel : DialogViewModelBase, IWizardDialogViewModel
    {
        private readonly ConfigurableCamera camera;
        private readonly string title;
        private readonly IList<IWizardPageViewModel> pages;
        private readonly ICommand previousCommand;
        private readonly ICommand nextCommand;
        private readonly ICommand finishCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardDialogViewModel"/> class.
        /// </summary>
        /// <param name="title">The title of the dialog.</param>
        /// <param name="camera">The camera configured by the wizard.</param>
        /// <param name="wizardPagesProvider">The wizard page view models provider.</param>
        public WizardDialogViewModel(
            string title,
            ConfigurableCamera camera,
            IWizardPageViewModelsProvider wizardPagesProvider)
        {
            Requires.NotNullOrEmpty(title);
            Requires.NotNull(camera);
            Requires.NotNull(wizardPagesProvider);

            this.title = title;
            this.camera = camera;

            pages = new List<IWizardPageViewModel>(wizardPagesProvider.Provide());
            previousCommand = new RelayCommand(Previous, CanPrevious);
            nextCommand = new RelayCommand(Next, CanNext);
            finishCommand = new RelayCommand(Finish, CanFinish);

            LoadAndShowPage(pages.First());
        }

        /// <summary>
        /// Gets the title of the dialog.
        /// </summary>
        public string Title
        {
            get { return title; }
        }

        /// <summary>
        /// Gets the current wizard page.
        /// </summary>
        public IWizardPageViewModel CurrentWizardPage
        {
            get { return GetValue<IWizardPageViewModel>(); }
            private set
            {
                if (SetValue(value))
                {
                    OnPropertyChanged(() => Header);
                    OnPropertyChanged(() => Description);
                }
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
            get { return previousCommand; }
        }

        /// <summary>
        /// Gets the command moving to the next page in the wizard.
        /// </summary>
        public ICommand NextCommand
        {
            get { return nextCommand; }
        }

        /// <summary>
        /// Gets the command finishing the wizard.
        /// </summary>
        public ICommand FinishCommand
        {
            get { return finishCommand; }
        }

        /// <summary>
        /// Gets the camera.
        /// </summary>
        public ConfigurableCamera Camera
        {
            get { return camera; }
        }

        /// <summary>
        /// Moving to the previous page in the wizard.
        /// </summary>
        private void Previous(object parameter)
        {
            // Save settings from page
            SaveCurrentPage();

            // Find previous page wishing to be displayed
            for (int index = pages.IndexOf(CurrentWizardPage) - 1; index >= 0; index--)
            {
                if (!pages[index].ShouldSkipPage(Camera))
                {
                    // Load settings to page and show it
                    LoadAndShowPage(pages[index]);
                    return;
                }
            }

            throw new InvalidOperationException("No previous page wishes to be displayed.");
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
            // Validate current page before moving to next page
            if (CurrentWizardPage.Validate())
            {
                // Save settings from page
                SaveCurrentPage();

                // Find next page wishing to be displayed
                for (int index = pages.IndexOf(CurrentWizardPage) + 1; index < pages.Count; index++)
                {
                    if (!pages[index].ShouldSkipPage(Camera))
                    {
                        // Load settings to page and show it
                        LoadAndShowPage(pages[index]);
                        return;
                    }
                }

                throw new InvalidOperationException("No next page wishes to be displayed.");
            }
        }

        /// <summary>
        /// Determines whether the next page in the wizard can be shown. It can be shown if the current
        /// page isn't the last.
        /// </summary>
        private bool CanNext(object parameter)
        {
            return CurrentWizardPage != pages.Last();
        }

        /// <summary>
        /// Finishes the wizard.
        /// </summary>
        private void Finish(object parameter)
        {
            // Validate last page page before finishing
            if (CurrentWizardPage.Validate())
            {
                // Save page properties to camera
                CurrentWizardPage.Save(Camera);

                DialogResultCommand.Execute(true);
            }
        }

        /// <summary>
        /// Determines whether the wizard can be finished. Can be shown if current page is the last.
        /// </summary>
        private bool CanFinish(object parameter)
        {
            return CurrentWizardPage == pages.Last();
        }

        /// <summary>
        /// Loads the camera settings to specified page and show it in the wizard.
        /// </summary>
        /// <param name="pageViewModel">
        /// The page to load the camera settings to and show in the wizard.
        /// </param>
        private void LoadAndShowPage(IWizardPageViewModel pageViewModel)
        {
            if (pageViewModel != null)
            {
                pageViewModel.Load(Camera);

                CurrentWizardPage = pageViewModel;
            }
        }

        /// <summary>
        /// Saves the camera settings entered in current page.
        /// </summary>
        private void SaveCurrentPage()
        {
            if (CurrentWizardPage != null)
            {
                CurrentWizardPage.Save(Camera);
            }
        }
    }
}