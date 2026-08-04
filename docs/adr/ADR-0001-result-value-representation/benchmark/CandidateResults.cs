// -----------------------------------------------------------------------
// <copyright file="CandidateResults.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results.Representation.Benchmarks;

internal enum ByteState : byte
{
    Uninitialized,
    Success,
    Failure,
}

internal enum IntState
{
    Uninitialized,
    Success,
    Failure,
}

public readonly record struct ByteStateResult<TValue, TFailure>
    where TFailure : struct, Enum
{
    private readonly TFailure _failureReason;
    private readonly ByteState _state;
    private readonly TValue? _value;

    public ByteStateResult(TValue value)
    {
        _failureReason = default;
        _state = ByteState.Success;
        _value = value;
    }

    public ByteStateResult(TFailure failureReason)
    {
        _failureReason = failureReason;
        _state = ByteState.Failure;
        _value = default;
    }

    public bool IsSuccess => _state switch
    {
        ByteState.Success => true,
        ByteState.Failure => false,
        _ => throw new InvalidOperationException(),
    };

    public TValue Value => _state switch
    {
        ByteState.Success => _value!,
        _ => throw new InvalidOperationException(),
    };

    public TFailure FailureReason => _state switch
    {
        ByteState.Failure => _failureReason,
        _ => throw new InvalidOperationException(),
    };
}

public readonly record struct IntStateResult<TValue, TFailure>
    where TFailure : struct, Enum
{
    private readonly TFailure _failureReason;
    private readonly IntState _state;
    private readonly TValue? _value;

    public IntStateResult(TValue value)
    {
        _failureReason = default;
        _state = IntState.Success;
        _value = value;
    }

    public IntStateResult(TFailure failureReason)
    {
        _failureReason = failureReason;
        _state = IntState.Failure;
        _value = default;
    }

    public bool IsSuccess => _state switch
    {
        IntState.Success => true,
        IntState.Failure => false,
        _ => throw new InvalidOperationException(),
    };

    public TValue Value => _state switch
    {
        IntState.Success => _value!,
        _ => throw new InvalidOperationException(),
    };

    public TFailure FailureReason => _state switch
    {
        IntState.Failure => _failureReason,
        _ => throw new InvalidOperationException(),
    };
}
