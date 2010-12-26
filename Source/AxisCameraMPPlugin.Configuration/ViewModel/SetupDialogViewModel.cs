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
using System.Collections.ObjectModel;
using System.Windows.Input;
using AxisCameraMPPlugin.Configuration.Provider;
using AxisCameraMPPlugin.Mvvm;

namespace AxisCameraMPPlugin.Configuration.ViewModel
{
	/// <summary>
	/// View model capable of adding, editing and removing cameras.
	/// </summary>
	class SetupDialogViewModel : DialogViewModelBase
	{
		public SetupDialogViewModel(ICameraNameViewModelsProvider cameraNameViewModelsProvider)
		{
			if (cameraNameViewModelsProvider == null) throw new ArgumentNullException("cameraNameViewModelsProvider");

			Cameras = new ObservableCollection<CameraNameViewModel>(cameraNameViewModelsProvider.Provide());
			
			AddCommand = new RelayCommand(Add);
			EditCommand = new RelayCommand(Edit, CanEdit);
			RemoveCommand = new RelayCommand(Remove, CanRemove);
		}



		/// <summary>
		/// Gets the cameras.
		/// </summary>
		public ObservableCollection<CameraNameViewModel> Cameras
		{
			get { return Property(() => Cameras); }
			private set { Property(() => Cameras, value); }
		}


		/// <summary>
		/// Gets the command adding a camera.
		/// </summary>
		public ICommand AddCommand
		{
			get { return Property(() => AddCommand); }
			private set { Property(() => AddCommand, value); }
		}


		/// <summary>
		/// Gets the command editing a camera.
		/// </summary>
		public ICommand EditCommand
		{
			get { return Property(() => EditCommand); }
			private set { Property(() => EditCommand, value); }
		}


		/// <summary>
		/// Gets the command removing a camera.
		/// </summary>
		public ICommand RemoveCommand
		{
			get { return Property(() => RemoveCommand); }
			private set { Property(() => RemoveCommand, value); }
		}


		/// <summary>
		/// Adds a new camera.
		/// </summary>
		private void Add(object parameter)
		{
		}


		/// <summary>
		/// Edits a camera.
		/// </summary>
		private void Edit(object parameter)
		{
		}


		/// <summary>
		/// Determines whether a camera can be edited.
		/// </summary>
		/// <returns>True if a camera can be edited; otherwise false.</returns>
		private bool CanEdit(object parameter)
		{
			return false;
		}


		/// <summary>
		/// Removes a camera.
		/// </summary>
		private void Remove(object parameter)
		{
		}


		/// <summary>
		/// Determines whether a camera can be removed.
		/// </summary>
		/// <returns>True if a camera can be removed; otherwise false.</returns>
		private bool CanRemove(object parameter)
		{
			return false;
		}
	}
}
