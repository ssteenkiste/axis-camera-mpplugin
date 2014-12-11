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

using AxisCameras.Mvvm;
using AxisCameras.Mvvm.Validation;
using NUnit.Framework;

namespace AxisCameras.MvvmTest
{
    [TestFixture]
    public class ViewModelBaseTest
    {
        [Test]
        public void AddMoreTests()
        {
            // ARRANGE

            // ACT

            // ASSERT
            Assert.Fail();
        }

        #region Helper classes and methods

        private class ViewModel : ViewModelBase
        {
            public ViewModel()
            {
                AddValidator(() => Username, new NotEmptyOrNullValidationRule());
            }

            public string Username
            {
                get { return GetValue<string>(); }
                set { SetValue(value); }
            }
        }

        private class NotEmptyOrNullValidationRule : IValidationRule
        {
            public string ErrorMessage
            {
                get { return "Validation failed!"; }
            }

            public bool Validate(object value)
            {
                return !string.IsNullOrEmpty(value as string);
            }
        }

        #endregion
    }
}