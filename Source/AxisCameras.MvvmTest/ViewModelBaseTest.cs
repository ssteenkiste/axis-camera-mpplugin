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

using System.Collections.Generic;
using System.ComponentModel;
using AxisCameras.Mvvm;
using AxisCameras.Mvvm.Validation;
using NUnit.Framework;
using Test;

namespace AxisCameras.MvvmTest
{
    [TestFixture]
    public class ViewModelBaseTest
    {
        private PersonViewModel personViewModel;

        [SetUp]
        public void SetUp()
        {
            personViewModel = new PersonViewModel();
        }

        [Test]
        public void SetNewValueOnProperty()
        {
            // ARRANGE
            var eventAsserter = new EventAsserter();
            personViewModel.PropertyChanged += eventAsserter.Handler;
            
            // ACT
            personViewModel.Username = "John Doe";

            // ASSERT
            eventAsserter.AssertEventReceived();
            Assert.That(eventAsserter.Count, Is.EqualTo(1));
            
            var propertyChangedEventArgs = eventAsserter.Dequeue<PropertyChangedEventArgs>();
            Assert.That(propertyChangedEventArgs.PropertyName, Is.EqualTo("Username"));
        }

        [Test]
        public void SetSameValueOnProperty()
        {
            // ARRANGE
            personViewModel.Username = "John Doe";

            var eventAsserter = new EventAsserter();
            personViewModel.PropertyChanged += eventAsserter.Handler;

            // ACT
            personViewModel.Username = "John Doe";

            // ASSERT
            eventAsserter.AssertNoEventReceived();
            Assert.That(eventAsserter.IsEmpty, Is.True);
        }

        [Test]
        public void Valid()
        {
            // ARRANGE
            personViewModel.Username = "John Doe";
            personViewModel.Password = "secure";

            // ACT
            bool isValid = personViewModel.Validate();

            // ASSERT
            Assert.That(isValid, Is.True);

            Assert.That(personViewModel.IsValid, Is.True);
            Assert.That(personViewModel.Error, Is.EqualTo(string.Empty));
            Assert.That(personViewModel["Username"], Is.EqualTo(string.Empty));
            Assert.That(personViewModel["Password"], Is.EqualTo(string.Empty));
        }

        [Test]
        public void Invalid()
        {
            // ARRANGE
            personViewModel.Username = string.Empty;
            personViewModel.Password = string.Empty;

            // ACT
            bool isValid = personViewModel.Validate();

            // ASSERT
            Assert.That(isValid, Is.False);

            Assert.That(personViewModel.IsValid, Is.False);
            Assert.That(personViewModel.Error, Is.EqualTo(string.Empty));
            Assert.That(personViewModel["Username"], Is.EqualTo(NotEmptyOrNullValidationRule.DefaultErrorMessage));
            Assert.That(personViewModel["Password"], Is.EqualTo(NotEmptyOrNullValidationRule.DefaultErrorMessage));
        }

        [Test]
        public void ValidWhenNoValidators()
        {
            // ARRANGE
            var viewModelWithNoValidators = new ViewModelWithNoValidators();
            
            // ACT
            bool isValid = viewModelWithNoValidators.Validate();

            // ASSERT
            Assert.That(isValid, Is.True);

            Assert.That(viewModelWithNoValidators.IsValid, Is.True);
            Assert.That(viewModelWithNoValidators.Error, Is.EqualTo(string.Empty));
        }

        [Test]
        public void PropertyChangedUsingPropertyChangedEventArgs()
        {
            // ARRANGE
            var eventAsserter = new EventAsserter();
            personViewModel.PropertyChanged += eventAsserter.Handler;

            // ACT
            personViewModel.FirePropertyChanged(new PropertyChangedEventArgs("Username"));

            // ASSERT
            eventAsserter.AssertEventReceived();
            Assert.That(eventAsserter.Count, Is.EqualTo(1));

            var propertyChangedEventArgs = eventAsserter.Dequeue<PropertyChangedEventArgs>();
            Assert.That(propertyChangedEventArgs.PropertyName, Is.EqualTo("Username"));
        }

        [Test]
        public void PropertyChangedUsingExpression()
        {
            // ARRANGE
            var eventAsserter = new EventAsserter();
            personViewModel.PropertyChanged += eventAsserter.Handler;

            // ACT
            personViewModel.FireUsernamePropertyChanged();

            // ASSERT
            eventAsserter.AssertEventReceived();
            Assert.That(eventAsserter.Count, Is.EqualTo(1));

            var propertyChangedEventArgs = eventAsserter.Dequeue<PropertyChangedEventArgs>();
            Assert.That(propertyChangedEventArgs.PropertyName, Is.EqualTo("Username"));
        }

