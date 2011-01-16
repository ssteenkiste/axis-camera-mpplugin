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
using AxisCameraMPPlugin.Configuration.Provider;
using AxisCameraMPPlugin.Configuration.Service;
using AxisCameraMPPlugin.Configuration.View;
using AxisCameraMPPlugin.Configuration.ViewModel;
using AxisCameraMPPlugin.Data;
using AxisCameraMPPlugin.Mvvm.Services;

namespace AxisCameraMPPlugin.Configuration
{
	/// <summary>
	/// Class acting as entry point for configuring the plugin.
	/// </summary>
	public class ConfigurationStarter : IConfigurationStarter
	{
		private readonly IWindowService windowService;
		private readonly IPluginSettings pluginSettings;
		private readonly ICurrentProcessService currentProcessService;
		private readonly ISetupDialogViewModelProvider setupProvider;
		private readonly ICameraConverter cameraConverter;
		private readonly ICameraNameViewModelProvider cameraViewModelProvider;


		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationStarter"/> class.
		/// </summary>
		/// <param name="windowService">The window service.</param>
		/// <param name="pluginSettings">The plugin settings.</param>
		/// <param name="currentProcessService">The current process service.</param>
		/// <param name="setupProvider">The setup view model provider.</param>
		/// <param name="cameraConverter">The camera converter.</param>
		/// <param name="cameraViewModelProvider">The camera name view model provider.</param>
		public ConfigurationStarter(
			IWindowService windowService,
			IPluginSettings pluginSettings,
			ICurrentProcessService currentProcessService,
			ISetupDialogViewModelProvider setupProvider,
			ICameraConverter cameraConverter,
			ICameraNameViewModelProvider cameraViewModelProvider)
		{
			if (windowService == null) throw new ArgumentNullException("windowService");
			if (pluginSettings == null) throw new ArgumentNullException("pluginSettings");
			if (currentProcessService == null) throw new ArgumentNullException("currentProcessService");
			if (setupProvider == null) throw new ArgumentNullException("setupProvider");
			if (cameraConverter == null) throw new ArgumentNullException("cameraConverter");
			if (cameraViewModelProvider == null) throw new ArgumentNullException("cameraViewModelProvider");

			this.windowService = windowService;
			this.pluginSettings = pluginSettings;
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
			IEnumerable<ICameraNameViewModel> cameraViewModels =
				from camera in pluginSettings.GetCameras()
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

			pluginSettings.SetCameras(cameras);
		}
	}
}
