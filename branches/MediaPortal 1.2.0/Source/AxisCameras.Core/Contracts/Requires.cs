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
		public static void IsTrue(bool condition)
		{
			if (!condition)
			{
				throw new RequiresException();
			}
		}


		/// <summary>
		/// Requires that specified value isn't null.
		/// </summary>
		public static void NotNull<T>(T value) where T : class
		{
			if (value == null)
			{
				throw new RequiresException();
			}
		}
	}
}
