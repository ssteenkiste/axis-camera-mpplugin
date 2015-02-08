#region Copyright (C) 2005-2015 Team MediaPortal

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

using AxisCameras.Mvvm;
using NUnit.Framework;

namespace AxisCameras.MvvmTest
{
    [TestFixture]
    public class RelayCommandTest
    {
        [Test]
        public void CanExecuteTrue()
        {
            // ARRANGE
            var command = new RelayCommand(_ => { }, _ => true);

            // ACT
            bool canExecute = command.CanExecute(null);

            // ASSERT
            Assert.That(canExecute, Is.True);
        }

        [Test]
        public void CanExecuteFalse()
        {
            // ARRANGE
            var command = new RelayCommand(_ => { }, _ => false);

            // ACT
            bool canExecute = command.CanExecute(null);

            // ASSERT
            Assert.That(canExecute, Is.False);
        }

        [Test]
        public void CanExecuteParameterTrue()
        {
            // ARRANGE
            var command = new RelayCommand(_ => { }, parameter => (bool)parameter);

            // ACT
            bool canExecute = command.CanExecute(true);

            // ASSERT
            Assert.That(canExecute, Is.True);
        }

        [Test]
        public void CanExecuteParameterFalse()
        {
            // ARRANGE
            var command = new RelayCommand(_ => { }, parameter => (bool)parameter);

            // ACT
            bool canExecute = command.CanExecute(false);

            // ASSERT
            Assert.That(canExecute, Is.False);
        }

        [Test]
        public void CanExecuteNotSpecified()
        {
            // ARRANGE
            var command = new RelayCommand(_ => { });

            // ACT
            bool canExecute = command.CanExecute(null);

            // ASSERT
            Assert.That(canExecute, Is.True);
        }

        [Test]
        public void Execute()
        {
            // ARRANGE
            int result = 0;
            var command = new RelayCommand(_ => { result = 42; });
            
            // ACT
            command.Execute(null);

            // ASSERT
            Assert.That(result, Is.EqualTo(42));
        }

        [Test]
        public void ExecuteParameter()
        {
            // ARRANGE
            int result = 0;
            var command = new RelayCommand(parameter => { result = (int)parameter; });

            // ACT
            command.Execute(42);

            // ASSERT
            Assert.That(result, Is.EqualTo(42));
        }
    }
}