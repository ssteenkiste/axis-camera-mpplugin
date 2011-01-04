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

namespace AxisCameraMPPlugin.Configuration.Service
{
	/// <summary>
	/// Interface responsible for I/O related operations.
	/// </summary>
	public interface IIOService
	{
		/// <summary>
		/// Deletes the specified file.
		/// </summary>
		/// <param name="filePath">The name of the file to be deleted.</param>
		/// <returns>true if file was deleted; otherwise false.</returns>
		bool Delete(string filePath);


		/// <summary>
		/// Gets the path of a plugin thumb.
		/// </summary>
		/// <param name="thumbFileName">Name of the thumb file.</param>
		/// <returns>The path of a plugin thumb.</returns>
		string GetThumbPath(string thumbFileName);
	}
}
