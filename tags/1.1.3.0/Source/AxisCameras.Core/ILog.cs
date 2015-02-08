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

namespace AxisCameras.Core
{
    /// <summary>
    /// Interface describing a logger in the plugin.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Is logging with debug level.
        /// </summary>
        void Debug(string format, params object[] arg);

        /// <summary>
        /// Is logging with information level.
        /// </summary>
        void Info(string format, params object[] arg);

        /// <summary>
        /// Is logging with warning level.
        /// </summary>
        void Warn(string format, params object[] arg);

        /// <summary>
        /// Is logging with error level.
        /// </summary>
        void Error(string format, params object[] arg);

        /// <summary>
        /// Is logging a exception.
        /// </summary>
        void Error(Exception exception);

        /// <summary>
        /// Is logging a message and exception with error level.
        /// </summary>
        void Error(string message, Exception exception);
    }
}