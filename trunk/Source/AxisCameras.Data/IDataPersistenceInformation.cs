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

namespace AxisCameras.Data
{
	/// <summary>
	/// Interface describing how the settings are persisted.
	/// </summary>
	public interface IDataPersistenceInformation
	{
		/// <summary>
		/// Gets the file name of settings.
		/// </summary>
		string FileName { get; }

		/// <summary>
		/// Gets the name of the section in the persisted file where the cameras are saved.
		/// </summary>
		string CameraSection { get; }


		/// <summary>
		/// Gets the name of the entry in the persisted file where the cameras are saved.
		/// </summary>
		string CamerasEntry { get; }
	}
}
