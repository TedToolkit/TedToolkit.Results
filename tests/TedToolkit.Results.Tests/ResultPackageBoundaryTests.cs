// -----------------------------------------------------------------------
// <copyright file="ResultPackageBoundaryTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results.Tests;

internal sealed class ResultPackageBoundaryTests
{
    private static readonly string[] _transferableMethodNames =
    [
        "Bind",
        "Deconstruct",
        "Ensure",
        "Map",
        "MapFailure",
        "Match",
        "Recover",
        "RecoverWith",
        "TryGetFailure",
        "TryGetValue",
    ];

    private static readonly string[] _expectedExportedTypes =
    [
        "TedToolkit.Results.Result",
        "TedToolkit.Results.Result`1",
        "TedToolkit.Results.Result`2",
    ];

    /// <summary>
    /// Verifies that only the Result contract types are public in the core assembly.
    /// </summary>
    [Test]
    public async Task Should_expose_only_result_contract_types_from_core_assembly()
    {
        var assembly = typeof(Result<,>).Assembly;
        var exportedTypes = assembly.GetExportedTypes()
            .Select(type => type.FullName!)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

        await Assert.That(exportedTypes).IsEquivalentTo(_expectedExportedTypes);
    }

    /// <summary>
    /// Verifies that transferable behavior is not declared by either core Result type.
    /// </summary>
    [Test]
    public async Task Should_not_declare_transferable_methods_on_core_result_types()
    {
        Type[] resultTypes = [typeof(Result<,>), typeof(Result<>),];
        var declaredTransferableMethods = resultTypes
            .SelectMany(resultType => resultType.GetMethods())
            .Where(method => method.DeclaringType is not null && resultTypes.Contains(method.DeclaringType))
            .Select(method => method.Name)
            .Where(_transferableMethodNames.Contains)
            .Distinct(StringComparer.Ordinal)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

        await Assert.That(declaredTransferableMethods).IsEquivalentTo(Array.Empty<string>());
    }
}