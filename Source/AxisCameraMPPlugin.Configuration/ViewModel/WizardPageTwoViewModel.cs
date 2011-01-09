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
using AxisCameraMPPlugin.Configuration.Properties;
using AxisCameraMPPlugin.Configuration.ViewModel.ValidationRule;
using AxisCameraMPPlugin.Data;

namespace AxisCameraMPPlugin.Configuration.ViewModel
{
	/// <summary>
	/// View model where camera name is entered and snapshot displayed.
	/// </summary>
	class WizardPageTwoViewModel : WizardPageViewModel, IWizardPageTwoViewModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WizardPageTwoViewModel"/> class.
		/// </summary>
		public WizardPageTwoViewModel()
		{
			AddValidators();
		}


		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name
		{
			get { return Property(() => Name); }
			set { Property(() => Name, value); }
		}


		/// <summary>
		/// Gets the snapshot path.
		/// </summary>
		public string SnapshotPath
		{
			get { return Property(() => SnapshotPath); }
			private set { Property(() => SnapshotPath, value); }
		}


		/// <summary>
		/// Gets the header of the wizard page.
		/// </summary>
		public override string Header
		{
			get { return Resources.WizardPageTwo_Header; }
		}


		/// <summary>
		/// Gets the description of the wizard page.
		/// </summary>
		public override string Description
		{
			get { return Resources.WizardPageTwo_Description; }
		}


		/// <summary>
		/// Loads page properties from specified camera.
		/// </summary>
		/// <param name="camera">The camera to load page properties from.</param>
		public override void Load(Camera camera)
		{
			if (camera == null) throw new ArgumentNullException("camera");

			Name = camera.Name;
			SnapshotPath = camera.SnapshotPath;
		}


		/// <summary>
		/// Saves page properties to specified camera.
		/// </summary>
		/// <param name="camera">The camera to save page properties to.</param>
		public override void Save(Camera camera)
		{
			if (camera == null) throw new ArgumentNullException("camera");

			camera.Name = Name;
			camera.SnapshotPath = SnapshotPath;
		}


		/// <summary>
		/// Adds the validators.
		/// </summary>
		private void AddValidators()
		{
			AddValidator(
				() => Name,
				new NotEmptyStringValidationRule { ErrorMessage = Resources.Validation_Failed_CameraName });
		}
	}
}
