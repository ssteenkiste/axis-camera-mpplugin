#region Copyright (C) 2005-2014 Team MediaPortal

// Copyright (C) 2005-2014 Team MediaPortal
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
using System.Diagnostics;
using System.Globalization;
using AxisCameras.Core;

namespace AxisCameras.Configuration.Starter
{
    /// <summary>
    /// Logger logging to Trace for the sake of debugging purposes.
    /// </summary>
    internal class TraceLog : ILog
    {
        public void Debug(string format, params object[] arg)
        {
            Trace.WriteLine(Format(format, arg), "DEBUG");
        }

        public void Info(string format, params object[] arg)
        {
            Trace.WriteLine(Format(format, arg), "INFO");
        }

        public void Warn(string format, params object[] arg)
        {
            Trace.WriteLine(Format(format, arg), "WARN");
        }

        public void Error(string format, params object[] arg)
        {
            Trace.WriteLine(Format(format, arg), "ERROR");
        }

        public void Error(Exception exception)
        {
            Trace.WriteLine(exception, "ERROR");
        }

        public void Error(string message, Exception exception)
        {
            Trace.WriteLine(message + Environment.NewLine + exception, "ERROR");
        }

        private static string Format(string format, object[] arg)
        {
            return string.Format(CultureInfo.InvariantCulture, format, arg);
        }
    }
}