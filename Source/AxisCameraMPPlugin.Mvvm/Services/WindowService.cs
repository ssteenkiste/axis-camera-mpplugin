using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using WinFormsDialogResult = System.Windows.Forms.DialogResult;
using AxisCameraMPPlugin.Mvvm.Services.FrameworkDialogs.FolderBrowse;
using AxisCameraMPPlugin.Mvvm.Services.FrameworkDialogs.OpenFile;
using AxisCameraMPPlugin.Mvvm.Services.FrameworkDialogs;

namespace AxisCameraMPPlugin.Mvvm.Services
{
	/// <summary>
	/// Service responsible for showing dialogs.
	/// </summary>
	public class WindowService : IWindowService
	{
		private HashSet<FrameworkElement> views;


		/// <summary>
		/// Initializes a new instance of the <see cref="WindowService"/> class.
		/// </summary>
		public WindowService()
		{
			views = new HashSet<FrameworkElement>();
		}


		#region IWindowService Members

		/// <summary>
		/// Registers a View.
		/// </summary>
		/// <param name="view">The registered View.</param>
		public void Register(FrameworkElement view)
		{
			if (view == null) throw new ArgumentNullException("view");
			if (views.Contains(view))
				throw new ArgumentException("View has already been registered.", "view");

			// Get owner window
			Window owner = view as Window;
			if (owner == null)
			{
				owner = Window.GetWindow(view);
			}

			if (owner == null)
			{
				throw new InvalidOperationException("View is not contained within a Window.");
			}

			// Register for owner window closing, since we then should unregister View reference,
			// preventing memory leaks
			owner.Closed += OwnerClosed;

			views.Add(view);
		}


		/// <summary>
		/// Unregisters a View.
		/// </summary>
		/// <param name="view">The unregistered View.</param>
		public void Unregister(FrameworkElement view)
		{
			if (view == null) throw new ArgumentNullException("view");
			if (!views.Contains(view))
				throw new ArgumentException("View has not been registered.", "view");

			views.Remove(view);
		}


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
		public void Show<T>(ViewModelBase viewModel, ViewModelBase ownerViewModel = null) where T : Window
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
		/// A ViewModel that represents the owner window of the dialog.
		/// </param>
		/// <returns>
		/// A nullable value of type bool that signifies how a window was closed by the user.
		/// </returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		public bool? ShowDialog<T>(DialogViewModelBase viewModel, ViewModelBase ownerViewModel)
			where T : Window
		{
			if (viewModel == null) throw new ArgumentNullException("viewModel");
			if (ownerViewModel == null) throw new ArgumentNullException("ownerViewModel");
			
			// Create dialog and set properties
			T dialog = Activator.CreateInstance<T>();
			dialog.Owner = FindOwnerWindow(ownerViewModel);
			dialog.DataContext = viewModel;

			// Make OK command close dialog
			viewModel.OkCommand = new RelayCommand((o) => dialog.DialogResult = true, viewModel.CanOk);

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
			ViewModelBase ownerViewModel,
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
			ViewModelBase ownerViewModel)
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
			ViewModelBase ownerViewModel)
		{
			if (viewModel == null) throw new ArgumentNullException("viewModel");
			if (ownerViewModel == null) throw new ArgumentNullException("ownerViewModel");
			
			// Create OpenFileDialog with specified ViewModel
			OpenFileDialog dialog = new OpenFileDialog(viewModel);

			// Show dialog
			return dialog.ShowDialog(new WindowWrapper(FindOwnerWindow(ownerViewModel)));
		}

		#endregion


		#region Attached properties

		/// <summary>
		/// Attached property describing whether a FrameworkElement is acting as a View in MVVM.
		/// </summary>
		public static readonly DependencyProperty IsRegisteredViewProperty =
			DependencyProperty.RegisterAttached(
			"IsRegisteredView",
			typeof(bool),
			typeof(WindowService),
			new UIPropertyMetadata(IsRegisteredViewPropertyChanged));


		/// <summary>
		/// Gets value describing whether FrameworkElement is acting as View in MVVM.
		/// </summary>
		public static bool GetIsRegisteredView(DependencyObject target)
		{
			return (bool)target.GetValue(IsRegisteredViewProperty);
		}


		/// <summary>
		/// Sets value describing whether FrameworkElement is acting as View in MVVM.
		/// </summary>
		public static void SetIsRegisteredView(DependencyObject target, bool value)
		{
			target.SetValue(IsRegisteredViewProperty, value);
		}


		/// <summary>
		/// Is responsible for handling IsRegisteredViewProperty changes, i.e. whether
		/// FrameworkElement is acting as View in MVVM or not.
		/// </summary>
		private static void IsRegisteredViewPropertyChanged(
			DependencyObject target,
			DependencyPropertyChangedEventArgs e)
		{
			// The Visual Studio Designer or Blend will run this code when setting the attached
			// property, however at that point there is no IWindowService registered
			// in the ServiceLocator which will cause the Resolve method to throw a ArgumentException.
			if (DesignerProperties.GetIsInDesignMode(target)) return;

			FrameworkElement view = target as FrameworkElement;
			if (view != null)
			{
				bool newValue = (bool)e.NewValue;

				if (newValue)
				{
					ServiceLocator.Resolve<IWindowService>().Register(view);
				}
				else
				{
					ServiceLocator.Resolve<IWindowService>().Unregister(view);
				}
			}
		}

		#endregion


		/// <summary>
		/// Finds window corresponding to specified ViewModel.
		/// </summary>
		private Window FindOwnerWindow(ViewModelBase viewModel)
		{
			FrameworkElement view = views.SingleOrDefault(
				v => ReferenceEquals(v.DataContext, viewModel));
			if (view == null)
			{
				throw new ArgumentException("Viewmodel is not referenced by any registered View.");
			}

			// Get owner window
			Window owner = view as Window;
			if (owner == null)
			{
				owner = Window.GetWindow(view);
			}

			// Make sure owner window was found
			if (owner == null)
			{
				throw new InvalidOperationException("View is not contained within a Window.");
			}

			return owner;
		}


		/// <summary>
		/// Handles owner window closed, View service should then unregister all Views acting within
		/// the closed window.
		/// </summary>
		private void OwnerClosed(object sender, EventArgs e)
		{
			Window owner = sender as Window;
			if (owner != null)
			{
				// Find Views acting within closed window
				IEnumerable<FrameworkElement> windowViews =
					from view in views
					where Window.GetWindow(view) == owner
					select view;

				// Unregister Views in window
				foreach (FrameworkElement view in windowViews.ToArray())
				{
					Unregister(view);
				}
			}
		}
	}
}
