using System.Collections.Generic;

namespace AxisCamerasDependencyChecker
{
	/// <summary>
	/// The result of the dependency check.
	/// </summary>
	internal class DependencyResult
	{
		private readonly IList<string> information;


		/// <summary>
		/// Initializes a new instance of the <see cref="DependencyResult"/> class.
		/// </summary>
		/// <param name="name">The name of the dependency.</param>
		public DependencyResult(string name)
		{
			information = new List<string>();

			Name = name;
		}


		/// <summary>
		/// Gets the name of the dependency.
		/// </summary>
		internal string Name { get; private set; }


		/// <summary>
		/// Gets or sets a value indicating whether dependency check was successful.
		/// </summary>
		internal bool IsSuccess { get; set; }


		/// <summary>
		/// Gets the additional information about the dependency check.
		/// </summary>
		internal IEnumerable<string> Information
		{
			get { return information; }
		}


		/// <summary>
		/// Adds additional information about the dependency check.
		/// </summary>
		internal void AddInformation(string name, string value)
		{
			string message = string.Format("{0}: {1}", name, value);

			AddInformation(message);
		}


		/// <summary>
		/// Adds additional information about the dependency check.
		/// </summary>
		internal void AddInformation(string message)
		{
			information.Add(message);
		}
	}
}