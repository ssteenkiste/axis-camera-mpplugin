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
using AxisCameraMPPlugin.Data;

namespace AxisCameraMPPlugin.Configuration.Service
{
	/// <summary>
	/// Interface responsible for communicating with a camera.
	/// </summary>
	interface ICameraCommunicationService
	{
		/// <summary>
		/// Occurs when the asynchronous getting information operation is completed, been canceled, or
		/// raised an exception.
		/// </summary>
		event EventHandler<GetInformationFromCameraCompletedEventArgs> GetInformationFromCameraCompleted;


		/// <summary>
		/// Gets information from specified camera network endpoint asynchronously.
		/// </summary>
		/// <param name="networkEndpoint">The network endpoint.</param>
		void GetInformationFromCameraAsync(NetworkEndpoint networkEndpoint);


		/// <summary>
		/// Cancels an asynchronous getting information operation.
		/// </summary>
		void CancelAsync();


		/// <summary>
		/// Gets whether a getting information operation is in progress.
		/// </summary>
		bool IsBusy { get; }
	}
}
