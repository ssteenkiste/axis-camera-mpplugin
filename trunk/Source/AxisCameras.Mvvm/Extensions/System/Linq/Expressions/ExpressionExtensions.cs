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

// ReSharper disable once CheckNamespace
namespace System.Linq.Expressions
{
    /// <summary>
    /// Class containing Expression extensions.
    /// </summary>
    internal static class ExpressionExtensions
    {
        /// <summary>
        /// Gets the name of the expression.
        /// </summary>
        /// <typeparam name="T">The property type.</typeparam>
        /// <param name="expression">The name expression.</param>
        /// <returns>The name of the expression.</returns>
        public static string GetName<T>(this Expression<T> expression)
        {
            var unaryExpression = expression.Body as UnaryExpression;

            // Convert name expression into MemberExpression
            MemberExpression memberExpression = unaryExpression != null
                ? (MemberExpression)unaryExpression.Operand
                : (MemberExpression)expression.Body;

            return memberExpression.Member.Name;
        }
    }
}