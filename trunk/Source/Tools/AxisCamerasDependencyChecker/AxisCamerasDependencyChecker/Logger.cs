using System;

namespace AxisCamerasDependencyChecker
{
    /// <summary>
    /// Class logging to <see cref="Console"/>.
    /// </summary>
    internal class Logger
    {
        private readonly ConsoleColor defaultForegroundColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        public Logger()
        {
            defaultForegroundColor = Console.ForegroundColor;
        }

        /// <summary>
        /// Logs the outcome of the dependency result.
        /// </summary>
        internal void Log(DependencyResult result)
        {
            // Change foreground color
            Console.ForegroundColor = result.IsSuccess ?
                ConsoleColor.Green :
                ConsoleColor.Red;

            // Name
            Console.WriteLine(
                "{0} [{1}]",
                result.Name,
                result.IsSuccess ? "OK" : "FAILED");

            // Reset foreground color
            Console.ForegroundColor = defaultForegroundColor;

            // Information
            foreach (var information in result.Information)
            {
                Console.WriteLine("  " + information);
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Pauses the console, and lets the user continue by pressing any key.
        /// </summary>
        internal void Pause()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}