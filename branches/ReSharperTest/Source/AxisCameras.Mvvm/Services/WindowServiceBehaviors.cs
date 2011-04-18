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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace AxisCameras.Mvvm.Services
{
	/// <summary>
	/// Used for registering a framework element as a view in MVVM.
	/// </summary>
	public static class WindowServiceBehaviors
	{
		/// <summary>
		/// The registered views.
		/// </summary>
		private static readonly HashSet<FrameworkElement> Views = new HashSet<FrameworkElement>();


		/// <summary>
		/// Attached property describing whether a FrameworkElement is acting as a View in MVVM.
		/// </summary>
		public static readonly DependencyProperty IsRegisteredViewProperty =
			DependencyProperty.RegisterAttached(
			"IsRegisteredView",
			typeof(bool),
			typeof(WindowServiceBehaviors),
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
		/// Finds a view by matching a view model to the data context of all views.
		/// </summary>
		/// <param name="viewModel">The view model to look for.</param>
		/// <returns>The view if found; otherwise null.</returns>
		internal static FrameworkElement FindView(INotifyPropertyChanged viewModel)
		{
			return Views.SingleOrDefault(view => ReferenceEquals(view.DataContext, viewModel));
		}


		/// <summary>
		/// Finds the owning window to a specified View.
		/// </summary>
		/// <param name="view">The view to use when searching for the owner.</param>
		/// <returns>A window if an owner was found; otherwise null.</returns>
		internal static Window FindOwner(FrameworkElement view)
		{
			var window = view as Window;

			return window ?? Window.GetWindow(view);
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
			if (DesignerProperties.GetIsInDesignMode(target))
			{
				return;
			}

			var view = target as FrameworkElement;
			if (view != null)
			{
				var newValue = (bool)e.NewValue;

				if (newValue)
				{
					Register(view);
				}
				else
				{
					Unregister(view);
				}
			}
		}


		/// <summary>
		/// Registers a View.
		/// </summary>
		/// <param name="view">The registered View.</param>
		private static void Register(FrameworkElement view)
		{
			if (view == null) throw new ArgumentNullException("view");
			if (Views.Contains(view))
				throw new ArgumentException("View has already been registered.", "view");

			// Get owner window
			Window owner = view as Window ?? Window.GetWindow(view);

			if (owner == null)
			{
				// Perform a late register when the View hasn't been loaded yet.
				// This will happen if e.g. the View is contained in a Frame.
				view.Loaded += LateRegister;
				return;
			}

			// Register for owner window closing, since we then should unregister View reference,
			// preventing memory leaks
			owner.Closed += OwnerClosed;

			Views.Add(view);
		}


		/// <summary>
		/// Unregisters a View.
		/// </summary>
		/// <param name="view">The unregistered View.</param>
		private static void Unregister(FrameworkElement view)
		{
			if (view == null) throw new ArgumentNullException("view");
			if (!Views.Contains(view))
				throw new ArgumentException("View has not been registered.", "view");

			Views.Remove(view);
		}


		/// <summary>
		/// Callback for late View register. It wasn't possible to do a instant register since the
		/// View wasn't at that point part of the logical nor visual tree.
		/// </summary>
		private static void LateRegister(object sender, RoutedEventArgs e)
		{
			var view = sender as FrameworkElement;
			if (view != null)
			{
				// Unregister loaded event
				view.Loaded -= LateRegister;

				// Register the view
				Register(view);
			}
		}


		/// <summary>
		/// Handles owner window closed, View service should then unregister all Views acting within
		/// the closed window.
		/// </summary>
		private static void OwnerClosed(object sender, EventArgs e)
		{
			var owner = sender as Window;
			if (owner != null)
			{
				// Find Views acting within closed window
				IEnumerable<FrameworkElement> windowViews =
					from view in Views
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
