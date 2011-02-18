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
using System.Windows.Input;

namespace AxisCameras.Mvvm
{
	/// <summary>
	/// Interface for all ViewModels representing a modal dialog.
	/// </summary>
	public interface IDialogViewModelBase : IViewModelBase
	{
		/// <summary>
		/// Gets or sets the command for setting the DialogResult of a dialog.
		/// </summary>
		ICommand DialogResultCommand { get; set; }


		/// <summary>
		/// Gets the command responsible for OK click.
		/// </summary>
		ICommand OkCommand { get; }
	}
}
