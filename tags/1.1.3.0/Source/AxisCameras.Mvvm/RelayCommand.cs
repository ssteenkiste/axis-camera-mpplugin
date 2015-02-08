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
using System.Diagnostics;
using System.Windows.Input;
using AxisCameras.Core.Contracts;

namespace AxisCameras.Mvvm
{
    /// <summary>
    /// A command whose sole purpose is to 
    /// relay its functionality to other
    /// objects by invoking delegates. The
    /// default return value for the CanExecute
    /// method is 'true'.
    /// 
    /// http://msdn.microsoft.com/en-us/magazine/dd419663.aspx
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            Requires.NotNull(execute);

            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current
        /// state.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command.  If the command does not require data to be passed, this
        /// object can be set to null.
        /// </param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command.  If the command does not require data to be passed, this
        /// object can be set to null.
        /// </param>
        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}