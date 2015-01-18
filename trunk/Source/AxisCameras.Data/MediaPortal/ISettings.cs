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
using System.Diagnostics.CodeAnalysis;

namespace AxisCameras.Data.MediaPortal
{
    /// <summary>
    /// Interface wrapping the MediaPortal Settings file.
    /// </summary>
    public interface ISettings : IDisposable
    {
        /// <summary>
        /// Gets a value from specified section with specified entry name.
        /// </summary>
        /// <param name="section">The settings section name.</param>
        /// <param name="entry">The settings entry name.</param>
        /// <returns>The setting if existing; otherwise string.Empty.</returns>
        string GetValue(string section, string entry);

        /// <summary>
        /// Gets a value as integer from specified section with specified entry name.
        /// </summary>
        /// <param name="section">The settings section name.</param>
        /// <param name="entry">The settings entry name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The setting if existing; otherwise the specified default value.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "int",
            Justification = "MediaPortal has named the method, this is just a wrapper")]
        int GetValueAsInt(string section, string entry, int defaultValue);

        /// <summary>
        /// Sets a value to the specified section with specified entry name.
        /// </summary>
        /// <param name="section">The settings section name.</param>
        /// <param name="entry">The settings entry name.</param>
        /// <param name="value">The setting value.</param>
        void SetValue(string section, string entry, object value);

        /// <summary>
        /// Saves the settings to disk.
        /// </summary>
        void Save();

        /// <summary>
        /// Clear the settings.
        /// </summary>
        void Clear();
    }
}