﻿#region Copyright (C) 2005-2015 Team MediaPortal

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

using System.Collections.Generic;

namespace AxisCameras.Configuration.Service
{
    /// <summary>
    /// Interface responsible for parsing a camera parameter response.
    /// </summary>
    internal interface IParameterParser
    {
        /// <summary>
        /// Parses the specified camera parameter response.
        /// </summary>
        /// <param name="parameters">The camera parameter response.</param>
        /// <returns>A dictionary with parameter name as key, and parameter value as value.</returns>
        IDictionary<string, string> Parse(string parameters);
    }
}