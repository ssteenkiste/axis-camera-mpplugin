using System;
using System.Diagnostics;
using System.IO;
using System.Security;
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

            try
            {
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(registryPath, false))
                {
                    if (registryKey != null)
                    {
                        // Get file path from CLSID
                        var filePath = registryKey.GetValue(null) as string;

                        if (filePath != null)
                        {
                            return FileVersionInfo.GetVersionInfo(filePath);    
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (SecurityException)
            {
            }
            catch (IOException)
            {
            }

            // CLSID is not registered or it wasn't possible to get version from file
            return null;
        }
    }
}