using System.Diagnostics;
using Microsoft.Win32;

namespace AxisCamerasDependencyChecker.Dependencies.Helpers
{
	/// <summary>
	/// Class finding a component based on CLSID.
	/// </summary>
	internal class Clsid
	{
		/// <summary>
		/// Gets the file version of registered assembly based on specified CLSID.
		/// </summary>
		/// <param name="clsid">The CLSID of the registered assembly.</param>
		/// <returns>
		/// The file version information of the registered assembly if successful; otherwise null.
		/// </returns>
		internal FileVersionInfo GetVersionFromClsid(string clsid)
		{
			string registryPath = @"SOFTWARE\Classes\CLSID\{" + clsid + @"}\InprocServer32";

			// Get file path from CLSID
			string filePath;

			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(registryPath, false))
				{
					filePath = registryKey.GetValue(null) as string;
				}
			}
			catch
			{
				// CLSID is not registered
				return null;
			}

			// Get version from file
			try
			{
				return FileVersionInfo.GetVersionInfo(filePath);
			}
			catch
			{
				// Unable to get version from file. Most probable answer is that the file doesn't exist
				return null;
			}
		}
	}
}