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
using MediaPortal.Configuration;

namespace AxisCameras.Data
{
	/// <summary>
	/// Class describing how the settings are persisted.
	/// </summary>
	internal static class DataPersistenceInformation
	{
		/// <summary>
		/// Gets or sets the file name where the data is persisted.
		/// </summary>
		internal static string FileName = Config.GetFile(Config.Dir.Config, "AxisCameras.xml");


		/// <summary>
		/// Class describing the camera section.
		/// </summary>
		internal static class CameraSection
		{
			/// <summary>
			/// Gets the name of the section.
			/// </summary>
			internal const string Name = "camera";


			/// <summary>
			/// Gets the name of the entry in the persisted file where the cameras are saved.
			/// </summary>
			internal const string CamerasEntry = "cameras";
		}


		/// <summary>
		/// Class describing the database section.
		/// </summary>
		internal static class DatabaseSection
		{
			/// <summary>
			/// Gets the name of the section.
			/// </summary>
			internal const string Name = "database";


			/// <summary>
			/// Gets the name of the entry in the persisted file where the database version is saved.
			/// </summary>
			internal const string VersionEntry = "version";
		}
	}
}
