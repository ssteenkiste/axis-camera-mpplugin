#region Copyright (C) 2005-2014 Team MediaPortal

// Copyright (C) 2005-2014 Team MediaPortal
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
using System.Windows.Threading;

namespace AxisCameras.Configuration.Service
{
    /// <summary>
    /// Class wrapping the Dispatcher.
    /// </summary>
    internal class DispatcherService : IDispatcherService
    {
        /// <summary>
        /// Executes the specified delegate asynchronously with the specified arguments on the thread
        /// that the Dispatcher was created on.
        /// </summary>
        /// <param name="method">
        /// The delegate to a method that takes parameters specified in args, which is pushed onto the
        /// Dispatcher event queue.
        /// </param>
        /// <param name="args">
        /// An array of objects to pass as arguments to the given method. Can be null.
        /// </param>
        /// <returns>
        /// An object, which is returned immediately after BeginInvoke is called, that can be used to
        /// interact with the delegate as it is pending execution in the event queue.
        /// </returns>
        public DispatcherOperation BeginInvoke(Delegate method, params object[] args)
        {
            return Dispatcher.CurrentDispatcher.BeginInvoke(method, args);
        }
    }
}