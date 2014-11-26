#region Copyright (C) 2005-2011 Team MediaPortal

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

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Autofac.Features.OwnedInstances;
using AxisCameras.Configuration.Provider;
using AxisCameras.Configuration.Service;
using AxisCameras.Configuration.View;
using AxisCameras.Configuration.ViewModel;
using AxisCameras.Core;
using AxisCameras.Core.Contracts;
using AxisCameras.Data;
using AxisCameras.Mvvm.Services;

namespace AxisCameras.Configuration
{
    /// <summary>
    /// Class acting as entry point for configuring the plugin.
    /// </summary>
    public class ConfigurationStarter : IConfigurationStarter
    {
        private readonly IWindowService windowService;
        private readonly ICurrentProcessService currentProcessService;
        private readonly Func<Owned<IPluginSettings>> pluginSettingsProvider;
        private readonly ISetupDialogViewModelProvider setupProvider;
        private readonly ICameraConverter cameraConverter;
        private readonly ICameraViewModelProvider cameraViewModelProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationStarter"/> class.
        /// </summary>
        /// <param name="windowService">The window service.</param>
        /// <param name="currentProcessService">The current process service.</param>
        /// <param name="pluginSettingsProvider">A provider providing the plugin settings.</param>
        /// <param name="setupProvider">The setup view model provider.</param>
        /// <param name="cameraConverter">The camera converter.</param>
        /// <param name="cameraViewModelProvider">The camera name view model provider.</param>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Nested generic types aren't complex.")]
        public ConfigurationStarter(
            IWindowService windowService,
            ICurrentProcessService currentProcessService,
            Func<Owned<IPluginSettings>> pluginSettingsProvider,
            ISetupDialogViewModelProvider setupProvider,
            ICameraConverter cameraConverter,
            ICameraViewModelProvider cameraViewModelProvider)
        {
            Requires.NotNull(windowService);
            Requires.NotNull(pluginSettingsProvider);
            Requires.NotNull(currentProcessService);
            Requires.NotNull(setupProvider);
            Requires.NotNull(cameraConverter);
            Requires.NotNull(cameraViewModelProvider);

            this.windowService = windowService;
            this.pluginSettingsProvider = pluginSettingsProvider;
            this.currentProcessService = currentProcessService;
            this.setupProvider = setupProvider;
            this.cameraConverter = cameraConverter;
            this.cameraViewModelProvider = cameraViewModelProvider;
        }

        /// <summary>
        /// Starts to configure the plugin.
        /// </summary>
        public void Start()
        {
            Log.Debug("Starting configuration");

            using (Owned<IPluginSettings> pluginSettings = pluginSettingsProvider())
            {
                IEnumerable<ICameraViewModel> cameraViewModels =
                    from camera in pluginSettings.Value.Cameras
                    let configurableCamera = cameraConverter.ToConfigurableCamera(camera)
                    select cameraViewModelProvider.Provide(configurableCamera);

                ISetupDialogViewModel setup = setupProvider.Provide(cameraViewModels);

                // Getting the window handle of the current process is a workaround since the owning window
                // is WinForms and we wish to open a WPF window
                windowService.ShowDialog<SetupDialog>(
                    setup,
                    currentProcessService.MainWindowHandle);

                // When the setup window has closed, save the cameras
                IEnumerable<Camera> cameras =
                    from camera in setup.Cameras
                    select cameraConverter.ToCamera(camera.Camera);

                pluginSettings.Value.Cameras = cameras;
            }
        }
    }
}