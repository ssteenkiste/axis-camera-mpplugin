#region Copyright (C) 2005-2014 Team MediaPortal

// Copyright (C) 2005-2014 Team MediaPortal
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

using System.Linq;
using AxisCameras.Mvvm.Validation;
using NUnit.Framework;
using System.Collections.Generic;

namespace AxisCameras.MvvmTest.Validation
{
    [TestFixture]
    public class ValidatorTest
    {
        private Validator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new Validator();
        }

        [Test]
        public void Empty()
        {
            // ACT
            bool isValid = validator.Validate();
            
            // ASSERT
            Assert.That(isValid, Is.True);

            Assert.That(validator.IsValid, Is.True);
            Assert.That(validator.InvalidPropertyNames, Is.Empty);
        }

        [Test]
        public void ErrorMessagesForUnknownProperty()
        {
            // ACT
            bool isValid = validator.Validate();
            IEnumerable<string> errorMessages = validator.GetErrorMessagesFor("UnknownProperty");
            
            // ASSERT
            Assert.That(isValid, Is.True);
            Assert.That(errorMessages, Is.Empty);

            Assert.That(validator.IsValid, Is.True);
            Assert.That(validator.InvalidPropertyNames, Is.Empty);
        }

        [Test]
        public void ValidateOneSuccessfull()
        {
            // ARRANGE
            validator.Add(() => Username, new SucceedingValidationRule());

            // ACT
            bool isValid = validator.Validate();
            IEnumerable<string> errorMessages = validator.GetErrorMessagesFor("Username");

            // ASSERT
            Assert.That(isValid, Is.True);
            Assert.That(errorMessages, Is.Empty);

            Assert.That(validator.IsValid, Is.True);
            Assert.That(validator.InvalidPropertyNames, Is.Empty);
        }

        [Test]
        public void ValidateOneUnsuccessful()
        {
            // ARRANGE
            validator.Add(() => Username, new FailingValidationRule());

            // ACT
            bool isValid = validator.Validate();
            IEnumerable<string> errorMessages = validator.GetErrorMessagesFor("Username");

            // ASSERT
            Assert.That(isValid, Is.False);
            Assert.That(errorMessages.Count(), Is.EqualTo(1));

            Assert.That(validator.IsValid, Is.False);
            CollectionAssert.AreEqual(new[] { "Username" }, validator.InvalidPropertyNames);
        }

        [Test]
        public void ValidateOneSuccessfullAndTwoUnsuccessful()
        {
            // ARRANGE
            validator.Add(() => Username, new SucceedingValidationRule());
            validator.Add(() => Username, new FailingValidationRule());
            validator.Add(() => Username, new FailingValidationRule());

            // ACT
            bool isValid = validator.Validate();
            IEnumerable<string> errorMessages = validator.GetErrorMessagesFor("Username");

            // ASSERT
            Assert.That(isValid, Is.False);
            Assert.That(errorMessages.Count(), Is.EqualTo(2));

            Assert.That(validator.IsValid, Is.False);
            CollectionAssert.AreEqual(new[] { "Username" }, validator.InvalidPropertyNames);
        }

        [Test]
        public void AddMoreTests()
        {
            // ARRANGE

            // ACT

            // ASSERT
            Assert.Fail();
        }

        #region Helper properties and classes

        public string Username { get; set; }

        public string Password { get; set; }

        private class SucceedingValidationRule : IValidationRule
        {
            public string ErrorMessage
            {
                get { return "Error!"; }
            }

            public bool Validate(object value)
            {
                return true;
            }
        }

        private class FailingValidationRule : IValidationRule
        {
            public string ErrorMessage
            {
                get { return "Error!"; }
            }

            public bool Validate(object value)
            {
                return false;
            }
        }

        #endregion
    }
}