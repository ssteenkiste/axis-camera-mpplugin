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
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using AxisCameraMPPlugin.Mvvm.Services.FrameworkDialogs;
using AxisCameraMPPlugin.Mvvm.Services.FrameworkDialogs.FolderBrowse;
using AxisCameraMPPlugin.Mvvm.Services.FrameworkDialogs.OpenFile;
using WinFormsDialogResult = System.Windows.Forms.DialogResult;

namespace AxisCameraMPPlugin.Mvvm.Services
{
	/// <summary>
	/// Service responsible for showing dialogs.
	/// </summary>
	public class WindowService : IWindowService
	{
		#region IWindowService Members


		/// <summary>
		/// Opens a window and returns without waiting for the newly opened window to close.
		/// </summary>
		/// <typeparam name="T">The type of window to open.</typeparam>
		/// <param name="viewModel">The ViewModel of the new window.</param>
		/// <param name="ownerViewModel">
		/// A ViewModel that represents the owner window.
		/// </param>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		[SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
		public void Show<T>(IViewModelBase viewModel, IViewModelBase ownerViewModel = null)
			where T : Window
		{
			if (viewModel == null) throw new ArgumentNullException("viewModel");
			
			// Create window and set properties
			T dialog = Activator.CreateInstance<T>();
			if (ownerViewModel != null)
			{
				dialog.Owner = FindOwnerWindow(ownerViewModel);
			}
			dialog.DataContext = viewModel;

			// Show window
			dialog.Show();
		}


		/// <summary>
		/// Shows a dialog.
		/// </summary>
		/// <typeparam name="T">The type of dialog to open.</typeparam>
		/// <param name="viewModel">The ViewModel of the new dialog.</param>
		/// <param name="ownerViewModel">
		/// A ViewModel that represents the owner window of the dialog. Default value is null.
		/// </param>
		/// <returns>
		/// A nullable value of type bool that signifies how a window was closed by the user.
		/// </returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		[SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
		public bool? ShowDialog<T>(IDialogViewModelBase viewModel, IViewModelBase ownerViewModel = null)
			where T : Window
		{
			if (viewModel == null) throw new ArgumentNullException("viewModel");
			
			// Create dialog and set properties
			T dialog = Activator.CreateInstance<T>();

			if (ownerViewModel != null)
			{
				dialog.Owner = FindOwnerWindow(ownerViewModel);
			}

			dialog.DataContext = viewModel;
			viewModel.DialogResultCommand = new RelayCommand(result => dialog.DialogResult = (bool)result);
			
			// Show dialog
			return dialog.ShowDialog();
		}


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
		[SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
		public MessageBoxResult ShowMessageBox(
			IViewModelBase ownerViewModel,
			string messageBoxText,
			string caption = "",
			MessageBoxButton button = MessageBoxButton.OK,
			MessageBoxImage icon = MessageBoxImage.None,
			MessageBoxResult defaultResult = MessageBoxResult.OK,
			MessageBoxOptions options = MessageBoxOptions.None)
		{
			if (ownerViewModel == null) throw new ArgumentNullException("ownerViewModel");
			if (string.IsNullOrEmpty(messageBoxText))
				throw new ArgumentException("MessageBox text cannot be null or empty.", "messageBoxText");

			return MessageBox.Show(
				FindOwnerWindow(ownerViewModel),
				messageBoxText,
				caption,
				button,
				icon,
				defaultResult,
				options);
		}


		/// <summary>
		/// Shows the FolderBrowserDialog.
		/// </summary>
		/// <param name="viewModel">The ViewModel of the FolderBrowserDialog.</param>
		/// <param name="ownerViewModel">
		/// A ViewModel that represents the owner window of the dialog.
		/// </param>
		/// <returns>The DialogResult.OK if successful; otherwise DialogResult.Cancel.</returns>
		public WinFormsDialogResult ShowFolderBrowserDialog(
			FolderBrowserDialogViewModel viewModel,
			IViewModelBase ownerViewModel)
		{
			if (viewModel == null) throw new ArgumentNullException("viewModel");
			if (ownerViewModel == null) throw new ArgumentNullException("ownerViewModel");
			
			// Create FolderBrowserDialog with specified ViewModel
			FolderBrowserDialog dialog = new FolderBrowserDialog(viewModel);

			// Show dialog
			return dialog.ShowDialog(new WindowWrapper(FindOwnerWindow(ownerViewModel)));
		}


		/// <summary>
		/// Shows the OpenFileDialog.
		/// </summary>
		/// <param name="viewModel">The ViewModel of the OpenFileDialog.</param>
		/// <param name="ownerViewModel">
		/// A ViewModel that represents the owner window of the dialog.
		/// </param>
		/// <returns>DialogResult.OK if successful; otherwise DialogResult.Cancel.</returns>
		public WinFormsDialogResult ShowOpenFileDialog(
			OpenFileDialogViewModel viewModel,
			IViewModelBase ownerViewModel)
		{
			if (viewModel == null) throw new ArgumentNullException("viewModel");
			if (ownerViewModel == null) throw new ArgumentNullException("ownerViewModel");
			
			// Create OpenFileDialog with specified ViewModel
			OpenFileDialog dialog = new OpenFileDialog(viewModel);

			// Show dialog
			return dialog.ShowDialog(new WindowWrapper(FindOwnerWindow(ownerViewModel)));
		}

		#endregion


		/// <summary>
		/// Finds window corresponding to specified ViewModel.
		/// </summary>
		private static Window FindOwnerWindow(IViewModelBase viewModel)
		{
			FrameworkElement view = WindowServiceBehaviors.FindView(viewModel);

			// Check if view was not registered
			if (view == null)
			{
				return null;
			}

			return WindowServiceBehaviors.FindOwner(view);
		}
	}
}
