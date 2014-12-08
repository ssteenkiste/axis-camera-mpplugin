using System;
using System.Runtime;

// ReSharper disable once CheckNamespace
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

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Allows you to obtain the method or property name of the caller to the method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    [__DynamicallyInvokable]
    public sealed class CallerMemberNameAttribute : Attribute
    {
        [__DynamicallyInvokable]
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public CallerMemberNameAttribute()
        {
        }
    }

    /// <summary>
    /// Allows you to obtain the full path of the source file that contains the caller. This is the
    /// file path at the time of compile.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    [__DynamicallyInvokable]
    public sealed class CallerFilePathAttribute : Attribute
    {
        [__DynamicallyInvokable]
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public CallerFilePathAttribute()
        {
        }
    }

    /// <summary>
    /// Allows you to obtain the line number in the source file at which the method is called.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    [__DynamicallyInvokable]
    public sealed class CallerLineNumberAttribute : Attribute
    {
        [__DynamicallyInvokable]
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public CallerLineNumberAttribute()
        {
        }
    }
}

internal sealed class __DynamicallyInvokableAttribute : Attribute
{
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public __DynamicallyInvokableAttribute()
    {
    }
}