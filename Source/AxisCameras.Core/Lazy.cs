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

namespace AxisCameras.Core
{
	/// <summary>
	/// Provides support for lazy initialization.
	/// </summary>
	/// <typeparam name="T">
	/// Specifies the type of object that is being lazily initialized.
	/// </typeparam>
	public class Lazy<T>
	{
		private readonly Func<T> valueFactory;
		private T value;


		/// <summary>
		/// Initializes a new instance of the Lazy<T> class. When lazy initialization occurs, the
		/// specified initialization function is used.
		/// </summary>
		/// <param name="valueFactory">
		/// The delegate that is invoked to produce the lazily initialized value when it is needed.
		/// </param>
		public Lazy(Func<T> valueFactory)
		{
			if (valueFactory == null) throw new ArgumentNullException("valueFactory");

			this.valueFactory = valueFactory;
		}


		/// <summary>
		/// Gets a value that indicates whether a value has been created for this Lazy<T> instance.
		/// </summary>
		/// <value>
		/// true if a value has been created for this Lazy<T> instance; otherwise, false.
		/// </value>
		public bool IsValueCreated { get; private set; }


		/// <summary>
		/// Gets the lazily initialized value of the current Lazy<T> instance.
		/// </summary>
		public T Value
		{
			get
			{
				if (!IsValueCreated)
				{
					value = valueFactory();
					IsValueCreated = true;
				}

				return value;
			}
		}
	}
}
