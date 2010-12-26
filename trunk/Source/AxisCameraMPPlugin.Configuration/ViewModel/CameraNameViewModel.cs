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
using AxisCameraMPPlugin.Data;
using AxisCameraMPPlugin.Mvvm;

namespace AxisCameraMPPlugin.Configuration.ViewModel
{
	/// <summary>
	/// Class acting as view model when displaying a camera by its name.
	/// </summary>
	class CameraNameViewModel : ViewModelBase
	{
		private readonly Camera camera;


		/// <summary>
		/// Initializes a new instance of the <see cref="CameraNameViewModel"/> class.
		/// </summary>
		/// <param name="camera">The camera.</param>
		public CameraNameViewModel(Camera camera)
		{
			if (camera == null) throw new ArgumentNullException("camera");

			this.camera = camera;
		}


		/// <summary>
		/// Gets the name of the camera.
		/// </summary>
		public string Name
		{
			get { return camera.Name; }
		}
	}
}
