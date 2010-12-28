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
		private readonly ICameraNameViewModelProvider cameraProvider;


		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationStarter"/> class.
		/// </summary>
		/// <param name="windowService">The window service.</param>
		/// <param name="pluginSettings">The plugin settings.</param>
		/// <param name="currentProcessService">The current process service.</param>
		/// <param name="setupProvider">The setup view model provider.</param>
		/// <param name="cameraProvider">The camera name view model provider.</param>
		public ConfigurationStarter(
			IWindowService windowService,
			IPluginSettings pluginSettings,
			ICurrentProcessService currentProcessService,
			ISetupDialogViewModelProvider setupProvider,
			ICameraNameViewModelProvider cameraProvider)
		{
			if (windowService == null) throw new ArgumentNullException("windowService");
			if (pluginSettings == null) throw new ArgumentNullException("pluginSettings");
			if (currentProcessService == null) throw new ArgumentNullException("currentProcessService");
			if (setupProvider == null) throw new ArgumentNullException("setupProvider");
			if (cameraProvider == null) throw new ArgumentNullException("cameraProvider");

			this.windowService = windowService;
			this.pluginSettings = pluginSettings;
			this.currentProcessService = currentProcessService;
			this.setupProvider = setupProvider;
			this.cameraProvider = cameraProvider;
		}


		/// <summary>
		/// Starts to configure the plugin.
		/// </summary>
		public void Start()
		{
			IEnumerable<ICameraNameViewModel> cameras = pluginSettings
				.GetCameras()
				.Select(camera => cameraProvider.Provide(camera));

			ISetupDialogViewModel setup = setupProvider.Provide(cameras);

			// Getting the window handle of the current process is a workaround since the owning window
			// is WinForms and we wish to open a WPF window
			windowService.ShowDialog<SetupDialog>(
				setup,
				currentProcessService.MainWindowHandle);

			// When the setup window has closed, save the cameras
			pluginSettings.SetCameras(setup.Cameras.Select(camera => camera.Camera));
		}
	}
}
