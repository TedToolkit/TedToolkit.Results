// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using BenchmarkDotNet.Running;

namespace TedToolkit.Results.Representation.Benchmarks;

internal static class Program
{
    private static void Main(string[] args)
    {
        if (args.Length == 1 && args[0] == "--sizes")
        {
            SizeReport.WriteToConsole();
            return;
        }

        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}
