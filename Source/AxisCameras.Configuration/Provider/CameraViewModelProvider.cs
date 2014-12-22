#region Copyright (C) 2005-2014 Team MediaPortal

// Copyright (C) 2005-2014 Team MediaPortal
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
using AxisCameras.Configuration.Service;
using AxisCameras.Configuration.ViewModel;
using AxisCameras.Configuration.ViewModel.Data;
using AxisCameras.Core;
using AxisCameras.Core.Contracts;

namespace AxisCameras.Configuration.Provider
{
    /// <summary>
    /// Class describing a provider that provides a ICameraViewModel.
    /// </summary>
    internal class CameraViewModelProvider : ICameraViewModelProvider
    {
        private readonly IBrowserService browserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraViewModelProvider"/> class.
        /// </summary>
        /// <param name="browserService">The browser service.</param>
        public CameraViewModelProvider(IBrowserService browserService)
        {
            Requires.NotNull(browserService);

            this.browserService = browserService;
        }

        /// <summary>
        /// Returns a ICameraViewModel created from a ConfigurableCamera.
        /// </summary>
        /// <param name="camera">The camera to turn into a ICameraViewModel.</param>
        /// <param name="editCommandProvider">
        /// A provider for the command editing the camera.
        /// </param>
        public ICameraViewModel Provide(ConfigurableCamera camera, Func<ICommand> editCommandProvider)
        {
            Requires.NotNull(camera);
            Requires.NotNull(editCommandProvider);

            Log.Debug("Provide a ICameraViewModel");

            return new CameraViewModel(camera, browserService, editCommandProvider);
        }
    }
}