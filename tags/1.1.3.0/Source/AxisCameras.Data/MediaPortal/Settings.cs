#region Copyright (C) 2005-2015 Team MediaPortal

// Copyright (C) 2005-2015 Team MediaPortal
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
using MediaPortalSettings = MediaPortal.Profile.Settings;

namespace AxisCameras.Data.MediaPortal
{
    /// <summary>
    /// Class wrapping the MediaPortal Settings file.
    /// </summary>
    internal class Settings : ISettings
    {
        private MediaPortalSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings()
        {
            settings = new MediaPortalSettings(DataPersistenceInformation.FileName);
        }

        /// <summary>
        /// Gets a value from specified section with specified entry name.
        /// </summary>
        /// <param name="section">The settings section name.</param>
        /// <param name="entry">The settings entry name.</param>
        /// <returns>The setting if existing; otherwise string.Empty.</returns>
        public string GetValue(string section, string entry)
        {
            return settings.GetValue(section, entry);
        }

        /// <summary>
        /// Gets a value as integer from specified section with specified entry name.
        /// </summary>
        /// <param name="section">The settings section name.</param>
        /// <param name="entry">The settings entry name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The setting if existing; otherwise the specified default value.</returns>
        public int GetValueAsInt(string section, string entry, int defaultValue)
        {
            return settings.GetValueAsInt(section, entry, defaultValue);
        }

        /// <summary>
        /// Sets a value to the specified section with specified entry name.
        /// </summary>
        /// <param name="section">The settings section name.</param>
        /// <param name="entry">The settings entry name.</param>
        /// <param name="value">The setting value.</param>
        public void SetValue(string section, string entry, object value)
        {
            settings.SetValue(section, entry, value);
        }

        /// <summary>
        /// Saves the settings to disk.
        /// </summary>
        public void Save()
        {
            MediaPortalSettings.SaveCache();
        }

        /// <summary>
        /// Clear the settings.
        /// </summary>
        public void Clear()
        {
            MediaPortalSettings.ClearCache();
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
        /// Settings is reclaimed by garbage collection.
        /// </summary>
        ~Settings()
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