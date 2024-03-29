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

using AxisCameras.Configuration.ViewModel.Data;
using AxisCameras.Data;

namespace AxisCameras.Configuration.Provider
{
    /// <summary>
    /// Interface describing a converter that converts between a <see cref="Camera"/> and a
    /// <see cref="ConfigurableCamera"/>.
    /// </summary>
    public interface ICameraConverter
    {
        /// <summary>
        /// Returns a <see cref="ConfigurableCamera"/> created from a <see cref="Camera"/>.
        /// </summary>
        /// <param name="camera">
        /// The <see cref="Camera"/> to turn into a <see cref="ConfigurableCamera"/>.
        /// </param>
        ConfigurableCamera ToConfigurableCamera(Camera camera);

        /// <summary>
        /// Returns a <see cref="Camera"/> created from a <see cref="ConfigurableCamera"/>.
        /// </summary>
        /// <param name="configurableCamera">
        /// The <see cref="ConfigurableCamera"/> camera to turn into a <see cref="Camera"/>.
        /// </param>
        Camera ToCamera(ConfigurableCamera configurableCamera);
    }
}