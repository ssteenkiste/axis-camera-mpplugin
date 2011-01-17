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
using System.Linq;
using AxisCameraMPPlugin.Configuration.Service;
using AxisCameraMPPlugin.Configuration.ViewModel.Data;
using AxisCameraMPPlugin.Core;
using AxisCameraMPPlugin.Data;

namespace AxisCameraMPPlugin.Configuration.Provider
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
			if (ioService == null) throw new ArgumentNullException("ioService");

			this.ioService = ioService;
		}


		/// <summary>
		/// Returns a ConfigurableCamera created from a Camera.
		/// </summary>
		/// <param name="camera">The camera to turn into a ConfigurableCamera.</param>
		public ConfigurableCamera ToConfigurableCamera(Camera camera)
		{
			if (camera == null) throw new ArgumentNullException("camera");

			Log.Debug("Convert a Camera to a ConfigurableCamera");

			return new ConfigurableCamera(camera.Id)
			{
				Name = camera.Name,
				Address = camera.Address,
				Port = camera.Port,
				UserName = camera.UserName,
				Password = camera.Password,
				Snapshot = ioService.GetThumb(camera.Id)
			};
		}


		/// <summary>
		/// Returns a Camera created from a ConfigurableCamera.
		/// </summary>
		/// <param name="configurableCamera">The configurable camera to turn into a Camera.</param>
		public Camera ToCamera(ConfigurableCamera configurableCamera)
		{
			if (configurableCamera == null) throw new ArgumentNullException("configurableCamera");

			Log.Debug("Convert a ConfigurableCamera to a Camera");

			return new Camera
			{
				Id = configurableCamera.Id,
				Name = configurableCamera.Name,
				Address = configurableCamera.Address,
				Port = configurableCamera.Port,
				UserName = configurableCamera.UserName,
				Password = configurableCamera.Password,
				SnapshotPath = ioService.SaveThumb(configurableCamera.Id, configurableCamera.Snapshot.ToArray())
			};
		}
	}
}
