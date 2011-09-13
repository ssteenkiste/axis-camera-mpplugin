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

namespace AxisCameras.Core.Contracts
{
	/// <summary>
	/// Class used when defining requirements, i.e. simplifies validating arguments.
	/// </summary>
	public static class Requires
	{
		/// <summary>
		/// Requires that specified action is true.
		/// </summary>
		public static void IsTrue(bool condition, string errorMessage = null)
		{
			if (!condition)
			{
				Throw(errorMessage);
			}
		}


		/// <summary>
		/// Requires that specified value isn't null or empty.
		/// </summary>
		public static void IsNotNullOrEmpty(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				Throw(string.Empty);
			}
		}


		/// <summary>
		/// Requires that specified value isn't null.
		/// </summary>
		public static void NotNull<T>(T value) where T : class
		{
			if (value == null)
			{
				Throw(string.Empty);
			}
		}


		/// <summary>
		/// Marks that requirements have failed.
		/// </summary>
		/// <param name="errorMessage">The error message.</param>
		public static void Fail(string errorMessage)
		{
			Throw(errorMessage);
		}


		/// <summary>
		/// Throws an <see cref="RequiresException"/> with specified error message.
		/// </summary>
		/// <param name="errorMessage">The error message.</param>
		private static void Throw(string errorMessage)
		{
			throw new RequiresException(errorMessage);
		}
	}
}