using System;
using System.Diagnostics;

namespace AxisCameras.Configuration.Service
{
	/// <summary>
	/// Class providing information about the current process.
	/// </summary>
	class CurrentProcessService : ICurrentProcessService
	{
		/// <summary>
		/// Gets the window handle of the main window of the associated process.
		/// </summary>
		public IntPtr MainWindowHandle
		{
			get { return Process.GetCurrentProcess().MainWindowHandle; }
		}
	}
}
