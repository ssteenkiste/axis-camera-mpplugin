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
using AxisCameraMPPlugin.Configuration.View;
using AxisCameraMPPlugin.Configuration.ViewModel;
using AxisCameraMPPlugin.Mvvm.Services;

namespace AxisCameraMPPlugin.Configuration
{
	/// <summary>
	/// Class acting as entry point for configuring the plugin.
	/// </summary>
	public class ConfigurationStarter : IConfigurationStarter
	{
		private readonly IWindowService windowService;
		private readonly Func<ISetupDialogViewModel> setupDialogViewModelProvider;


		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationStarter"/> class.
		/// </summary>
		/// <param name="windowService">The window service.</param>
		/// <param name="setupDialogViewModelProvider">The ISetupDialogViewModel provider.</param>
		public ConfigurationStarter(
			IWindowService windowService,
			Func<ISetupDialogViewModel> setupDialogViewModelProvider)
		{
			if (windowService == null) throw new ArgumentNullException("windowService");
			if (setupDialogViewModelProvider == null) throw new ArgumentNullException("setupDialogViewModelProvider");

			this.windowService = windowService;
			this.setupDialogViewModelProvider = setupDialogViewModelProvider;
		}


		/// <summary>
		/// Starts to configure the plugin.
		/// </summary>
		public void Start()
		{
			ISetupDialogViewModel setupDialogViewModel = setupDialogViewModelProvider();
			windowService.ShowDialog<SetupDialog>(setupDialogViewModel, null);
		}
	}
}
