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

namespace AxisCameraMPPlugin.Mvvm.Services.FrameworkDialogs.FolderBrowse
{
	/// <summary>
	/// Interface describing the FolderBrowserDialog.
	/// </summary>
	public interface IFolderBrowserDialog
	{
		/// <summary>
		/// Gets or sets the descriptive text displayed above the tree view control in the dialog box.
		/// </summary>
		string Description { get; set; }


		/// <summary>
		/// Gets or sets the path selected by the user.
		/// </summary>
		string SelectedPath { get; set; }


		/// <summary>
		/// Gets or sets a value indicating whether the New Folder button appears in the folder browser
		/// dialog box.
		/// </summary>
		bool ShowNewFolderButton { get; set; }
	}
}
