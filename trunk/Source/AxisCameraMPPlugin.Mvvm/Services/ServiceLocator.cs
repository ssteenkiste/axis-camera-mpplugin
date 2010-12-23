using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
