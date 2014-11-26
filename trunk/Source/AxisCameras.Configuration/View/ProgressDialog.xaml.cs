#region Copyright (C) 2005-2011 Team MediaPortal

// Copyright (C) 2005-2011 Team MediaPortal
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

using System.ComponentModel;
using System.Windows;

namespace AxisCameras.Configuration.View
{
    /// <summary>
    /// Dialog displayed to the user when application is communicating with the camera.
    /// </summary>
    public partial class ProgressDialog : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressDialog"/> class.
        /// </summary>
        public ProgressDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Closing event of the Window control.
        /// </summary>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // Prevent closing the window if DialogResult hasn't been set
            e.Cancel = !DialogResult.HasValue;
        }
    }
}