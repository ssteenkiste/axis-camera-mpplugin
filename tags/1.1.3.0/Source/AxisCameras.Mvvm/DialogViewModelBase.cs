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

using System.Windows.Input;

namespace AxisCameras.Mvvm
{
    /// <summary>
    /// Base class for all ViewModels representing a modal dialog.
    /// </summary>
    public abstract class DialogViewModelBase : ViewModelBase, IDialogViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DialogViewModelBase"/> class.
        /// </summary>
        protected DialogViewModelBase()
        {
            OkCommand = new RelayCommand(Ok);
        }

        /// <summary>
        /// Gets or sets the command for setting the DialogResult of a dialog.
        /// </summary>
        public ICommand DialogResultCommand { get; set; }

        /// <summary>
        /// Gets the command responsible for OK click.
        /// </summary>
        public ICommand OkCommand { get; private set; }

        /// <summary>
        /// Method inheriting classes can override to apply custom logic to prevent the dialog from
        /// closing when OK button is pressed.
        /// </summary>
        /// <returns>True if dialog should close;otherwise false.</returns>
        protected virtual bool OnOk()
        {
            return true;
        }

        /// <summary>
        /// Ok method that executes when the OK button in a dialog is clicked.
        /// </summary>
        /// <param name="parameter">The command parameter</param>
        private void Ok(object parameter)
        {
            if (Validate() && OnOk() && DialogResultCommand != null)
            {
                DialogResultCommand.Execute(true);
            }
        }
    }
}