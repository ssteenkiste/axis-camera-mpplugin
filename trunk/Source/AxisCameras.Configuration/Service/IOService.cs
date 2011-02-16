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
using System.IO;
using AxisCameras.Core;
using MediaPortal.Configuration;

namespace AxisCameras.Configuration.Service
{
	/// <summary>
	/// Class responsible for I/O related operations.
	/// </summary>
	class IOService : IIOService
	{
		private const string PluginFolderName = "AxisCameras";


		/// <summary>
		/// Gets the path of the camera icon.
		/// </summary>
		/// <returns>The path of the camera icon.</returns>
		public string CameraIconPath
		{
			get { return Config.GetFile(Config.Dir.Thumbs, PluginFolderName, "CameraPortrait.png"); }
		}


		/// <summary>
		/// Gets the thumb for specified camera.
		/// </summary>
		/// <param name="cameraId">The camera id.</param>
		/// <returns>The thumb.</returns>
		public byte[] GetThumb(Guid cameraId)
		{
			if (cameraId == null) throw new ArgumentNullException("id");
			if (cameraId == Guid.Empty) throw new ArgumentException("ID cannot be Guid.Empty");

			Log.Debug("Get thumb from camera with id {0}", cameraId);

			return File.ReadAllBytes(GetThumbFileName(cameraId));
		}


		/// <summary>
		/// Saves a thumb for specified camera.
		/// </summary>
		/// <param name="cameraId">The camera id.</param>
		/// <param name="image">The image thumb.</param>
		/// <returns>The path where the thumb is saved.</returns>
		public string SaveThumb(Guid cameraId, byte[] image)
		{
			Log.Debug("Save thumb of camera with id {0}", cameraId);

			string thumbFileName = GetThumbFileName(cameraId);

			// Make sure thumb directory exists
			string thumbDirectory = Path.GetDirectoryName(thumbFileName);
			if (!Directory.Exists(thumbDirectory))
			{
				Directory.CreateDirectory(thumbDirectory);
			}

			File.WriteAllBytes(thumbFileName, image);

			return thumbFileName;
		}


		/// <summary>
		/// Deletes the thumb belonging to specified camera.
		/// </summary>
		/// <param name="cameraId">The camera id.</param>
		public void DeleteThumb(Guid cameraId)
		{
			Log.Debug("Delete thumb of camera with id {0}", cameraId);

			File.Delete(GetThumbFileName(cameraId));
		}


		/// <summary>
		/// Gets the file name of the thumb based on specified camera id.
		/// </summary>
		/// <param name="cameraId">The camera id.</param>
		/// <returns>The file name of the thumb.</returns>
		private static string GetThumbFileName(Guid cameraId)
		{
			return Config.GetFile(
				Config.Dir.Thumbs,
				PluginFolderName,
				cameraId + ".jpg");
		}
	}
}
