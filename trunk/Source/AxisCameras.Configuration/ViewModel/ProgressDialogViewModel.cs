#region Copyright (C) 2005-2011 Team MediaPortal

// Copyright (C) 2005-2011 Team MediaPortal
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
using System.Threading;
using System.Windows.Input;
using AxisCameras.Mvvm;

namespace AxisCameras.Configuration.ViewModel
{
	/// <summary>
	/// Abstract progress dialog view model.
	/// </summary>
	abstract class ProgressDialogViewModel : DialogViewModelBase, IDisposable
	{
		private CancellationTokenSource cancellationTokenSource;


		/// <summary>
		/// Initializes a new instance of the <see cref="ProgressDialogViewModel"/> class.
		/// </summary>
		protected ProgressDialogViewModel()
		{
			cancellationTokenSource = new CancellationTokenSource();

			LoadedCommand = new RelayCommand(Loaded);
			CancelCommand = new RelayCommand(Cancel);
		}


		/// <summary>
		/// Gets the loaded command.
		/// </summary>
		public ICommand LoadedCommand
		{
			get { return Property(() => LoadedCommand); }
			private set { Property(() => LoadedCommand, value); }
		}


		/// <summary>
		/// Gets the command canceling the dialog.
		/// </summary>
		public ICommand CancelCommand
		{
			get { return Property(() => CancelCommand); }
			private set { Property(() => CancelCommand, value); }
		}


		/// <summary>
		/// Gets a cancellation token.
		/// </summary>
		protected CancellationToken CancellationToken
		{
			get { return cancellationTokenSource.Token; }
		}


		/// <summary>
		/// Executes the Loaded command.
		/// </summary>
		protected abstract void Loaded(object parameter);


		/// <summary>
		/// Executes the Cancel command.
		/// </summary>
		private void Cancel(object parameter)
		{
			cancellationTokenSource.Cancel();
		}


		#region IDisposable Members

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting
		/// unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}


		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// FolderBrowserDialog is reclaimed by garbage collection.
		/// </summary>
		~ProgressDialogViewModel()
		{
			Dispose(false);
		}


		/// <summary>
		/// Releases unmanaged and optionally managed resources.
		/// </summary>
		/// <param name="disposing">
		/// true to release both managed and unmanaged resources; false to release only unmanaged
		/// resources.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (cancellationTokenSource != null)
				{
					cancellationTokenSource.Dispose();
					cancellationTokenSource = null;
				}
			}
		}

		#endregion
	}
}
