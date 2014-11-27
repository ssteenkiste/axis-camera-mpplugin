using System;
using System.Diagnostics;
using AxisCamerasDependencyChecker.Dependencies.Helpers;

namespace AxisCamerasDependencyChecker.Dependencies
{
    /// <summary>
    /// Class checking the Axis RTP source filter.
    /// </summary>
    internal class AxisRtpSourceFilter : IDependency
    {
        private const string Clsid = "4F1D0C59-5ECC-4028-87F3-482191D2230F";
        private static readonly Version MinVersion = new Version(3, 0, 4, 2);

        private readonly Clsid clsid;

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisRtpSourceFilter"/> class.
        /// </summary>
        public AxisRtpSourceFilter()
        {
            clsid = new Clsid();
        }

        /// <summary>
        /// Runs verification of the dependency.
        /// </summary>
        public DependencyResult Run()
        {
            var result = new DependencyResult("Axis RTP source filter");

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