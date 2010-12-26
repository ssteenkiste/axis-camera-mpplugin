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
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using AxisCameraMPPlugin.Mvvm.Services.FrameworkDialogs.FolderBrowse;
using AxisCameraMPPlugin.Mvvm.Services.FrameworkDialogs.OpenFile;
using WinFormsDialogResult = System.Windows.Forms.DialogResult;

namespace AxisCameraMPPlugin.Mvvm.Services
{
	/// <summary>
	/// Interface responsible for showing dialogs.
	/// </summary>
	public interface IWindowService
	{
		/// <summary>
		/// Opens a window and returns without waiting for the newly opened window to close.
		/// </summary>
		/// <typeparam name="T">The type of window to open.</typeparam>
		/// <param name="viewModel">The ViewModel of the new window.</param>
		/// <param name="ownerViewModel">
		/// A ViewModel that represents the owner window.
		/// </param>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		void Show<T>(IViewModelBase viewModel, IViewModelBase ownerViewModel) where T : Window;


		/// <summary>
		/// Shows a dialog.
		/// </summary>
		/// <typeparam name="T">The type of dialog to open.</typeparam>
		/// <param name="viewModel">The ViewModel of the new dialog.</param>
		/// <param name="ownerViewModel">
		/// A ViewModel that represents the owner window of the dialog.
		/// </param>
		/// <returns>
		/// A nullable value of type bool that signifies how a window was closed by the user.
		/// </returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		bool? ShowDialog<T>(IDialogViewModelBase viewModel, IViewModelBase ownerViewModel)
			where T : Window;


		/// <summary>
		/// Shows a message box.
		/// </summary>
		/// <param name="ownerViewModel">
		/// A ViewModel that represents the owner window of the message box.
		/// </param>
		/// <param name="messageBoxText">A string that specifies the text to display.</param>
		/// <param name="caption">A string that specifies the title bar caption to display.</param>
		/// <param name="button">
		/// A MessageBoxButton value that specifies which button or buttons to display.
		/// </param>
		/// <param name="icon">A MessageBoxImage value that specifies the icon to display.</param>
		/// <param name="defaultResult">
		/// A MessageBoxResult value that specifies the default result of the message box.
		/// </param>
		/// <param name="options">
		/// A MessageBoxOptions value object that specifies the options.
		/// </param>
		/// <returns>
		/// A MessageBoxResult value that specifies which message box button is clicked by the user.
		/// </returns>
		MessageBoxResult ShowMessageBox(
			IViewModelBase ownerViewModel,
			string messageBoxText,
			string caption = "",
			MessageBoxButton button = MessageBoxButton.OK,
			MessageBoxImage icon = MessageBoxImage.None,
			MessageBoxResult defaultResult = MessageBoxResult.OK,
			MessageBoxOptions options = MessageBoxOptions.None)


		/// <summary>
		/// Shows the FolderBrowserDialog.
		/// </summary>
		/// <param name="viewModel">The ViewModel of the FolderBrowserDialog.</param>
		/// <param name="ownerViewModel">
		/// A ViewModel that represents the owner window of the dialog.
		/// </param>
		/// <returns>The DialogResult.OK if successful; otherwise DialogResult.Cancel.</returns>
		WinFormsDialogResult ShowFolderBrowserDialog(
			FolderBrowserDialogViewModel viewModel,
			IViewModelBase ownerViewModel);


		/// <summary>
		/// Shows the OpenFileDialog.
		/// </summary>
		/// <param name="viewModel">The ViewModel of the OpenFileDialog.</param>
		/// <param name="ownerViewModel">
		/// A ViewModel that represents the owner window of the dialog.
		/// </param>
		/// <returns>DialogResult.OK if successful; otherwise DialogResult.Cancel.</returns>
		WinFormsDialogResult ShowOpenFileDialog(
			OpenFileDialogViewModel viewModel,
			IViewModelBase ownerViewModel);
	}
}
