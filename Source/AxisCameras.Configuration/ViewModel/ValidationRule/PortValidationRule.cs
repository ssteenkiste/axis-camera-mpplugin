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
using AxisCameras.Configuration.Properties;
using AxisCameras.Mvvm.Validation;

namespace AxisCameras.Configuration.ViewModel.ValidationRule
{
	/// <summary>
	/// Validation rule that validates a HTTP port.
	/// </summary>
	class PortValidationRule : IValidationRule
	{
		/// <summary>
		/// Validates that specified value is a HTTP port.
		/// </summary>
		/// <param name="value">The value to validate.</param>
		/// <returns>true if validation is successful; otherwise false.</returns>
		public bool Validate(object value)
		{
			string portText = value as string;
			if (string.IsNullOrEmpty(portText))
			{
				return false;
			}

			int port;
			if (!int.TryParse(portText, out port))
			{
				return false;
			}

			return port > 0 && port < 65536;
		}


		/// <summary>
		/// Gets the error message.
		/// </summary>
		public string ErrorMessage
		{
			get { return Resources.Validation_Failed_Port; }
		}
	}
}
