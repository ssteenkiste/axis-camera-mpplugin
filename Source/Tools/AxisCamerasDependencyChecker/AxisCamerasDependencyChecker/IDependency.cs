namespace AxisCamerasDependencyChecker
{
	/// <summary>
	/// Interface describing a dependency.
	/// </summary>
	internal interface IDependency
	{
		/// <summary>
		/// Runs verification of the dependency.
		/// </summary>
		/// <returns>The result of the dependency check.</returns>
		DependencyResult Run();
	}
}