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
using System.ComponentModel;

namespace AxisCameraMPPlugin.Configuration.Service
{
	/// <summary>
	/// Event argument sent when getting information from a camera completed.
	/// </summary>
	class GetInformationFromCameraCompletedEventArgs : AsyncCompletedEventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GetInformationFromCameraCompletedEventArgs"/>
		/// class.
		/// </summary>
		/// <param name="friendlyName">The friendly name.</param>
		/// <param name="snapshot">The camera snapshot.</param>
		/// <param name="error">
		/// Any error that occurred during the asynchronous operation. Default value is null.
		/// </param>
		/// <param name="cancelled">
		/// A value indicating whether the asynchronous operation was canceled. Default value is false.
		/// </param>
		public GetInformationFromCameraCompletedEventArgs(
			string friendlyName,
			byte[] snapshot,
			Exception error = null,
			bool cancelled = false) :
			base(error, cancelled, null)
		{
			if (friendlyName == null) throw new ArgumentNullException("friendlyName");
			if (snapshot == null) throw new ArgumentNullException("snapshot");

			FriendlyName = friendlyName;
			Snapshot = snapshot;
		}


		/// <summary>
		/// Gets the friendly name.
		/// </summary>
		public string FriendlyName { get; private set; }


		/// <summary>
		/// Gets the snapshot.
		/// </summary>
		public byte[] Snapshot { get; private set; }
	}
}
