﻿#region Copyright (C) 2005-2011 Team MediaPortal

// Copyright (C) 2005-2011 Team MediaPortal
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
using AxisCameras.Data;
using AxisCameras.Mvvm;

namespace AxisCameras.Configuration.ViewModel
{
    /// <summary>
    /// Interface responsible for reading parameters from a camera.
    /// </summary>
    internal interface ICameraParametersDialogViewModel : IDialogViewModelBase, IDisposable
    {
        /// <summary>
        /// Gets the friendly name.
        /// </summary>
        string FriendlyName { get; }

        /// <summary>
        /// Gets the firmware version.
        /// </summary>
        string FirmwareVersion { get; }

        /// <summary>
        /// Gets the video source count, i.e. the number of video source this camera (device actually)
        /// has.
        /// </summary>
        int VideoSourceCount { get; }

        /// <summary>
        /// Gets the video capabilities.
        /// </summary>
        VideoCapabilities VideoCapabilities { get; }
    }
}