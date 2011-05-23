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
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AxisCameras.Core.Contracts;

namespace AxisCameras.Configuration.Service
{
	/// <summary>
	/// Class containing methods wrapping WebClient in tasks.
	/// </summary>
	static class WebClientTasks
	{
		/// <summary>
		/// Downloads the resource specified as a Uri. This method does not block the calling thread.
		/// </summary>
		/// <param name="address">A Uri containing the URI to download.</param>
		/// <param name="credentials">
		/// The network credentials that are sent to the host and used to authenticate the request.
		/// Default value is null.
		/// </param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <param name="state">
		/// An object that is passed to the method invoked when the asynchronous operation completes.
		/// Default value is null.
		/// </param>
		/// <returns>A task downloading the resource specified as a Uri.</returns>
		public static Task<string> DownloadStringAsync(
			Uri address,
			NetworkCredential credentials = null,
			CancellationToken cancellationToken = default(CancellationToken),
			object state = null)
		{
			Requires.NotNull(address);

			var task = new TaskCompletionSource<string>(state);
			var webClient = new WebClient
			{
				Credentials = credentials
			};

			// Register a cancel handler, which will cancel the request if Cancel is triggered
			var ctr = cancellationToken.Register(() => webClient.CancelAsync());

			// Set up a completed event handler
			DownloadStringCompletedEventHandler handler = null;
			handler = (sender, e) =>
				{
					if (e.Cancelled)
					{
						task.TrySetCanceled();
					}
					else if (e.Error != null)
					{
						task.TrySetException(e.Error);
					}
					else
					{
						task.TrySetResult(e.Result);
					}

					webClient.DownloadStringCompleted -= handler;
					webClient.Dispose();
					ctr.Dispose();
				};

			// Register event handler
			webClient.DownloadStringCompleted += handler;

			// Start downloading
			webClient.DownloadStringAsync(address);

			return task.Task;
		}


		/// <summary>
		/// Downloads the specified resource as a Byte array. This method does not block the calling
		/// thread.
		/// </summary>
		/// <param name="address">A Uri containing the URI to download.</param>
		/// <param name="credentials">
		/// The network credentials that are sent to the host and used to authenticate the request.
		/// Default value is null.
		/// </param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <param name="state">
		/// An object that is passed to the method invoked when the asynchronous operation completes.
		/// Default value is null.
		/// </param>
		/// <returns>A task downloading the specified resource as a Byte array.</returns>
		public static Task<byte[]> DownloadDataAsync(
			Uri address,
			NetworkCredential credentials = null,
			CancellationToken cancellationToken = default(CancellationToken),
			object state = null)
		{
			var task = new TaskCompletionSource<byte[]>(state);
			var webClient = new WebClient
			{
				Credentials = credentials
			};

			// Register a cancel handler, which will cancel the request if Cancel is triggered
			var ctr = cancellationToken.Register(() => webClient.CancelAsync());

			// Set up a completed event handler
			DownloadDataCompletedEventHandler handler = null;
			handler = (sender, e) =>
			{
				if (e.Cancelled)
				{
					task.TrySetCanceled();
				}
				else if (e.Error != null)
				{
					task.TrySetException(e.Error);
				}
				else
				{
					task.TrySetResult(e.Result);
				}

				webClient.DownloadDataCompleted -= handler;
				webClient.Dispose();
				ctr.Dispose();
			};

			// Register event handler
			webClient.DownloadDataCompleted += handler;

			// Start downloading
			webClient.DownloadDataAsync(address);

			return task.Task;
		}
	}
}
