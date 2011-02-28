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
using AxisCameras.Configuration.ViewModel.Data;
using AxisCameras.Data;
using AxisCameras.Mvvm;

namespace AxisCameras.Configuration.ViewModel
{
	/// <summary>
	/// Class acting as view model when displaying a camera by its name.
	/// </summary>
	class CameraNameViewModel : ViewModelBase, ICameraNameViewModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CameraNameViewModel"/> class.
		/// </summary>
		/// <param name="camera">The camera.</param>
		public CameraNameViewModel(ConfigurableCamera camera)
		{
			if (camera == null) throw new ArgumentNullException("camera");

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
		/// Gets or sets the camera.
		/// </summary>
		public ConfigurableCamera Camera
		{
			get { return Property(() => Camera); }
			set
			{
				Property(() => Camera, value);
				OnPropertyChanged(() => Name);
			}
		}
	}
}
