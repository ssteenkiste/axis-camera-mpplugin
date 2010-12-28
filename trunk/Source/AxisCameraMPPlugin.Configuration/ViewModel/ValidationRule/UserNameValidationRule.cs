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
using System.Text.RegularExpressions;
using AxisCameraMPPlugin.Configuration.Properties;
using AxisCameraMPPlugin.Mvvm.Validation;

namespace AxisCameraMPPlugin.Configuration.ViewModel.ValidationRule
{
	/// <summary>
	/// Validation rule that validates the username of a camera.
	/// </summary>
	class UserNameValidationRule : IValidationRule
	{
		private readonly Regex usernameRegex;


		/// <summary>
		/// Initializes a new instance of the <see cref="UserNameValidationRule"/> class.
		/// </summary>
		public UserNameValidationRule()
		{
			usernameRegex = new Regex("^[A-Za-z]+[A-Za-z0-9]*$");
		}

		
		/// <summary>
		/// Validates the specified username.
		/// </summary>
		/// <param name="value">The value to validate.</param>
		/// <returns>true if validation is successful; otherwise false.</returns>
		public bool Validate(object value)
		{
			string username = value as string;
			if (string.IsNullOrEmpty(username))
			{
				return false;
			}

			if (username.Length > 14)
			{
				return false;
			}

			// Make sure a user name starts with a letter and only contains letters and digits
			return usernameRegex.IsMatch(username);
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
