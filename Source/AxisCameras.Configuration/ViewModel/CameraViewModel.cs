﻿#region Copyright (C) 2005-2010 Team MediaPortal

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
using System.Globalization;
using System.Windows.Input;
using AxisCameras.Configuration.Service;
using AxisCameras.Configuration.ViewModel.Data;
using AxisCameras.Core.Contracts;
using AxisCameras.Data;
using AxisCameras.Mvvm;

namespace AxisCameras.Configuration.ViewModel
{
	/// <summary>
	/// Class acting as view model when displaying a camera by its name.
	/// </summary>
	class CameraViewModel : ViewModelBase, ICameraViewModel
	{
		private const string CameraUrl = "http://{0}:{1}";

		private readonly IBrowserService browserService;


		/// <summary>
		/// Initializes a new instance of the <see cref="CameraViewModel"/> class.
		/// </summary>
		/// <param name="camera">The camera.</param>
		/// <param name="browserService">The browser service.</param>
		public CameraViewModel(ConfigurableCamera camera, IBrowserService browserService)
		{
			Requires.NotNull(camera);
			Requires.NotNull(browserService);

			this.browserService = browserService;

			BrowseCommand = new RelayCommand(Browse);
			Camera = camera;
		}


		/// <summary>
		/// Gets the name of the camera.
		/// </summary>
		public string Name
		{
			get { return Camera.Name; }
		}


		/// <summary>
		/// Gets the snapshot.
		/// </summary>
		public IEnumerable<byte> Snapshot
		{
			get { return Camera.Snapshot; }
		}


		/// <summary>
		/// Gets the browse command.
		/// </summary>
		public ICommand BrowseCommand
		{
			get { return Property(() => BrowseCommand); }
			private set { Property(() => BrowseCommand, value); }
		}


		/// <summary>
		/// Gets or sets the camera.
		/// </summary>
		public ConfigurableCamera Camera
		{
			get { return Property(() => Camera); }
			set
			{
				Property(() => Camera, value);
				OnAllPropertiesChanged();
			}
		}


		/// <summary>
		/// Is opening the camera's web page in default browser.
		/// </summary>
		private void Browse(object parameter)
		{
			string url = CameraUrl.InvariantFormat(
				Camera.Address,
				Camera.Port.ToString(CultureInfo.InvariantCulture));

			browserService.Open(url);
		}
	}
}
