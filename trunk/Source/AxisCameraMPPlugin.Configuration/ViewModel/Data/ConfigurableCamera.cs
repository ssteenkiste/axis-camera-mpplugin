﻿#region Copyright (C) 2005-2010 Team MediaPortal

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

namespace AxisCameraMPPlugin.Configuration.ViewModel.Data
{
	/// <summary>
	/// Class describing a configurable camera.
	/// </summary>
	public class ConfigurableCamera
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurableCamera"/> class.
		/// </summary>
		/// <param name="id">The camera id.</param>
		public ConfigurableCamera(Guid id)
		{
			if (id == null) throw new ArgumentNullException("id");
			if (id == Guid.Empty) throw new ArgumentException("ID cannot be Guid.Empty");

			Id = id;
		}


		/// <summary>
		/// Gets the Id.
		/// </summary>
		public Guid Id { get; private set; }


		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name { get; set; }


		/// <summary>
		/// Gets or sets the address.
		/// </summary>
		public string Address { get; set; }


		/// <summary>
		/// Gets or sets the HTTP port.
		/// </summary>
		public int Port { get; set; }


		/// <summary>
		/// Gets or sets the user name.
		/// </summary>
		public string UserName { get; set; }


		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		public string Password { get; set; }


		/// <summary>
		/// Gets or sets the snapshot.
		/// </summary>
		public byte[] Snapshot { get; set; }


		/// <summary>
		/// Creates a new object that is a deep copy of the current instance.
		/// </summary>
		/// <returns>
		/// A new object that is a deep copy of this instance.
		/// </returns>
		public ConfigurableCamera Clone()
		{
			return new ConfigurableCamera(Id)
			{
				Name = Name,
				Address = Address,
				Port = Port,
				UserName = UserName,
				Password = Password,
				Snapshot = (byte[])Snapshot.Clone()
			};
		}
	}
}