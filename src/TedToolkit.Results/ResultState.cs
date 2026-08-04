// -----------------------------------------------------------------------
// <copyright file="ResultState.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results;

/// <summary>
/// Identifies the active state stored by a Result value.
/// </summary>
internal enum ResultState
{
    /// <summary>
    /// No result branch has been initialized.
    /// </summary>
    UNINITIALIZED = 0,

    /// <summary>
    /// The success branch is active.
    /// </summary>
    SUCCESS = 1,

    /// <summary>
    /// The failure branch is active.
    /// </summary>
    FAILURE = 2,
}