#region Copyright (C) 2005-2010 Team MediaPortal

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
using System.Globalization;
using System.Net;
using System.Net.Configuration;
using System.Reflection;
using AxisCameraMPPlugin.Core;

namespace AxisCameraMPPlugin
{
	static class NetworkConfiguration
	{
		/// <summary>
		/// Disables the safe header parsing used in .NET 2.0. This basically does the same as the fix
		/// for .NET 1.1, but the internal structure of the framework have changed. It is the Vapix API
		/// that requires a specific format to be backward compatible with old versions. 
		/// </summary>
		internal static bool SetAllowUnsafeHeaderParsing20()
		{
			Log.Debug("Disabling safe header parsing");

			Assembly netAssembly = Assembly.GetAssembly(typeof(SettingsSection));
			if (netAssembly != null)
			{
				// Use the assembly in order to get the internal type for the internal class
				Type settingsType = netAssembly.GetType("System.Net.Configuration.SettingsSectionInternal");
				if (settingsType != null)
				{
					// Use the internal static property to get an instance of the internal settings class. If
					// the static instance isn't created already the property will create it for us
					object instance = settingsType.InvokeMember(
						"Section",
						BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic,
						null,
						null,
						new object[] { },
						CultureInfo.InvariantCulture);

					if (instance != null)
					{
						// Locate the private bool field that tells the framework is unsafe header parsing
						// should be allowed or not
						FieldInfo useUnsafeHeaderParsing = settingsType.GetField(
							"useUnsafeHeaderParsing",
							BindingFlags.NonPublic | BindingFlags.Instance);

						if (useUnsafeHeaderParsing != null)
						{
							useUnsafeHeaderParsing.SetValue(instance, true); //And that's it!
							return true;
						}
					}
				}
			}

			return false;
		}
	}
}
