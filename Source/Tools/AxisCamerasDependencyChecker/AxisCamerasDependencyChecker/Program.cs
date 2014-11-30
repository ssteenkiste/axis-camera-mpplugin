using AxisCamerasDependencyChecker.Dependencies;

namespace AxisCamerasDependencyChecker
{
    internal class Program
    {
        private static readonly Logger Logger = new Logger();

        /// <summary>
        /// Entry method for console application.
        /// </summary>
        private static void Main()
        {
            // Check RTP source filter
            IDependency axisRtpSourceFilter = new AxisRtpSourceFilter();
            Logger.Log(axisRtpSourceFilter.Run());

            // Check embedded source filter
            IDependency embeddedAxisRtpSourceFilter = new EmbeddedAxisRtpSourceFilter();
            Logger.Log(embeddedAxisRtpSourceFilter.Run());

            Logger.Pause();
        }
    }
}