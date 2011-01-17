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
using MediaPortalLog = MediaPortal.GUI.Library.Log;

namespace AxisCameraMPPlugin.Core
{
	/// <summary>
	/// Class acting as logger in the plugin.
	/// </summary>
	public static class Log
	{
		/// <summary>
		/// Is logging with debug level.
		/// </summary>
		public static void Debug(string format, params object[] arg)
		{
			MediaPortalLog.Debug(format, arg);
		}


		/// <summary>
		/// Is logging with information level.
		/// </summary>
		public static void Info(string format, params object[] arg)
		{
			MediaPortalLog.Info(format, arg);
		}


		/// <summary>
		/// Is logging with warning level.
		/// </summary>
		public static void Warn(string format, params object[] arg)
		{
			MediaPortalLog.Warn(format, arg);
		}


		/// <summary>
		/// Is logging with error level.
		/// </summary>
		public static void Error(string format, params object[] arg)
		{
			MediaPortalLog.Error(format, arg);
		}


		/// <summary>
		/// Is logging a exception.
		/// </summary>
		public static void Error(Exception exception)
		{
			MediaPortalLog.Error(exception);
		}
	}
}
