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
		// Install Axis RTP source filter
		Install(
			AxisRtpSrcFilter.FileName,
			AxisRtpSrcFilter.Clsid,
			AxisRtpSrcFilter.ProductVersion);

		// Install embedded Axis RTP source filter
		Install(
			EmbeddedAxisRtpSrcFilter.FileName,
			EmbeddedAxisRtpSrcFilter.Clsid,
			EmbeddedAxisRtpSrcFilter.ProductVersion);
	}


	/// <summary>
	/// Installs the specified file with specified CLSID and version
	/// </summary>
	private static void Install(string fileName, string clsid, Version version)
	{
		FileVersionInfo fileVersionInfo = GetVersionFromClsid(clsid);

		if (fileVersionInfo != null)
		{
			// File is registered, determine if its version is the same or newer than the one supplied by
			// the installation
			Version registeredVersion = new Version(fileVersionInfo.ProductVersion);
			if (registeredVersion >= version)
			{
				return;
			}
		}

		// Move file
		string newFileName = Move(fileName);
		if (newFileName != null)
		{
			// Register file
			Register(newFileName);
		}
	}


	/// <summary>
	/// Moves the specified file from the temp directory to the correct Axis components folder.
	/// </summary>
	/// <returns>The path of the moved file if successfully; otherwise null.</returns>
	private static string Move(string fileName)
	{
		string sourceFileName = Path.Combine(
			MpeInstaller.TransformInRealPath("%Temp%"),
			fileName);

		string destDirectory = AxisComponentsFolder;

		string destFileName = Path.Combine(
			destDirectory,
			fileName);

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

			return destFileName;
		}
		catch
		{
			return null;
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
		/// Gets the file name.
		/// </summary>
		public static string FileName = "AxisRTPSrcFilter.ax";


		/// <summary>
		/// Gets the CLSID.
		/// </summary>
		public static readonly string Clsid = "4F1D0C59-5ECC-4028-87F3-482191D2230F";


		/// <summary>
		/// Gets the version of the AxisRTPSrcFilter.
		/// </summary>
		public static readonly Version ProductVersion = new Version(3, 0, 4, 2);
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


		/// <summary>
		/// Gets the version of the AxisRTPSrcFilter.
		/// </summary>
		public static readonly Version ProductVersion = new Version(3, 0, 4, 2);
	}
}