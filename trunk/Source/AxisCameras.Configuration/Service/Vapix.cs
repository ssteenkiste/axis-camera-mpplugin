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

namespace AxisCameras.Configuration.Service
{
	/// <summary>
	/// Class describing the Axis VAPIX:
	/// http://www.axis.com/techsup/cam_servers/dev/cam_http_api_index.php
	/// </summary>
	static class Vapix
	{
		/// <summary>
		/// Class containing the VAPIX CGI.
		/// </summary>
		public static class Cgi
		{
			/// <summary>
			/// CGI for listing parameters.
			/// </summary>
			public const string Parameter = "http://{0}:{1}/axis-cgi/admin/param.cgi?action=list&group={2}";


			/// <summary>
			/// CGI for requesting a snapshot.
			/// </summary>
			public const string Snapshot = "http://{0}:{1}/axis-cgi/jpg/image.cgi?camera={2}";
		}


		/// <summary>
		/// Class containing the camera parameters.
		/// </summary>
		public static class Parameters
		{
			/// <summary>
			/// UPnP friendly name parameter name.
			/// </summary>
			public const string FriendlyName = "Network.UPnP.FriendlyName";


			/// <summary>
			/// Firmware version parameter name.
			/// </summary>
			public const string FirmwareVersion = "Properties.Firmware.Version";


			/// <summary>
			/// Number of video sources parameter name.
			/// </summary>
			public const string NbrOfImageSources = "ImageSource.NbrOfSources";


			/// <summary>
			/// The supported video capabilities.
			/// </summary>
			public const string ImageFormats = "Properties.Image.Format";
		}


		/// <summary>
		/// Class containing the parameter values.
		/// </summary>
		public static class Values
		{
			/// <summary>
			/// M-JPEG parameter value.
			/// </summary>
			public const string Mjpeg = "mjpeg";


			/// <summary>
			/// M-JPEG parameter value.
			/// </summary>
			public const string Mpeg4 = "mpeg4";


			/// <summary>
			/// M-JPEG parameter value.
			/// </summary>
			public const string H264 = "h264";
		}
	}
}