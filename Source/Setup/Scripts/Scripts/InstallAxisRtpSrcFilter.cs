//css_reference "MpeCore.dll";

using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using MpeCore;
using MpeCore.Classes;

public class Script
{
	public static void Main(PackageClass packageClass, ActionItem actionItem)
	{
		FileVersionInfo axisRtpSourceFilterFileVersion = GetVersionFromClsid(AxisRtpSrcFilter.Clsid);

		if (axisRtpSourceFilterFileVersion != null)
		{
			// Axis RTP source filter is registered, determine if its version is same or newer
			Version registeredVersion = new Version(axisRtpSourceFilterFileVersion.ProductVersion);
			if (registeredVersion >= AxisRtpSrcFilter.ProductVersion)
			{
				// Registered version is same or newer, do nothing
				return;
			}
		}

		// Move source filter to correct location
		if (MoveAxisRtpSourceFilter())
		{
			// Register source filter
			RegisterAxisRtpSourceFilter();
		}
	}


	/// <summary>
	/// Moves the Axis RTP source filter from the temp directory to the correct Axis components
	/// folder.
	/// </summary>
	/// <returns>true if source filter was moved successfully; otherwise false.</returns>
	private static bool MoveAxisRtpSourceFilter()
	{
		string sourceFileName = Path.Combine(
			MpeInstaller.TransformInRealPath("%Temp%"),
			"AxisRTPSrcFilter.ax");

		string destDirectory = AxisComponentsFolder;

		string destFileName = Path.Combine(
			destDirectory,
			"AxisRTPSrcFilter.ax");

		try
		{
			if (!Directory.Exists(destDirectory))
			{
				// Create directory if it doesn't exist
				Directory.CreateDirectory(destDirectory);
			}
			else if (File.Exists(destFileName))
			{
				// Delete file if it already exists
				File.Delete(destFileName);
			}

			File.Move(sourceFileName, destFileName);

			return true;
		}
		catch
		{
			return false;
		}
	}


	/// <summary>
	/// Is registering the Axis RTP source filter.
	/// </summary>
	private static void RegisterAxisRtpSourceFilter()
	{
		string fileName = Path.Combine(
			AxisComponentsFolder,
			"AxisRTPSrcFilter.ax");

		ProcessStartInfo startInfo = new ProcessStartInfo
		{
			FileName = "regsvr32.exe",
			Arguments = "/s \"" + fileName + "\""
		};

		try
		{
			using (Process process = Process.Start(startInfo))
			{
				process.WaitForExit();
			}
		}
		catch
		{
		}
	}


	/// <summary>
	/// Gets the file version of registered assembly based on specified CLSID.
	/// </summary>
	/// <param name="clsid">The CLSID of the registered assembly.</param>
	/// <returns>
	/// The file version of the registered assembly if successful; otherwise null.
	/// </returns>
	private static FileVersionInfo GetVersionFromClsid(string clsid)
	{
		if (clsid == null) throw new ArgumentNullException("clsid");

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


	/// <summary>
	/// Gets the Axis components folder.
	/// </summary>
	private static string AxisComponentsFolder
	{
		get
		{
			return Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
				@"Axis Communications\Components");
		}
	}


	/// <summary>
	/// Class describing the properties of AxisRTPSrcFilter.
	/// </summary>
	static class AxisRtpSrcFilter
	{
		/// <summary>
		/// Gets the CLSID.
		/// </summary>
		public static string Clsid = "4F1D0C59-5ECC-4028-87F3-482191D2230F";


		/// <summary>
		/// Gets the version of the AxisRTPSrcFilter.
		/// </summary>
		public static Version ProductVersion = new Version(2, 9, 1, 0);
	}
}