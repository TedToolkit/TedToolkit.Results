---
name: compose-typed-results
description: >-
  Use TedToolkit.Results.Composition for branch-safe result matching and synchronous, Task, or collection composition.
---

# Compose Typed Results

- Import `TedToolkit.Results` and use `Match` when converting both branches to a common output.
- Choose the composition overload whose success and failure types match the next operation. Keep an existing failure from reaching a success-only callback.
- Test a successful pipeline and a failed intermediate operation, including the intended short-circuit or aggregation behavior for a collection.
