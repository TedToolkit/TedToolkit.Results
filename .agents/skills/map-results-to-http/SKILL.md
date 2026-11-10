---
name: map-results-to-http
description: >-
  Use TedToolkit.Results.AspNetCore to map typed results to MVC or minimal API responses with application-owned failure policy.
---

# Map Results to HTTP

- Use the MVC or minimal API adapter matching the endpoint shape. With MVC, `ToActionResult` maps failure reasons through a caller-supplied function.
- Define the response for each failure reason explicitly. Keep service failure enums free of framework status codes.
- Test the status and response body for a successful result and each externally visible failure mapping.
