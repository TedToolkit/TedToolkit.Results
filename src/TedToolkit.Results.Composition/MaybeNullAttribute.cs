// -----------------------------------------------------------------------
// <copyright file="MaybeNullAttribute.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

#if NET472 || NET48 || NETSTANDARD2_0
namespace System.Diagnostics.CodeAnalysis;

/// <summary>
/// Indicates that an output may be null even when its static type is non-nullable.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue)]
internal sealed class MaybeNullAttribute : Attribute;
#endif