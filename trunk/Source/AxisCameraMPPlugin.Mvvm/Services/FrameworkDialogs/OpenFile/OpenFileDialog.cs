using System;
using System.Windows.Forms;
using WinFormsOpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace AxisCameraMPPlugin.Mvvm.Services.FrameworkDialogs.OpenFile
{
	/// <summary>
	/// Class wrapping System.Windows.Forms.OpenFileDialog, making it accept a ViewModel.
	/// </summary>
	public class OpenFileDialog : IDisposable
	{
		private WinFormsOpenFileDialog openFileDialog;
		private OpenFileDialogViewModel viewModel;


		/// <summary>
		/// Initializes a new instance of the <see cref="OpenFileDialog"/> class.
		/// </summary>
		/// <param name="viewModel">The ViewModel representing the open file dialog.</param>
		public OpenFileDialog(OpenFileDialogViewModel viewModel)
		{
			if (viewModel == null) throw new ArgumentNullException("viewModel");
			
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
			if (owner == null) throw new ArgumentNullException("owner");

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


		~OpenFileDialog()
		{
			Dispose(false);
		}


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
