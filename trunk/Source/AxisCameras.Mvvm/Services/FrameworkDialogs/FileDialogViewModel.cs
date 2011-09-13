#region Copyright (C) 2005-2011 Team MediaPortal

// Copyright (C) 2005-2011 Team MediaPortal
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

namespace AxisCameras.Mvvm.Services.FrameworkDialogs
{
	/// <summary>
	/// ViewModel of the abstract FileDialog.
	/// </summary>
	public abstract class FileDialogViewModel : IFileDialog
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FileDialogViewModel"/> class.
		/// </summary>
		protected FileDialogViewModel()
		{
			// Set default values
			AddExtension = true;
			CheckFileExists = true;
			CheckPathExists = true;
			DefaultExt = string.Empty;
			FileName = string.Empty;
			FileNames = new string[] { string.Empty };
			Filter = string.Empty;
			InitialDirectory = string.Empty;
			Title = string.Empty;
		}


		/// <summary>
		/// Gets or sets a value indicating whether the dialog box automatically adds an extension to a
		/// file name if the user omits the extension.
		/// </summary>
		public bool AddExtension { get; set; }


		/// <summary>
		/// Gets or sets a value indicating whether the dialog box displays a warning if the user
		/// specifies a file name that does not exist.
		/// </summary>
		public bool CheckFileExists { get; set; }


		/// <summary>
		/// Gets or sets a value indicating whether the dialog box displays a warning if the user
		/// specifies a path that does not exist.
		/// </summary>
		public bool CheckPathExists { get; set; }


		/// <summary>
		/// Gets or sets the default file name extension.
		/// </summary>
		public string DefaultExt { get; set; }


		/// <summary>
		/// Gets or sets a string containing the file name selected in the file dialog box.
		/// </summary>
		public string FileName { get; set; }


		/// <summary>
		/// Gets the file names of all selected files in the dialog box.
		/// </summary>
		public string[] FileNames { get; set; }


		/// <summary>
		/// Gets or sets the current file name filter string, which determines the choices that appear
		/// in the "Save as file type" or "Files of type" box in the dialog box.
		/// </summary>
		public string Filter { get; set; }


		/// <summary>
		/// Gets or sets the initial directory displayed by the file dialog box.
		/// </summary>
		public string InitialDirectory { get; set; }


		/// <summary>
		/// Gets or sets the file dialog box title.
		/// </summary>
		public string Title { get; set; }
	}
}
