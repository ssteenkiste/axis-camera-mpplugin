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
using System.IO;
using System.Xml.Serialization;
using MediaPortal.Configuration;
using MediaPortal.Profile;

namespace AxisCameraMPPlugin.Data
{
	/// <summary>
	/// Class responsible for reading and saving settings.
	/// </summary>
	public class PluginSettings : IDisposable
	{
		private static readonly string FileName = "axiscamerampplugin.xml";

		private Settings settings;


		/// <summary>
		/// Initializes a new instance of the <see cref="PluginSettings"/> class.
		/// </summary>
		public PluginSettings()
		{
			settings = new Settings(Config.GetFile(Config.Dir.Config, FileName));
		}


		/// <summary>
		/// Gets the cameras.
		/// </summary>
		public IEnumerable<Camera> GetCameras()
		{
			string value = settings.GetValue(SettingNames.CameraSection, SettingNames.CamerasEntry);

			if (value != string.Empty)
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
		public void SetCameras(IEnumerable<Camera> cameras)
		{
			if (cameras == null) throw new ArgumentNullException("cameras");
			
			using (StringWriter writer = new StringWriter())
			{
				XmlSerializer serializer = new XmlSerializer(typeof(List<Camera>));
				serializer.Serialize(writer, cameras);

				settings.SetValue(SettingNames.CameraSection, SettingNames.CamerasEntry, writer.ToString());
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


		~PluginSettings()
		{
			Dispose(false);
		}


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
