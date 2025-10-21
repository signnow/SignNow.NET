---
applyTo: '**/*.cs'
description: Guidelines for building C# applications
---

# C# Development

## C# Instructions

- Always use the specified version C#, this project is limited to C# 8.0 features.

## General Instructions

- Make only high confidence suggestions when reviewing code changes.

## Naming Conventions

- Follow PascalCase for component names, method names, and public members.
- Use camelCase for private fields and local variables.
- Prefix interface names with "I" (e.g., IUserService).

## Formatting

- Apply code-formatting style defined in `.editorconfig`.
- Prefer file-scoped namespace declarations and single-line using directives.
- Insert a newline before the opening curly brace of any code block (e.g., after if, for, while, foreach, using, try, etc.).
- Ensure that the final return statement of a method is on its own line.
- Use pattern matching and switch expressions wherever possible.
- Use `nameof` instead of string literals when referring to member names.
- Ensure that XML doc comments are created for any public APIs. When applicable, include `<example>` and `<code>` documentation in the comments.

## Nullable Reference Types

- Declare variables non-nullable, and check for null at entry points.
- Use `default` instead of `null` for method parameters to follow type safety best practices.

## Testing

- Always include test cases for critical paths of the application.
- Guide users through creating unit tests.
- Omit "Act", "Arrange" or "Assert" comments.

## Performance Optimization

- Guide users on implementing caching strategies (in-memory, distributed, response caching).
- Explain asynchronous programming patterns and why they matter for API performance.