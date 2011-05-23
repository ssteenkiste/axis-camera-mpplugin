﻿#region Copyright (C) 2005-2010 Team MediaPortal

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
using System.Diagnostics;
using System.Threading;
using AxisCameras.Core.Contracts;

namespace AxisCameras.Configuration.Service
{
	/// <summary>
	/// Class responsible for opening URLs in the default browser.
	/// </summary>
	class BrowserService : IBrowserService
	{
		/// <summary>
		/// Opens specified URL in default browser.
		/// </summary>
		/// <param name="url">The URL to open.</param>
		public void Open(string url)
		{
			Requires.IsNotNullOrEmpty(url);

			ThreadPool.QueueUserWorkItem(state => Process.Start(new ProcessStartInfo(url)));
		}
	}
}
