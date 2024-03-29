﻿#region Copyright (C) 2005-2015 Team MediaPortal

// Copyright (C) 2005-2015 Team MediaPortal
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
    public class NotEmptyStringValidationRuleTest
    {
        private NotEmptyStringValidationRule rule;

        [SetUp]
        public void SetUp()
        {
            rule = new NotEmptyStringValidationRule();
        }

        [Test]
        public void ValidationSuccessful()
        {
            // ASSERT
            Assert.That(rule.Validate("some text"), Is.True);
            Assert.That(rule.Validate("123"), Is.True);
        }

        [Test]
        public void ValidationFailed()
        {
            // ASSERT
            Assert.That(rule.Validate(null), Is.False);
            Assert.That(rule.Validate(string.Empty), Is.False);
            Assert.That(rule.Validate(new object()), Is.False);
        }
    }
}