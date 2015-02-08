using System;
using System.Diagnostics;
using AxisCamerasDependencyChecker.Dependencies.Helpers;

namespace AxisCamerasDependencyChecker.Dependencies
{
    /// <summary>
    /// Class checking the embedded Axis RTP source filter.
    /// </summary>
    internal class EmbeddedAxisRtpSourceFilter : IDependency
    {
        private const string Clsid = "67B1A88A-B5D2-48B1-BF93-EB74D6FCB077";
        private static readonly Version MinVersion = new Version(3, 0, 4, 2);

        private readonly Clsid clsid;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedAxisRtpSourceFilter"/> class.
        /// </summary>
        public EmbeddedAxisRtpSourceFilter()
        {
            clsid = new Clsid();
        }

        /// <summary>
        /// Runs verification of the dependency.
        /// </summary>
        /// <returns>The result of the dependency check.</returns>
        public DependencyResult Run()
        {
            var result = new DependencyResult("Embedded Axis RTP source filter");

            FileVersionInfo fileVersionInfo = clsid.GetVersionFromClsid(Clsid);

            if (fileVersionInfo != null)
            {
                // Add information
                result.AddInformation("File", fileVersionInfo.FileName);
                result.AddInformation("File version", fileVersionInfo.FileVersion);
                result.AddInformation("Product version", fileVersionInfo.ProductVersion);

                // Check minimum version
                var registeredVersion = new Version(fileVersionInfo.ProductVersion);
                if (registeredVersion >= MinVersion)
                {
                    result.IsSuccess = true;
                }
            }

            return result;
        }
    }
}