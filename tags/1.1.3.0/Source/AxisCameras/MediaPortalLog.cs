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
using AxisCameras.Core;

namespace AxisCameras
{
    /// <summary>
    /// Logger logging to MediaPortal's log.
    /// </summary>
    internal class MediaPortalLog : ILog
    {
        private const string PluginName = "Axis Cameras";

        /// <summary>
        /// Is logging with debug level.
        /// </summary>
        public void Debug(string format, params object[] arg)
        {
            MediaPortal.GUI.Library.Log.Debug(Prefix(format), arg);
        }

        /// <summary>
        /// Is logging with information level.
        /// </summary>
        public void Info(string format, params object[] arg)
        {
            MediaPortal.GUI.Library.Log.Info(Prefix(format), arg);
        }

        /// <summary>
        /// Is logging with warning level.
        /// </summary>
        public void Warn(string format, params object[] arg)
        {
            MediaPortal.GUI.Library.Log.Warn(Prefix(format), arg);
        }

        /// <summary>
        /// Is logging with error level.
        /// </summary>
        public void Error(string format, params object[] arg)
        {
            MediaPortal.GUI.Library.Log.Error(Prefix(format), arg);
        }

        /// <summary>
        /// Is logging a exception.
        /// </summary>
        public void Error(Exception exception)
        {
            MediaPortal.GUI.Library.Log.Error(exception);
        }

        /// <summary>
        /// Is logging a message and exception with error level.
        /// </summary>
        public void Error(string message, Exception exception)
        {
            MediaPortal.GUI.Library.Log.Error(Prefix(message) + " " + exception);
        }

        /// <summary>
        /// Adds the plugin name as prefix to the format text.
        /// </summary>
        /// <param name="format">The format.</param>
        private static string Prefix(string format)
        {
            return PluginName + ": " + format;
        }
    }
}