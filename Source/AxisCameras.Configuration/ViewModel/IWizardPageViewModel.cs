﻿#region Copyright (C) 2005-2011 Team MediaPortal

// Copyright (C) 2005-2011 Team MediaPortal
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

using AxisCameras.Configuration.ViewModel.Data;
using AxisCameras.Mvvm;

namespace AxisCameras.Configuration.ViewModel
{
    /// <summary>
    /// Interface for WizardPageViewModel.
    /// </summary>
    internal interface IWizardPageViewModel : IViewModelBase
    {
        /// <summary>
        /// Gets the header of the wizard page.
        /// </summary>
        string Header { get; }

        /// <summary>
        /// Gets the description of the wizard page.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Loads page properties from specified camera.
        /// </summary>
        /// <param name="camera">The camera to load page properties from.</param>
        void Load(ConfigurableCamera camera);

        /// <summary>
        /// Saves page properties to specified camera.
        /// </summary>
        /// <param name="camera">The camera to save page properties to.</param>
        void Save(ConfigurableCamera camera);

        /// <summary>
        /// Instruct the wizard whether wizard page should be skipped.
        /// </summary>
        /// <param name="camera">The camera displayed in the page.</param>
        /// <returns>true to skip page; otherwise false.</returns>
        bool ShouldSkipPage(ConfigurableCamera camera);
    }
}