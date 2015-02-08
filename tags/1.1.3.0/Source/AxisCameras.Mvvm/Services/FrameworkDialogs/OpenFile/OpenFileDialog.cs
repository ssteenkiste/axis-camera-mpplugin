#region Copyright (C) 2005-2015 Team MediaPortal

// Copyright (C) 2005-2015 Team MediaPortal
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
using AxisCameras.Core.Contracts;
using WinFormsOpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace AxisCameras.Mvvm.Services.FrameworkDialogs.OpenFile
{
    /// <summary>
    /// Class wrapping System.Windows.Forms.OpenFileDialog, making it accept a ViewModel.
    /// </summary>
    public class OpenFileDialog : IDisposable
    {
        private readonly OpenFileDialogViewModel viewModel;
        private WinFormsOpenFileDialog openFileDialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenFileDialog"/> class.
        /// </summary>
        /// <param name="viewModel">The ViewModel representing the open file dialog.</param>
        public OpenFileDialog(OpenFileDialogViewModel viewModel)
        {
            Requires.NotNull(viewModel);

            this.viewModel = viewModel;

            // Create OpenFileDialog
            openFileDialog = new WinFormsOpenFileDialog
            {
                AddExtension = viewModel.AddExtension,
                CheckFileExists = viewModel.CheckFileExists,
                CheckPathExists = viewModel.CheckPathExists,
                DefaultExt = viewModel.DefaultExt,
                FileName = viewModel.FileName,
                Filter = viewModel.Filter,
                InitialDirectory = viewModel.InitialDirectory,
                Multiselect = viewModel.Multiselect,
                Title = viewModel.Title
            };
        }

        /// <summary>
        /// Runs a common dialog box with the specified owner.
        /// </summary>
        /// <param name="owner">Any object that implements System.Windows.Forms.IWin32Window that
        /// represents the top-level window that will own the modal dialog box.</param>
        /// <returns>System.Windows.Forms.DialogResult.OK if the user clicks OK in the dialog box;
        /// otherwise, System.Windows.Forms.DialogResult.Cancel.</returns>
        public DialogResult ShowDialog(IWin32Window owner)
        {
            Requires.NotNull(owner);

            DialogResult result = openFileDialog.ShowDialog(owner);

            // Update ViewModel
            viewModel.FileName = openFileDialog.FileName;
            viewModel.FileNames = openFileDialog.FileNames;

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

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// OpenFileDialog is reclaimed by garbage collection.
        /// </summary>
        ~OpenFileDialog()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and optionally managed resources.
        /// </summary>
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources; false to release only unmanaged
        /// resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (openFileDialog != null)
                {
                    openFileDialog.Dispose();
                    openFileDialog = null;
                }
            }
        }

        #endregion
    }
}