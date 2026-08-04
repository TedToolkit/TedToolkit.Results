// -----------------------------------------------------------------------
// <copyright file="MaybeNullWhenAttribute.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

#if NET472 || NET48 || NETSTANDARD2_0
namespace System.Diagnostics.CodeAnalysis;

/// <summary>
/// Indicates that an output may be null when the method returns the specified Boolean value.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class MaybeNullWhenAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MaybeNullWhenAttribute"/> class.
    /// </summary>
    /// <param name="returnValue">The return value associated with a possibly null output.</param>
    internal MaybeNullWhenAttribute(bool returnValue)
    {
        ReturnValue = returnValue;
    }

    /// <summary>
    /// Gets the return value associated with a possibly null output.
    /// </summary>
    internal bool ReturnValue { get; }
}
#endif