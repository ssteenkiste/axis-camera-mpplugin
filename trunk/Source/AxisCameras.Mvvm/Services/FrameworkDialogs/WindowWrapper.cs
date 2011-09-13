#region Copyright (C) 2005-2011 Team MediaPortal

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
using System.Windows;
using System.Windows.Forms;
using WindowInteropHelper = System.Windows.Interop.WindowInteropHelper;

namespace AxisCameras.Mvvm.Services.FrameworkDialogs
{
	/// <summary>
	/// WindowWrapper is an IWin32Window wrapper around a WPF window.
	/// </summary>
	internal class WindowWrapper : IWin32Window
	{
		/// <summary>
		/// Construct a new wrapper taking a WPF window.
		/// </summary>
		/// <param name="window">The WPF window to wrap.</param>
		public WindowWrapper(Window window)
		{
			Handle = new WindowInteropHelper(window).Handle;
		}


		/// <summary>
		/// Gets the handle to the window represented by the implementer.
		/// </summary>
		/// <returns>A handle to the window represented by the implementer.</returns>
		public IntPtr Handle { get; private set; }
	}
}
