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
using System.Linq;
using AxisCameras.Configuration.Service;
using AxisCameras.Configuration.ViewModel.Data;
using AxisCameras.Core;
using AxisCameras.Core.Contracts;
using AxisCameras.Data;

namespace AxisCameras.Configuration.Provider
{
	/// <summary>
	/// Class describing a converter that converts between a Camera and a ConfigurableCamera.
	/// </summary>
	class CameraConverter : ICameraConverter
	{
		private readonly IIOService ioService;


		/// <summary>
		/// Initializes a new instance of the <see cref="CameraConverter"/> class.
		/// </summary>
		/// <param name="ioService">The I/O service.</param>
		public CameraConverter(IIOService ioService)
		{
			Requires.NotNull(ioService);

			this.ioService = ioService;
		}


		/// <summary>
		/// Returns a ConfigurableCamera created from a Camera.
		/// </summary>
		/// <param name="camera">The camera to turn into a ConfigurableCamera.</param>
		public ConfigurableCamera ToConfigurableCamera(Camera camera)
		{
			Requires.NotNull(camera);

			Log.Debug("Convert a Camera to a ConfigurableCamera");

			return new ConfigurableCamera(camera.Id)
			{
				Name = camera.Name,
				Address = camera.Address,
				Port = camera.Port,
				VideoSource = camera.VideoSource,
				VideoSourceCount = camera.VideoSourceCount,
				VideoCapabilities = camera.VideoCapabilities,
				UserName = camera.UserName,
				Password = camera.Password,
				FirmwareVersion = camera.FirmwareVersion,
				Snapshot = ioService.GetThumb(camera.Id)
			};
		}


		/// <summary>
		/// Returns a Camera created from a ConfigurableCamera.
		/// </summary>
		/// <param name="configurableCamera">The configurable camera to turn into a Camera.</param>
		public Camera ToCamera(ConfigurableCamera configurableCamera)
		{
			Requires.NotNull(configurableCamera);

			Log.Debug("Convert a ConfigurableCamera to a Camera");

			// Save thumb to disk
			ioService.SaveThumb(configurableCamera.Id, configurableCamera.Snapshot.ToArray());

			return new Camera
			{
				Id = configurableCamera.Id,
				Name = configurableCamera.Name,
				Address = configurableCamera.Address,
				Port = configurableCamera.Port,
				VideoSource = configurableCamera.VideoSource,
				VideoSourceCount = configurableCamera.VideoSourceCount,
				VideoCapabilities = configurableCamera.VideoCapabilities,
				UserName = configurableCamera.UserName,
				Password = configurableCamera.Password,
				FirmwareVersion = configurableCamera.FirmwareVersion
			};
		}
	}
}