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
using System.Collections.Generic;
using System.Windows.Input;
using AxisCameras.Configuration.ViewModel.Data;
using AxisCameras.Mvvm;

namespace AxisCameras.Configuration.ViewModel
{
	/// <summary>
	/// Interface for CameraViewModel.
	/// </summary>
	public interface ICameraViewModel : IViewModelBase
	{
		/// <summary>
		/// Gets the name of the camera.
		/// </summary>
		string Name { get; }


		/// <summary>
		/// Gets the snapshot.
		/// </summary>
		IEnumerable<byte> Snapshot { get; }


		/// <summary>
		/// Gets the browse command.
		/// </summary>
		ICommand BrowseCommand { get; }


		/// <summary>
		/// Gets the camera.
		/// </summary>
		ConfigurableCamera Camera { get; }
	}
}
