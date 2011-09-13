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
		// Install if needed the Axis RTP source filter
		InstallIfNeeded(
			AxisRtpSrcFilter.FileName,
			AxisRtpSrcFilter.Clsid);

		// Install if needed the embedded Axis RTP source filter
		InstallIfNeeded(
			EmbeddedAxisRtpSrcFilter.FileName,
			EmbeddedAxisRtpSrcFilter.Clsid);
	}


	/// <summary>
	/// Installs if needed the specified filter with specified CLSID.
	/// </summary>
	private static void InstallIfNeeded(string filterName, string clsid)
	{
		Version installedVersion = GetVersionFromClsid(clsid);
		string bundledFilterFileName = GetBundledFilterFileName(filterName);

		if (installedVersion == null)
		{
			// Filter is not installed, install it
			Install(bundledFilterFileName);
		}
		else
		{
			// Filter is installed, determine if it needs upgrading
			Version bundledVersion = GetVersionFromFile(bundledFilterFileName);
			if (bundledVersion != null && bundledVersion > installedVersion)
			{
				Install(bundledFilterFileName);
			}
		}
	}


	/// <summary>
	/// Installs the filter specified by its file name.
	/// </summary>
	private static void Install(string fileName)
	{
		string destFileName = Path.Combine(
			AxisComponentsFolder,
			Path.GetFileName(fileName));

		// Move filter to correct location
		if (Move(fileName, destFileName))
		{
			// Register filter
			Register(destFileName);
		}
	}


	/// <summary>
	/// Moves the specified file.
	/// </summary>
	/// <param name="sourceFileName">The source file name.</param>
	/// <param name="destFileName">The destination file name.</param>
	/// <returns>true if source file was moved successfully; otherwise false.</returns>
	private static bool Move(string sourceFileName, string destFileName)
	{
		try
		{
			string destDirectory = Path.GetDirectoryName(destFileName);

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
	/// Is registering the specified file.
	/// </summary>
	private static void Register(string fileName)
	{
		ProcessStartInfo startInfo = new ProcessStartInfo();
		startInfo.FileName = "regsvr32.exe";
		startInfo.Arguments = "/s \"" + fileName + "\"";

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
	/// Gets the version of registered assembly based on specified CLSID.
	/// </summary>
	/// <param name="clsid">The CLSID of the registered assembly.</param>
	/// <returns>
	/// The version of the registered assembly if successful; otherwise null.
	/// </returns>
	private static Version GetVersionFromClsid(string clsid)
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
		return GetVersionFromFile(filePath);
	}


	/// <summary>
	/// Gets the version from a file on disk.
	/// </summary>
	/// <returns>The version of the file if existing; otherwise null.</returns>
	private static Version GetVersionFromFile(string fileName)
	{
		if (fileName == null) throw new ArgumentNullException("fileName");

		try
		{
			return new Version(FileVersionInfo.GetVersionInfo(fileName).ProductVersion);
		}
		catch
		{
			// Unable to get version from file. Most probable answer is that the file doesn't exist
			return null;
		}
	}


	/// <summary>
	/// Gets the file name of the filter in the TEMP directory.
	/// </summary>
	private static string GetBundledFilterFileName(string filterName)
	{
		if (filterName == null) throw new ArgumentNullException("filterName");

		return Path.Combine(
			MpeInstaller.TransformInRealPath("%Temp%"),
			filterName);
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
		/// Gets the file name.
		/// </summary>
		public static string FileName = "AxisRTPSrcFilter.ax";


		/// <summary>
		/// Gets the CLSID.
		/// </summary>
		public static readonly string Clsid = "4F1D0C59-5ECC-4028-87F3-482191D2230F";
	}


	/// <summary>
	/// Class describing the properties of the embedded version of AxisRTPSrcFilter.
	/// </summary>
	static class EmbeddedAxisRtpSrcFilter
	{
		/// <summary>
		/// Gets the file name.
		/// </summary>
		public static string FileName = "AxisRTPSrcFilterEmb.ax";


		/// <summary>
		/// Gets the CLSID.
		/// </summary>
		public static readonly string Clsid = "67B1A88A-B5D2-48B1-BF93-EB74D6FCB077";
	}
}