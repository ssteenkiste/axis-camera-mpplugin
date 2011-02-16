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

namespace AxisCameras.Configuration.Service
{
	/// <summary>
	/// Class describing the Axis VAPIX:
	/// http://www.axis.com/techsup/cam_servers/dev/cam_http_api_index.php
	/// </summary>
	static class Vapix
	{
		/// <summary>
		/// Gets CGI for listing parameters.
		/// </summary>
		public const string ParameterCgi = "http://{0}:{1}/axis-cgi/admin/param.cgi?action=list&group={2}";


		/// <summary>
		/// Gets CGI for requesting a snapshot.
		/// </summary>
		public const string SnapshotCgi = "http://{0}:{1}/axis-cgi/jpg/image.cgi";


		/// <summary>
		/// Gets UPnP friendly name parameter name.
		/// </summary>
		public const string FriendlyName = "Network.UPnP.FriendlyName";


		/// <summary>
		/// Gets firmware version parameter name.
		/// </summary>
		public const string FirmwareVersion = "Properties.Firmware.Version";
	}
}
