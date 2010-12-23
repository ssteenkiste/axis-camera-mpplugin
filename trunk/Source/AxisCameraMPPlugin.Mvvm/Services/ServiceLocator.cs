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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AxisCameraMPPlugin.Mvvm.Services
{
	/// <summary>
	/// A very basic service locator.
	/// </summary>
	public static class ServiceLocator
	{
		private static Dictionary<Type, object> services = new Dictionary<Type, object>();


		/// <summary>
		/// Adds a service.
		/// </summary>
		public static void Add<T>(T service)
		{
			if (services.ContainsKey(typeof(T)))
				throw new ArgumentException("Service has already been added.", "service");

			services.Add(typeof(T), service);
		}


		/// <summary>
		/// Resolves a service.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		public static T Resolve<T>()
		{
			if (!services.ContainsKey(typeof(T)))
				throw new ArgumentException("Service has not been added: " + typeof(T));
			
			return (T)services[typeof(T)];
		}
	}
}
