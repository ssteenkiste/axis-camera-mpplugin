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
using System.Windows.Forms;
using WinFormsFolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;

namespace AxisCameraMPPlugin.Mvvm.Services.FrameworkDialogs.FolderBrowse
{
	/// <summary>
	/// Class wrapping System.Windows.Forms.FolderBrowserDialog, making it accept a ViewModel.
	/// </summary>
	public class FolderBrowserDialog : IDisposable
	{
		private WinFormsFolderBrowserDialog folderBrowserDialog;
		private FolderBrowserDialogViewModel viewModel;


		/// <summary>
		/// Initializes a new instance of the <see cref="FolderBrowserDialog"/> class.
		/// </summary>
		/// <param name="viewModel">The ViewModel representing the folder browser dialog.</param>
		public FolderBrowserDialog(FolderBrowserDialogViewModel viewModel)
		{
			if (viewModel == null) throw new ArgumentNullException("viewModel");
			
			this.viewModel = viewModel;

			// Create FolderBrowserDialog
			folderBrowserDialog = new WinFormsFolderBrowserDialog
			{
				Description = viewModel.Description,
				SelectedPath = viewModel.SelectedPath,
				ShowNewFolderButton = viewModel.ShowNewFolderButton
			};
		}


		/// <summary>
		/// Runs a common dialog box with the specified owner.
		/// </summary>
		/// <param name="owner">
		/// Any object that implements System.Windows.Forms.IWin32Window that represents the top-level
		/// window that will own the modal dialog box.
		/// </param>
		/// <returns>
		/// System.Windows.Forms.DialogResult.OK if the user clicks OK in the dialog box; otherwise,
		/// System.Windows.Forms.DialogResult.Cancel.
		/// </returns>
		public DialogResult ShowDialog(IWin32Window owner)
		{
			if (owner == null) throw new ArgumentNullException("owner");
			
			DialogResult result = folderBrowserDialog.ShowDialog(owner);

			// Update ViewModel
			viewModel.SelectedPath = folderBrowserDialog.SelectedPath;

			return result;
		}


		#region IDisposable Members

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting
		/// unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}


		~FolderBrowserDialog()
		{
			Dispose(false);
		}


		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (folderBrowserDialog != null)
				{
					folderBrowserDialog.Dispose();
					folderBrowserDialog = null;
				}
			}
		}

		#endregion
	}
}
