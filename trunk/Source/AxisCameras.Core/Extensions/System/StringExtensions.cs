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

using System.Globalization;

namespace System
{
    /// <summary>
    /// Class containing extensions for the String class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Replaces the format item in a specified String with the text equivalent of the value of a
        /// corresponding invariant culture Object instance in a specified array.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An Object array containing zero or more objects to format.</param>
        /// <returns>
        /// A copy of format in which the format items have been replaced by the String equivalent of
        /// the corresponding instances of Object in args.
        /// </returns>
        public static string InvariantFormat(this string format, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }

        /// <summary>
        /// Replaces the format item in a specified String with the text equivalent of the value of a
        /// corresponding current culture Object instance in a specified array.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An Object array containing zero or more objects to format.</param>
        /// <returns>
        /// A copy of format in which the format items have been replaced by the String equivalent of
        /// the corresponding instances of Object in args.
        /// </returns>
        public static string CurrentFormat(this string format, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }
    }
}