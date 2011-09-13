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
using AxisCameras.Core.Contracts;
using MediaPortal.Configuration;

namespace AxisCameras.Configuration.Service
{
	/// <summary>
	/// Class responsible for I/O related operations.
	/// </summary>
	class IOService : IIOService
	{
		private const string PluginFolderName = "AxisCameras";
		private const string CameraIcon = "CameraPortrait.png";
		private const string DefaultThumb = "DefaultSnapshot.png";


		/// <summary>
		/// Gets the file name of the camera icon.
		/// </summary>
		/// <returns>The file name of the camera icon.</returns>
		public string CameraIconFileName
		{
			get { return Config.GetFile(Config.Dir.Thumbs, PluginFolderName, CameraIcon); }
		}


		/// <summary>
		/// Gets the file name of the default thumb.
		/// </summary>
		/// <returns>The file name of the default thumb.</returns>
		public string DefaultThumbFileName
		{
			get { return Config.GetFile(Config.Dir.Thumbs, PluginFolderName, DefaultThumb); }
		}


		/// <summary>
		/// Gets the file name of the thumb for specified camera.
		/// </summary>
		/// <param name="cameraId">The camera id.</param>
		/// <returns>
		/// The file name of the thumb if existing on disk, otherwise the default thumb.
		/// </returns>
		public string GetThumbFileName(Guid cameraId)
		{
			Requires.True(cameraId != Guid.Empty, "ID cannot be Guid.Empty");

			Log.Debug("Get file name of thumb from camera with id {0}", cameraId);

			string fileName = CreateThumbFileNameFrom(cameraId);

			if (File.Exists(fileName))
			{
				return fileName;
			}

			Log.Warn(
				"Snapshot of camera with ID {0} has been removed from disk, returning default.",
				cameraId);

			return DefaultThumbFileName;
		}


		/// <summary>
		/// Gets the default thumb.
		/// </summary>
		/// <returns>The default thumb.</returns>
		public byte[] GetDefaultThumb()
		{
			return File.ReadAllBytes(DefaultThumbFileName);
		}


		/// <summary>
		/// Gets the thumb for specified camera.
		/// </summary>
		/// <param name="cameraId">The camera id.</param>
		/// <returns>The thumb if existing; otherwise the default thumb.</returns>
		public byte[] GetThumb(Guid cameraId)
		{
			Requires.True(cameraId != Guid.Empty, "ID cannot be Guid.Empty");

			Log.Debug("Get thumb from camera with id {0}", cameraId);

			string thumbFileName = GetThumbFileName(cameraId);

			return File.ReadAllBytes(thumbFileName);
		}


		/// <summary>
		/// Saves a thumb for specified camera.
		/// </summary>
		/// <param name="cameraId">The camera id.</param>
		/// <param name="image">The image thumb.</param>
		/// <returns>The file name where the thumb is saved.</returns>
		public string SaveThumb(Guid cameraId, byte[] image)
		{
			Requires.True(cameraId != Guid.Empty, "ID cannot be Guid.Empty");
			Requires.NotNull(image);

			Log.Debug("Save thumb of camera with id {0}", cameraId);

			string thumbFileName = CreateThumbFileNameFrom(cameraId);

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
			Requires.True(cameraId != Guid.Empty, "ID cannot be Guid.Empty");

			Log.Debug("Delete thumb of camera with id {0}", cameraId);

			File.Delete(CreateThumbFileNameFrom(cameraId));
		}


		/// <summary>
		/// Creates the thumb file name from specified camera.
		/// </summary>
		/// <param name="cameraId">The camera id.</param>
		private static string CreateThumbFileNameFrom(Guid cameraId)
		{
			return Config.GetFile(
				Config.Dir.Thumbs,
				PluginFolderName,
				cameraId + ".jpg");
		}
	}
}
