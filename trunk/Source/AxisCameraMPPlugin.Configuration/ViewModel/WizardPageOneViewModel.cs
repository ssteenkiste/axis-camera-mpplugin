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
using AxisCameraMPPlugin.Configuration.Properties;
using AxisCameraMPPlugin.Configuration.ViewModel.ValidationRule;

namespace AxisCameraMPPlugin.Configuration.ViewModel
{
	/// <summary>
	/// View model where camera network endpoint is entered.
	/// </summary>
	class WizardPageOneViewModel : WizardPageViewModel, IWizardPageOneViewModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WizardPageOneViewModel"/> class.
		/// </summary>
		public WizardPageOneViewModel()
		{
			AddValidators();

			// Default values
			Port = "80";
			Username = "root";
		}


		/// <summary>
		/// Gets the header of the wizard page.
		/// </summary>
		public override string Header
		{
			get { return Resources.WizardPageOne_Header; }
		}


		/// <summary>
		/// Gets the description of the wizard page.
		/// </summary>
		public override string Description
		{
			get { return Resources.WizardPageOne_Description; }
		}


		/// <summary>
		/// Gets or sets the address.
		/// </summary>
		public string Address
		{
			get { return Property(() => Address); }
			set { Property(() => Address, value); }
		}


		/// <summary>
		/// Gets or sets the HTTP port.
		/// </summary>
		public string Port
		{
			get { return Property(() => Port); }
			set { Property(() => Port, value); }
		}


		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		public string Username
		{
			get { return Property(() => Username); }
			set { Property(() => Username, value); }
		}


		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		public string Password
		{
			get { return Property(() => Password); }
			set { Property(() => Password, value); }
		}


		/// <summary>
		/// Adds the validators.
		/// </summary>
		private void AddValidators()
		{
			AddValidator(
				() => Address,
				new NotEmptyStringValidationRule { ErrorMessage = Resources.Validation_Failed_Address });
			AddValidator(
				() => Port,
				new PortValidationRule());
			AddValidator(
				() => Username,
				new UsernameValidationRule());
			AddValidator(
				() => Password,
				new NotEmptyStringValidationRule { ErrorMessage = Resources.Validation_Failed_Password });
		}
	}
}
