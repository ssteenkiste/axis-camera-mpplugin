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
using AxisCameras.Configuration.ViewModel.ValidationRule;
using NUnit.Framework;

namespace AxisCameras.ConfigurationTest.ViewModel.ValidationRule
{
	[TestFixture]
	public class PortValidationRuleTest
	{
		[Test]
		public void ValidationSuccessful()
		{
			PortValidationRule validationRule = new PortValidationRule();

			Assert.That(validationRule.Validate("1"), Is.True);
			Assert.That(validationRule.Validate("2"), Is.True);
			Assert.That(validationRule.Validate("65534"), Is.True);
			Assert.That(validationRule.Validate("65535"), Is.True);
		}


		[Test]
		public void ValidationFailed()
		{
			PortValidationRule validationRule = new PortValidationRule();

			Assert.That(validationRule.Validate("-1"), Is.False);
			Assert.That(validationRule.Validate("0"), Is.False);
			Assert.That(validationRule.Validate("65536"), Is.False);
			Assert.That(validationRule.Validate("65537"), Is.False);
			Assert.That(validationRule.Validate(null), Is.False);
			Assert.That(validationRule.Validate(string.Empty), Is.False);
			Assert.That(validationRule.Validate(new object()), Is.False);
		}
	}
}
