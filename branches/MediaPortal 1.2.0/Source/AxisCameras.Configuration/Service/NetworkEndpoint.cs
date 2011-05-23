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
using AxisCameras.Core.Contracts;

namespace AxisCameras.Configuration.Service
{
	/// <summary>
	/// Class describing a network endpoint.
	/// </summary>
	class NetworkEndpoint
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NetworkEndpoint"/> class.
		/// </summary>
		/// <param name="address">The network address.</param>
		/// <param name="port">The HTTP port.</param>
		/// <param name="userName">The user name.</param>
		/// <param name="password">The password.</param>
		public NetworkEndpoint(string address, int port, string userName, string password)
		{
			Requires.IsNotNullOrEmpty(address);
			Requires.IsTrue(port >= 1 && port < 65536);
			Requires.IsNotNullOrEmpty(userName);
			Requires.IsNotNullOrEmpty(password);

			Address = address;
			Port = port;
			UserName = userName;
			Password = password;
		}

		/// <summary>
		/// Gets the network address.
		/// </summary>
		public string Address { get; private set; }


		/// <summary>
		/// Gets the HTTP port.
		/// </summary>
		public int Port { get; private set; }


		/// <summary>
		/// Gets the user name.
		/// </summary>
		public string UserName { get; private set; }


		/// <summary>
		/// Gets the password.
		/// </summary>
		public string Password { get; private set; }
	}
}
