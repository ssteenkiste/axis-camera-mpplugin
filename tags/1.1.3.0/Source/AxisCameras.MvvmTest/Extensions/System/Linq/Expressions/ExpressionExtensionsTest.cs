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

using System;
using System.Linq.Expressions;
using NUnit.Framework;

namespace AxisCameras.MvvmTest.Extensions.System.Linq.Expressions
{
    [TestFixture]
    public class ExpressionExtensionsTest
    {
        [Test]
        public void GetNameFromExpression()
        {
            // ARRANGE
            Expression<Func<Person, string>> expression = person => person.FirstName;

            // ACT
            string propertyName = expression.GetName();

            // ASSERT
            Assert.That(propertyName, Is.EqualTo("FirstName"));
        }

        [Test]
        public void GetNameFromUnaryExpression()
        {
            // ARRANGE
            Expression<Func<Person, object>> expression = person => person.Age;

            // ACT
            string propertyName = expression.GetName();

            // ASSERT
            Assert.That(propertyName, Is.EqualTo("Age"));
        }

        #region Helper classes
        
        private class Person
        {
            public string FirstName { get; set; }

            public int Age { get; set; }
        }

        #endregion
    }
}