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
using System.Windows.Input;
using AxisCameras.Configuration.ViewModel;
using AxisCameras.Configuration.ViewModel.Data;

namespace AxisCameras.Configuration.Provider
{
    /// <summary>
    /// Interface describing a provider that provides a ICameraViewModel.
    /// </summary>
    public interface ICameraViewModelProvider
    {
        /// <summary>
        /// Returns a ICameraViewModel created from a ConfigurableCamera.
        /// </summary>
        /// <param name="camera">The camera to turn into a ICameraViewModel.</param>
        /// <param name="editCommandProvider">
        /// A provider for the command editing the camera.
        /// </param>
        ICameraViewModel Provide(ConfigurableCamera camera, Func<ICommand> editCommandProvider);
    }
}