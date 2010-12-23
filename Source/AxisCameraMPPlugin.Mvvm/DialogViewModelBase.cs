using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AxisCameraMPPlugin.Mvvm
{
	/// <summary>
	/// Base class for all ViewModels representing a modal dialog.
	/// </summary>
	public abstract class DialogViewModelBase : ViewModelBase
	{
		/// <summary>
		/// Gets or sets the command responsible for OK click.
		/// </summary>
		public ICommand OkCommand { get; set; }


		/// <summary>
		/// Method inheriting classes can override to apply custom logic to prevent the dialog from
		/// closing when OK button is pressed.
		/// </summary>
		protected virtual bool OnOk
		{
			get { return true; }
		}


		#region Command methods

		/// <summary>
		/// Determins whether OK can execute.
		/// </summary>
		public bool CanOk(object parameter)
		{
			return Validate() && OnOk;
		}

		#endregion
	}
}
