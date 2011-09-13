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
using System;
using System.Diagnostics.CodeAnalysis;
using AxisCameras.Core.Contracts;

namespace MediaPortal.Dialogs
{
	/// <summary>
	/// Class containing extensions for the GUIDialogOK class.
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "GUI",
		Justification = "MediaPortal has named the control like this.")]
	[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OK",
		Justification = "MediaPortal has named the control like this.")]
	public static class GUIDialogOKExtensions
	{
		/// <summary>
		/// Is setting the lines of the GUIDialogOK.
		/// </summary>
		/// <param name="dialog">The OK dialog to set the lines on.</param>
		/// <param name="message">
		/// The message displayed in the OK dialog. Each line is split on "\n" or "\\n".
		/// </param>
		[SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "SetLines",
			Justification = "Method setting single line is called SetLine, only appropriate to call this method SetLines.")]
		public static void SetLines(this GUIDialogOK dialog, string message)
		{
			Requires.IsNotNullOrEmpty(message);

			string[] lines = message.Split(new[] { "\n", "\\n" }, StringSplitOptions.None);

			// The GUIDialogOK supports maximum 4 lines
			int maxLineCount = 4;
			if (lines.Length > maxLineCount)
			{
				string errorMessageFormat =
					"The GUIDialogOK dialog supports maximum {0} lines, the specified message has {1}.";
				string errorMessage = errorMessageFormat.InvariantFormat(maxLineCount, lines.Length);

				Requires.Fail(errorMessage);
			}

			for (int row = 1; row <= Math.Min(lines.Length, maxLineCount); row++)
			{
				dialog.SetLine(row, lines[row - 1]);
			}
		}
	}
}
