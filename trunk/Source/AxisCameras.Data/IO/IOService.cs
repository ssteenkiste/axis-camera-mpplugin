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
using System.IO;

namespace AxisCameras.Data.IO
{
	/// <summary>
	/// Class responsible for I/O operations.
	/// </summary>
	class IOService : IIOService
	{
		/// <summary>
		/// Determines whether the specified file exists.
		/// </summary>
		/// <param name="path">The file to check.</param>
		/// <returns>
		/// true if the caller has the required permissions and path contains the name of an existing
		/// file; otherwise, false. This method also returns false if path is null, an invalid path, or
		/// a zero-length string. If the caller does not have sufficient permissions to read the
		/// specified file, no exception is thrown and the method returns false regardless of the
		/// existence of path.
		/// </returns>
		public bool FileExists(string path)
		{
			return File.Exists(path);
		}
	}
}
