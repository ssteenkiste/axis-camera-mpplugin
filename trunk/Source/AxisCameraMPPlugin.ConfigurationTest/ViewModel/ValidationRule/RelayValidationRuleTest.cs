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
using AxisCameraMPPlugin.Configuration.ViewModel.ValidationRule;
using NUnit.Framework;


namespace AxisCameraMPPlugin.ConfigurationTest.ViewModel.ValidationRule
{
	[TestFixture]
	public class RelayValidationRuleTest
	{
		[Test]
		public void ValidationSuccessful()
		{
			RelayValidationRule validationRule = new RelayValidationRule(() => true);

			Assert.That(validationRule.Validate(null), Is.True);
			Assert.That(validationRule.Validate("some text"), Is.True);
			Assert.That(validationRule.Validate(new object()), Is.True);
		}


		[Test]
		public void ValidationFailed()
		{
			RelayValidationRule validationRule = new RelayValidationRule(() => false);

			Assert.That(validationRule.Validate(null), Is.False);
			Assert.That(validationRule.Validate("some text"), Is.False);
			Assert.That(validationRule.Validate(new object()), Is.False);
		}
	}
}
