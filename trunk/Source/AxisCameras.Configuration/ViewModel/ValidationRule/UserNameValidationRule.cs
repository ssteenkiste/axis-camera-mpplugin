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

using System.Text.RegularExpressions;
using AxisCameras.Configuration.Properties;
using AxisCameras.Mvvm.Validation;

namespace AxisCameras.Configuration.ViewModel.ValidationRule
{
    /// <summary>
    /// Validation rule that validates the user name of a camera.
    /// </summary>
    internal class UserNameValidationRule : IValidationRule
    {
        private readonly Regex userNameRegex;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNameValidationRule"/> class.
        /// </summary>
        public UserNameValidationRule()
        {
            userNameRegex = new Regex("^[A-Za-z]+[A-Za-z0-9_]*$");
        }

        /// <summary>
        /// Validates the specified user name.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>true if validation is successful; otherwise false.</returns>
        public bool Validate(object value)
        {
            string userName = value as string;
            if (string.IsNullOrEmpty(userName))
            {
                return false;
            }

            if (userName.Length > 14)
            {
                return false;
            }

            // Make sure a user name starts with a letter and only contains letters and digits
            return userNameRegex.IsMatch(userName);
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string ErrorMessage
        {
            get { return Resources.Validation_Failed_UserName; }
        }
    }
}