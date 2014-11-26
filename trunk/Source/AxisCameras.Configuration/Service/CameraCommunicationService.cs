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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AxisCameras.Core;
using AxisCameras.Core.Contracts;
using AxisCameras.Data;

namespace AxisCameras.Configuration.Service
{
    /// <summary>
    /// Interface responsible for communicating with a camera.
    /// </summary>
    internal class CameraCommunicationService : ICameraCommunicationService
    {
        private readonly IParameterParser parameterParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraCommunicationService"/> class.
        /// </summary>
        /// <param name="parameterParser">The parameter parser.</param>
        public CameraCommunicationService(IParameterParser parameterParser)
        {
            Requires.NotNull(parameterParser);

            this.parameterParser = parameterParser;
        }

        /// <summary>
        /// Gets camera parameters from specified camera network endpoint asynchronously.
        /// </summary>
        /// <param name="networkEndpoint">The network endpoint.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task getting camera parameters.</returns>
        public Task<CameraParameters> GetCameraParametersAsync(
            NetworkEndpoint networkEndpoint,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Requires.NotNull(networkEndpoint);

            Log.Debug(
                "Getting camera parameters from {0}:{1}",
                networkEndpoint.Address,
                networkEndpoint.Port);

            var parameterUri = new Uri(
                Vapix.Cgi.Parameter.InvariantFormat(
                    networkEndpoint.Address,
                    networkEndpoint.Port,
                    string.Join(
                        ",",
                        new[]
                        {
                            Vapix.Parameters.FriendlyName,
                            Vapix.Parameters.FirmwareVersion,
                            Vapix.Parameters.NbrOfImageSources,
                            Vapix.Parameters.ImageFormats
                        })));

            // Task getting parameters
            Task<string> getParametersTask = WebClientTasks.DownloadStringAsync(
                parameterUri,
                new NetworkCredential(networkEndpoint.UserName, networkEndpoint.Password),
                cancellationToken);

            // Task parsing result
            return getParametersTask.ContinueWith(
                t =>
                {
                    if (t.IsFaulted)
                    {
                        Log.Error("Error when getting parameters.", t.Exception.InnerException);
                        throw t.Exception.InnerException;
                    }

                    // Parse result
                    IDictionary<string, string> parameters = parameterParser.Parse(t.Result);

                    string friendlyName = GetValue(Vapix.Parameters.FriendlyName, parameters);
                    string firmwareVersion = GetValue(Vapix.Parameters.FirmwareVersion, parameters);
                    int videoSourceCount = GetValueAsInteger(Vapix.Parameters.NbrOfImageSources, parameters);
                    VideoCapabilities videoCapabilities = GetVideoCapabilities(parameters);

                    Log.Debug(
                        "Get camera parameters from camera completed. [{0}|{1}|{2}|{3}]",
                        friendlyName,
                        firmwareVersion,
                        videoSourceCount,
                        videoCapabilities);

                    return new CameraParameters(
                        friendlyName,
                        firmwareVersion,
                        videoSourceCount,
                        videoCapabilities);
                });
        }

        /// <summary>
        /// Gets camera snapshot from specified camera network endpoint asynchronously.
        /// </summary>
        /// <param name="networkEndpoint">The network endpoint.</param>
        /// <param name="videoSource">The video source to get snapshot from.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task getting a camera snapshot.</returns>
        public Task<byte[]> GetSnapshotAsync(
            NetworkEndpoint networkEndpoint,
            int videoSource,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Requires.NotNull(networkEndpoint);
            Requires.True(videoSource >= 1, "Video source must be 1 or greater.");

            Log.Debug(
                "Getting camera parameters from {0}:{1}, video source {2}",
                networkEndpoint.Address,
                networkEndpoint.Port,
                videoSource);

            var snapshotUri = new Uri(
                Vapix.Cgi.Snapshot.InvariantFormat(
                    networkEndpoint.Address,
                    networkEndpoint.Port,
                    videoSource));

            return WebClientTasks.DownloadDataAsync(
                snapshotUri,
                new NetworkCredential(networkEndpoint.UserName, networkEndpoint.Password),
                cancellationToken);
        }

        /// <summary>
        /// Gets a value with specified name from specified dictionary of parameter name/value pairs.
        /// </summary>
        /// <returns>The parameter value if existing; otherwise null.</returns>
        private static string GetValue(string parameterName, IDictionary<string, string> parameters)
        {
            string parameterValue;
            if (parameters.TryGetValue(parameterName, out parameterValue))
            {
                return parameterValue;
            }

            return null;
        }

        /// <summary>
        /// Gets a value with specified name from specified dictionary of parameter name/value pairs.
        /// </summary>
        /// <returns>The parameter value if existing; otherwise 0.</returns>
        private static int GetValueAsInteger(
            string parameterName,
            IDictionary<string, string> parameters)
        {
            string parameterValueText = GetValue(parameterName, parameters);
            if (parameterValueText != null)
            {
                int parameterValue;
                if (int.TryParse(parameterValueText, out parameterValue))
                {
                    return parameterValue;
                }
            }

            return 0;
        }

        /// <summary>
        /// Gets the video capabilities from specified dictionary of parameter name/value pairs.
        /// </summary>
        /// <returns>The supported video capabilities.</returns>
        private static VideoCapabilities GetVideoCapabilities(IDictionary<string, string> parameters)
        {
            VideoCapabilities capabilities = VideoCapabilities.None;

            string parameterValueText = GetValue(Vapix.Parameters.ImageFormats, parameters);
            if (parameterValueText != null)
            {
                string[] videoFormats = parameterValueText.Split(',');

                if (videoFormats.Contains(Vapix.Values.H264))
                {
                    capabilities |= VideoCapabilities.H264;
                }

                if (videoFormats.Contains(Vapix.Values.Mpeg4))
                {
                    capabilities |= VideoCapabilities.Mpeg4;
                }

                if (videoFormats.Contains(Vapix.Values.Mjpeg))
                {
                    capabilities |= VideoCapabilities.Mjpeg;
                }
            }

            return capabilities;
        }
    }
}