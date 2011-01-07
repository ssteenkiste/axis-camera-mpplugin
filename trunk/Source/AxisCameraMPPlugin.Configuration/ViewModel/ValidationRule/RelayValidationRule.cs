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
using AxisCameraMPPlugin.Mvvm.Validation;

namespace AxisCameraMPPlugin.Configuration.ViewModel.ValidationRule
{
	/// <summary>
	/// Validation rule that validates a specified relay function.
	/// </summary>
	class RelayValidationRule : IValidationRule
	{
		private readonly Func<bool> rule;


		/// <summary>
		/// Initializes a new instance of the <see cref="RelayValidationRule"/> class.
		/// </summary>
		/// <param name="rule">The validation rule function.</param>
		public RelayValidationRule(Func<bool> rule)
		{
			if (rule == null) throw new ArgumentNullException("rule");

			this.rule = rule;
		}


		/// <summary>
		/// Validates rule specified in construct.
		/// </summary>
		/// <param name="value">Is not used.</param>
		/// <returns>true if validation is successful; otherwise false.</returns>
		public bool Validate(object value)
		{
			return rule();
		}


		/// <summary>
		/// Gets or sets the error message.
		/// </summary>
		public string ErrorMessage { get; set; }
	}
}
