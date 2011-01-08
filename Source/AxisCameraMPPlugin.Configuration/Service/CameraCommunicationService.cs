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
using AxisCameraMPPlugin.Data;

namespace AxisCameraMPPlugin.Configuration.Service
{
	/// <summary>
	/// Interface responsible for communicating with a camera.
	/// </summary>
	class CameraCommunicationService : ICameraCommunicationService, IDisposable
	{
		private const string ParameterCgi = "http://{0}:{1}/axis-cgi/admin/param.cgi?action=list&group=Network.UPnP.FriendlyName";
		private const string SnapshotCgi = "http://{0}:{1}/axis-cgi/jpg/image.cgi";

		private ClientState<string> parameterState;
		private ClientState<byte[]> snapshotState;

		private object syncRoot = new object();


		/// <summary>
		/// Occurs when the asynchronous getting information operation is completed, been canceled, or
		/// raised an exception.
		/// </summary>
		public event EventHandler<GetInformationFromCameraCompletedEventArgs> GetInformationFromCameraCompleted;


		/// <summary>
		/// Initializes a new instance of the <see cref="CameraCommunicationService"/> class.
		/// </summary>
		public CameraCommunicationService()
		{
			parameterState = new ClientState<string>();
			snapshotState = new ClientState<byte[]>();
		}


		/// <summary>
		/// Gets information from specified camera network endpoint asynchronously.
		/// </summary>
		/// <param name="networkEndpoint">The network endpoint.</param>
		public void GetInformationFromCameraAsync(NetworkEndpoint networkEndpoint)
		{
			if (networkEndpoint == null) throw new ArgumentNullException("networkEndpoint");
			if (IsBusy) throw new InvalidOperationException("Cannot start a second operation before first finishes.");

			IsBusy = true;

			// Download parameters
			parameterState.Client.DownloadStringCompleted += DownloadParametersCompleted;
			parameterState.Client.Credentials = new NetworkCredential(
				networkEndpoint.UserName,
				networkEndpoint.Password);
			Uri parameterUri = new Uri(ParameterCgi.InvariantFormat(
				networkEndpoint.Address,
				networkEndpoint.Port));
			parameterState.Client.DownloadStringAsync(parameterUri);

			// Download snapshot
			snapshotState.Client.DownloadDataCompleted += DownloadSnapshotCompleted;
			snapshotState.Client.Credentials = new NetworkCredential(
				networkEndpoint.UserName,
				networkEndpoint.Password);
			Uri snapshotUri = new Uri(SnapshotCgi.InvariantFormat(
				networkEndpoint.Address,
				networkEndpoint.Port));
			snapshotState.Client.DownloadDataAsync(snapshotUri);
		}


		/// <summary>
		/// Cancels an asynchronous getting information operation.
		/// </summary>
		public void CancelAsync()
		{
			parameterState.Client.CancelAsync();
			snapshotState.Client.CancelAsync();
		}


		/// <summary>
		/// Gets whether a getting information operation is in progress.
		/// </summary>
		public bool IsBusy { get; private set; }


		/// <summary>
		/// Raises the GetInformationFromCameraCompleted event.
		/// </summary>
		protected virtual void OnGetInformationFromCameraCompleted(
			GetInformationFromCameraCompletedEventArgs e)
		{
			EventHandler<GetInformationFromCameraCompletedEventArgs> handler =
				GetInformationFromCameraCompleted;

			if (handler != null)
			{
				handler(this, e);
			}
		}


		/// <summary>
		/// Callback when downloading the parameters completed.
		/// </summary>
		private void DownloadParametersCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			parameterState.Client.DownloadStringCompleted -= DownloadParametersCompleted;

			parameterState.IsCancelled = e.Cancelled;
			parameterState.Error = e.Error;
			parameterState.Result = e.Result;

			SendEventIfCompleted();
		}


		/// <summary>
		/// Callback when downloading the snapshot completed.
		/// </summary>
		private void DownloadSnapshotCompleted(object sender, DownloadDataCompletedEventArgs e)
		{
			snapshotState.Client.DownloadDataCompleted -= DownloadSnapshotCompleted;

			snapshotState.IsCancelled = e.Cancelled;
			snapshotState.Error = e.Error;
			snapshotState.Result = e.Result;

			SendEventIfCompleted();
		}


		/// <summary>
		/// Sends the GetInformationFromCameraCompleted event if completed.
		/// </summary>
		private void SendEventIfCompleted()
		{
			GetInformationFromCameraCompletedEventArgs e = null;

			lock (syncRoot)
			{
				if (parameterState.IsCompleted && snapshotState.IsCompleted)
				{
					// Cancelled has first priority
					if (parameterState.IsCancelled || snapshotState.IsCancelled)
					{
						e = new GetInformationFromCameraCompletedEventArgs(null, null, cancelled: true);
					}

					// Errors has second priority
					else if (parameterState.Error != null || snapshotState.Error != null)
					{
						e = new GetInformationFromCameraCompletedEventArgs(
							null,
							null,
							error: parameterState.Error ?? snapshotState.Error);
					}

					// Operation was successful
					else
					{
						e = new GetInformationFromCameraCompletedEventArgs(
							parameterState.Result,
							snapshotState.Result);
					}

					// Reset busy state
					IsBusy = false;
				}
			}

			if (e != null)
			{
				OnGetInformationFromCameraCompleted(e);
			}
		}


		/// <summary>
		/// Disposes the specified client.
		/// </summary>
		/// <param name="client">The client.</param>
		private static void DisposeClient(WebClient client)
		{
			if (client != null)
			{
				client.Dispose();
				client = null;
			}
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
		/// CameraCommunicationService is reclaimed by garbage collection.
		/// </summary>
		~CameraCommunicationService()
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
				DisposeClient(parameterState.Client);
				DisposeClient(snapshotState.Client);
			}
		}

		#endregion


		/// <summary>
		/// Class responsible for holding a web client state.
		/// </summary>
		/// <typeparam name="T">The result type.</typeparam>
		class ClientState<T>
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="ClientState"/> class.
			/// </summary>
			public ClientState()
			{
				Client = new WebClient();
			}


			/// <summary>
			/// Gets or sets the web client.
			/// </summary>
			public WebClient Client { get; set; }


			/// <summary>
			/// Gets or sets a value indicating whether this client was cancelled.
			/// </summary>
			public bool IsCancelled { get; set; }


			/// <summary>
			/// Gets or sets a value indicating which error occurred during an asynchronous operation.
			/// </summary>
			public Exception Error { get; set; }


			/// <summary>
			/// Gets or sets the result.
			/// </summary>
			public T Result { get; set; }


			/// <summary>
			/// Gets a value indicating whether this client is completed.
			/// </summary>
			public bool IsCompleted
			{
				get
				{
					return
						IsCancelled ||
						Error != null ||
						Result != null;
				}
			}
		}
	}
}
