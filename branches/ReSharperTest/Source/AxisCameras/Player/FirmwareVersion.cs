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
using System.Text.RegularExpressions;

namespace AxisCameras.Player
{
	/// <summary>
	/// Represents the version number of a camera firmware.
	/// </summary>
	class FirmwareVersion
	{
		private static readonly Regex FirmwareVersionRegex = new Regex(
			@"^(?<major>\d+)\.(?<minor>\d+)(\.(?<build>\d+))?(\.(?<revision>\d+))?");

		private readonly Version version;


		/// <summary>
		/// Initializes a new instance of the <see cref="FirmwareVersion"/> class.
		/// </summary>
		/// <param name="version">The firmware version.</param>
		public FirmwareVersion(string version)
		{
			if (version == null) throw new ArgumentNullException("version");

			Match match = FirmwareVersionRegex.Match(version);
			if (!match.Success)
			{
				throw new ArgumentException("Error when parsing firmware version.", "version");
			}

			this.version = new Version(
				GetGroupValue(match, "major"),
				GetGroupValue(match, "minor"),
				GetGroupValue(match, "build"),
				GetGroupValue(match, "revision"));
		}


		/// <summary>
		/// Tries to convert the string representation of a firmware version, and returns a value that
		/// indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="input">A string that contains a firmware version to convert.</param>
		/// <param name="result">
		/// When this method returns, contains the FirmwareVersion equivalent of the firmware version
		/// that is contained in input, if the conversion succeeded, or a FirmwareVersion object whose
		/// major and minor version numbers are 0 if the conversion failed.
		/// </param>
		/// <returns>true if the input parameter was converted successfully; otherwise false.</returns>
		public static bool TryParse(string input, out FirmwareVersion result)
		{
			try
			{
				result = new FirmwareVersion(input);
				return true;
			}
			catch (ArgumentNullException)
			{
				result = new FirmwareVersion("0.0");
				return false;
			}
			catch (ArgumentException)
			{
				result = new FirmwareVersion("0.0");
				return false;
			}
		}


		/// <summary>
		/// Gets the value of the major component of the firmware version.
		/// </summary>
		public int Major
		{
			get { return version.Major; }
		}


		/// <summary>
		/// Gets the value of the minor component of the firmware version.
		/// </summary>
		public int Minor
		{
			get { return version.Minor; }
		}


		/// <summary>
		/// Gets the value of the build component of the firmware version.
		/// </summary>
		public int Build
		{
			get { return version.Build; }
		}


		/// <summary>
		/// Gets the value of the revision component of the firmware version.
		/// </summary>
		public int Revision
		{
			get { return version.Revision; }
		}


		/// <summary>
		/// Gets the value of the specified group.
		/// </summary>
		/// <param name="match">The Regex match.</param>
		/// <param name="groupName">The group name.</param>
		/// <returns>The value of the specified group if successful; otherwise 0.</returns>
		private static int GetGroupValue(Match match, string groupName)
		{
			Group group = match.Groups[groupName];

			return group.Success ? int.Parse(group.Value, CultureInfo.InvariantCulture) : 0;
		}
	}
}
