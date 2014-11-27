namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    /// Attribute telling the Visual Studio Code Analyzer that the specified parameter cannot be
    /// null.
    /// </summary>
    internal sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}