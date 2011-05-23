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
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using AxisCameras.Core.Contracts;
using AxisCameras.Data.MediaPortal;

namespace AxisCameras.Data.Upgrades
{
	/// <summary>
	/// Base class for all partial upgrades.
	/// </summary>
	abstract class PartialUpgrade : IPartialUpgrade
	{
		private readonly ISettings settings;


		/// <summary>
		/// Initializes a new instance of the <see cref="PartialUpgrade"/> class.
		/// </summary>
		/// <param name="settings">The MediaPortal settings.</param>
		protected PartialUpgrade(ISettings settings)
		{
			Requires.NotNull(settings);

			this.settings = settings;
		}


		/// <summary>
		/// Gets the version before the upgrade.
		/// </summary>
		/// <remarks>Can be overridden if FromVersion != ToVersion - 1.</remarks>
		public virtual int FromVersion
		{
			get { return ToVersion - 1; }
		}


		/// <summary>
		/// Gets the version after the upgrade.
		/// </summary>
		public abstract int ToVersion { get; }


		/// <summary>
		/// Upgrades the data.
		/// </summary>
		/// <returns>true if upgrade was successful; otherwise false.</returns>
		public abstract bool Upgrade();


		/// <summary>
		/// Gets a value from specified section with specified entry name.
		/// </summary>
		/// <param name="section">The settings section name.</param>
		/// <param name="entry">The settings entry name.</param>
		/// <returns>The setting if existing; otherwise string.Empty.</returns>
		protected string GetValue(string section, string entry)
		{
			return settings.GetValue(section, entry);
		}


		/// <summary>
		/// Sets a value to the specified section with specified entry name.
		/// </summary>
		/// <param name="section">The settings section name.</param>
		/// <param name="entry">The settings entry name.</param>
		/// <param name="value">The setting value.</param>
		protected void SetValue(string section, string entry, object value)
		{
			settings.SetValue(section, entry, value);
		}


		/// <summary>
		/// Serializes the specified value.
		/// </summary>
		/// <typeparam name="T">The type of the object that this method should serialize.</typeparam>
		/// <param name="value">The value.</param>
		/// <returns>The serialized value.</returns>
		protected static string Serialize<T>(T value)
		{
			using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				serializer.Serialize(writer, value);

				return writer.ToString();
			}
		}


		/// <summary>
		/// Deserializes the specified value.
		/// </summary>
		/// <typeparam name="T">The type of the object that this method should deserialize.</typeparam>
		/// <param name="value">The value.</param>
		/// <returns>The deserialized object.</returns>
		protected static T Deserialize<T>(string value)
		{
			using (StringReader reader = new StringReader(value))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				return (T)serializer.Deserialize(reader);
			}
		}
	}
}
