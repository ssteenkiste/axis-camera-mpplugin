using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace AxisCameraMPPlugin.Mvvm.Services.FrameworkDialogs.OpenFile
{
	/// <summary>
	/// Interface describing the OpenFileDialog.
	/// </summary>
	public interface IOpenFileDialog : IFileDialog
	{
		/// <summary>
		/// Gets or sets a value indicating whether the dialog box allows multiple files to be
		/// selected.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly",
			MessageId = "Multiselect",
			Justification = "The syntax is a replica of System.Windows.Forms.OpenFileDialog.")]
		bool Multiselect { get; set; }
	}
}
