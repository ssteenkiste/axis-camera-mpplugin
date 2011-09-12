using AxisCamerasDependencyChecker.Dependencies;

namespace AxisCamerasDependencyChecker
{
	internal class Program
	{
		private static readonly Printer Printer = new Printer();


		/// <summary>
		/// Entry method for console application.
		/// </summary>
		private static void Main()
		{
			// Check RTP source filter
			IDependency axisRtpSourceFilter = new AxisRtpSourceFilter();
			Printer.Print(axisRtpSourceFilter.Run());

			// Check embedded source filter
			IDependency embeddedAxisRtpSourceFilter = new EmbeddedAxisRtpSourceFilter();
			Printer.Print(embeddedAxisRtpSourceFilter.Run());

			Printer.Pause();
		}
	}
}