        [Test]
        public void PropertyChangedUsingString()
        {
            // ARRANGE
            var eventAsserter = new EventAsserter();
            personViewModel.PropertyChanged += eventAsserter.Handler;

            // ACT
            personViewModel.FirePropertyChanged("Username");

            // ASSERT
            eventAsserter.AssertEventReceived();
            Assert.That(eventAsserter.Count, Is.EqualTo(1));

            var propertyChangedEventArgs = eventAsserter.Dequeue<PropertyChangedEventArgs>();
            Assert.That(propertyChangedEventArgs.PropertyName, Is.EqualTo("Username"));
        }

        [Test]
        public void PropertiesChanged()
        {
            // ARRANGE
            var eventAsserter = new EventAsserter();
            personViewModel.PropertyChanged += eventAsserter.Handler;

            // ACT
            personViewModel.FirePropertiesChanged(new[] { "Username", "Password" });

            // ASSERT
            eventAsserter.AssertEventReceived();
            Assert.That(eventAsserter.Count, Is.EqualTo(2));

            var propertyChangedEventArgs = eventAsserter.Dequeue<PropertyChangedEventArgs>();
            Assert.That(propertyChangedEventArgs.PropertyName, Is.EqualTo("Username"));

            propertyChangedEventArgs = eventAsserter.Dequeue<PropertyChangedEventArgs>();
            Assert.That(propertyChangedEventArgs.PropertyName, Is.EqualTo("Password"));
        }

        [Test]
        public void AllPropertiesChanged()
        {
            // ARRANGE
            var eventAsserter = new EventAsserter();
            personViewModel.PropertyChanged += eventAsserter.Handler;

            // ACT
            personViewModel.FireAllPropertiesChanged();

            // ASSERT
            eventAsserter.AssertEventReceived();
            Assert.That(eventAsserter.Count, Is.EqualTo(1));

            var propertyChangedEventArgs = eventAsserter.Dequeue<PropertyChangedEventArgs>();
            Assert.That(propertyChangedEventArgs.PropertyName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void IsLoadedUndertermined()
        {
            // ACT
            bool? isLoaded = personViewModel.IsLoaded;

            // ASSERT
            Assert.That(isLoaded, Is.Null);
        }

        [Test]
        public void IsLoaded()
        {
            // ARRANGE
            personViewModel.LoadedCommand.Execute(null);

            // ACT
            bool? isLoaded = personViewModel.IsLoaded;

            // ASSERT
            Assert.That(isLoaded, Is.True);
        }

        [Test]
        public void IsNotLoaded()
        {
            // ARRANGE
            personViewModel.UnloadedCommand.Execute(null);

            // ACT
            bool? isLoaded = personViewModel.IsLoaded;

            // ASSERT
            Assert.That(isLoaded, Is.False);
        }

        #region Helper classes and methods

        private class ViewModelWithNoValidators : ViewModelBase
        {
        }

        private class PersonViewModel : ViewModelBase
        {
            public PersonViewModel()
            {
                AddValidator(() => Username, new NotEmptyOrNullValidationRule());
                AddValidator(() => Password, new NotEmptyOrNullValidationRule());
            }

            public string Username
            {
                get { return GetValue<string>(); }
                set { SetValue(value); }
            }

            public string Password
            {
                get { return GetValue<string>(); }
                set { SetValue(value); }
            }

            public void FirePropertyChanged(PropertyChangedEventArgs e)
            {
                OnPropertyChanged(e);
            }

            public void FireUsernamePropertyChanged()
            {
                OnPropertyChanged(() => Username);
            }

            public void FirePropertyChanged(string propertyName)
            {
                OnPropertyChanged(propertyName);
            }

            public void FirePropertiesChanged(IEnumerable<string> propertyNames)
            {
                OnPropertiesChanged(propertyNames);
            }

            public void FireAllPropertiesChanged()
            {
                OnAllPropertiesChanged();
            }
        }

        private class NotEmptyOrNullValidationRule : IValidationRule
        {
            public const string DefaultErrorMessage = "Validation failed!";

            public string ErrorMessage
            {
                get { return DefaultErrorMessage; }
            }

            public bool Validate(object value)
            {
                return !string.IsNullOrEmpty(value as string);
            }
        }

        #endregion
    }
}