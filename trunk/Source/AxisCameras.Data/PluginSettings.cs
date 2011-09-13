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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using AxisCameras.Core;
using AxisCameras.Core.Contracts;
using AxisCameras.Data.MediaPortal;

namespace AxisCameras.Data
{
	/// <summary>
	/// Class responsible for reading and saving settings.
	/// </summary>
	public class PluginSettings : IPluginSettings
	{
		private readonly IUpgradeData upgradeData;
		private ISettings settings;


		/// <summary>
		/// Initializes a new instance of the <see cref="PluginSettings"/> class.
		/// </summary>
		/// <param name="settings">The MediaPortal settings.</param>
		/// <param name="upgradeData">Responsible for upgrading the data.</param>
		public PluginSettings(ISettings settings, IUpgradeData upgradeData)
		{
			Requires.NotNull(settings);
			Requires.NotNull(upgradeData);

			this.settings = settings;
			this.upgradeData = upgradeData;
		}


		/// <summary>
		/// Gets or sets the cameras.
		/// </summary>
		public IEnumerable<Camera> Cameras
		{
			get { return GetCameras(); }
			set { SetCameras(value); }
		}


		/// <summary>
		/// Gets the cameras.
		/// </summary>
		private IEnumerable<Camera> GetCameras()
		{
			Log.Debug("Getting cameras from disk");

			// Upgrade data if required
			if (upgradeData.IsUpgradeRequired)
			{
				Upgrade();
			}

			string value = settings.GetValue(
				DataPersistenceInformation.CameraSection.Name,
				DataPersistenceInformation.CameraSection.CamerasEntry);

			if (!string.IsNullOrEmpty(value))
			{
				using (StringReader reader = new StringReader(value))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(List<Camera>));
					return (List<Camera>)serializer.Deserialize(reader);
				}
			}

			// No cameras have been saved before
			return new Camera[0];
		}


		/// <summary>
		/// Sets the cameras.
		/// </summary>
		/// <param name="cameras">The cameras.</param>
		private void SetCameras(IEnumerable<Camera> cameras)
		{
			Requires.NotNull(cameras);

			Log.Debug("Saving cameras to disk");

			using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(List<Camera>));
				serializer.Serialize(writer, cameras.ToList());

				settings.SetValue(
					DataPersistenceInformation.CameraSection.Name,
					DataPersistenceInformation.CameraSection.CamerasEntry,
					writer.ToString());
			}
		}


		/// <summary>
		/// Upgrades the data.
		/// </summary>
		private void Upgrade()
		{
			Log.Info("Upgrade of data is required");

			if (upgradeData.Upgrade())
			{
				Log.Info("Upgrade successfully");
			}
			else
			{
				Log.Error("Upgrade failed!");
			}
		}


		#region IDisposable Members

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting
		/// unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}


		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// PluginSettings is reclaimed by garbage collection.
		/// </summary>
		~PluginSettings()
		{
			Dispose(false);
		}


		/// <summary>
		/// Releases unmanaged and optionally managed resources.
		/// </summary>
		/// <param name="disposing">
		/// true to release both managed and unmanaged resources; false to release only unmanaged
		/// resources.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (settings != null)
				{
					settings.Dispose();
					settings = null;
				}
			}
		}

		#endregion
	}
}
