using System.Diagnostics.CodeAnalysis;
using System.Windows;
using WinFormsDialogResult = System.Windows.Forms.DialogResult;
using AxisCameraMPPlugin.Mvvm.Services.FrameworkDialogs.FolderBrowse;
using AxisCameraMPPlugin.Mvvm.Services.FrameworkDialogs.OpenFile;

namespace AxisCameraMPPlugin.Mvvm.Services
{
	/// <summary>
	/// Interface responsible for showing dialogs.
	/// </summary>
	public interface IWindowService
	{
		/// <summary>
		/// Registers a View.
		/// </summary>
		/// <param name="view">The registered View.</param>
		void Register(FrameworkElement view);


		/// <summary>
		/// Unregisters a View.
		/// </summary>
		/// <param name="view">The unregistered View.</param>
		void Unregister(FrameworkElement view);


		/// <summary>
		/// Opens a window and returns without waiting for the newly opened window to close.
		/// </summary>
		/// <typeparam name="T">The type of window to open.</typeparam>
		/// <param name="viewModel">The ViewModel of the new window.</param>
		/// <param name="ownerViewModel">
		/// A ViewModel that represents the owner window.
		/// </param>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		void Show<T>(ViewModelBase viewModel, ViewModelBase ownerViewModel) where T : Window;


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
		bool? ShowDialog<T>(DialogViewModelBase viewModel, ViewModelBase ownerViewModel)
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
			ViewModelBase ownerViewModel,
			string messageBoxText,
			string caption,
			MessageBoxButton button,
			MessageBoxImage icon,
			MessageBoxResult defaultResult,
			MessageBoxOptions options);


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
			ViewModelBase ownerViewModel);


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
			ViewModelBase ownerViewModel);
	}
}
