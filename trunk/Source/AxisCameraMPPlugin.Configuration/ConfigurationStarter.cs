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
using System.Diagnostics;
using System.Linq;
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
		private readonly Func<ISetupDialogViewModel> setupProvider;
		private readonly Func<IPluginSettings> pluginSettingsProvider;


		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationStarter"/> class.
		/// </summary>
		/// <param name="windowService">The window service.</param>
		/// <param name="setupProvider">The setup view model provider.</param>
		/// <param name="pluginSettingsProvider">The plugin settings provider.</param>
		public ConfigurationStarter(
			IWindowService windowService,
			Func<ISetupDialogViewModel> setupProvider,
			Func<IPluginSettings> pluginSettingsProvider)
		{
			if (windowService == null) throw new ArgumentNullException("windowService");
			if (setupProvider == null) throw new ArgumentNullException("setupProvider");
			if (pluginSettingsProvider == null) throw new ArgumentNullException("pluginSettingsProvider");

			this.windowService = windowService;
			this.setupProvider = setupProvider;
			this.pluginSettingsProvider = pluginSettingsProvider;
		}


		/// <summary>
		/// Starts to configure the plugin.
		/// </summary>
		public void Start()
		{
			ISetupDialogViewModel setup = setupProvider();

			// This is a workaround since the owning window is WinForms and we wish to open a WPF window
			windowService.ShowDialog<SetupDialog>(
				setup,
				Process.GetCurrentProcess().MainWindowHandle);
			
			// When the setup window has closed, save the cameras
			using (IPluginSettings pluginSettings = pluginSettingsProvider())
			{
				pluginSettings.SetCameras(setup.Cameras.Select(camera => camera.Camera));
			}
		}
	}
}
