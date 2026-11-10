---
name: serialize-typed-results
description: >-
  Use TedToolkit.Results.SystemTextJson to serialize both Result arities with the explicit System.Text.Json union contract.
---

# Serialize Typed Results

- Register the package converter factory on the same `JsonSerializerOptions` used to read and write results.
- Preserve the branch discriminator. Successful value results carry `Value` and failures carry `FailureReason` under the configured naming policy.
- Round-trip each arity and branch, including a success without a value. Verify malformed or contradictory branch payloads against the package’s current converter behavior.
