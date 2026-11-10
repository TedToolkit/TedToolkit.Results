---
name: model-typed-results
description: >-
  Use TedToolkit.Results for expected operation outcomes represented as success values or explicit enum failure reasons.
---

# Model Typed Results

- Choose the `Result` arity that matches whether success carries data. Construct branches with the appropriate `Result.Succeed` or `Result.Fail` factory.
- Use `IsSuccess`, `IsFailure`, or an explicit branch pattern before accessing branch data. Reserve exceptions for failures the API contract does not model as a result.
- Keep application policy in the consumer. Add a dedicated composition, serialization, or HTTP package when the caller needs those capabilities, and test both result branches.
