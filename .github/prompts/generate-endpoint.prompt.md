--
mode: agent
model: Claude Sonnet 4
description: 'Instructions for generating new API endpoints for the SignNow .NET SDK.'
---

# How to implement an Endpoint
Analyze the API endpoint described in the file `endpoint.json` owned in root project directory.
Implement the endpoint following these steps:
1. Create a method in the appropriate API client interface.
2. IMPORTANT: type consistency should be maintained across ALL layers of codebase (public models, internal requests models). Therefore,
 when you design request/response models mentioned in the next steps, take in account the following guidelines:
	 - Use `Uri` type for URL values.
	 - Prefer use domain objects/types instead of primitive types.
	 - Always use existing enum types for finite sets of values or create new ones if necessary.
3. For request body: create internal request class and related public model (options) class.
4. For response body: create a model class.
5. Create a method in the appropriate service interface which uses internal request class and related public model (options) class.
6. Implement the method in the appropriate service class.
7. Cover your implementation with Unit Tests.
8. Cover your implementation with Acceptance Tests.
9. Add example in SignNow.Net/Examples/
10. Build project, pass tests and fix errors when they occur.
11. Generate a changelog entry for your implementation.

Example of command to run specific tests:
`dotnet test --filter "CreateSigningLinkRequestTest" --verbosity minimal --framework net8.0`

# Requirements regarding implementation
1. Make sure you are following the Coding Guidelines in `./github/instructions/signnow.instructions.md`, `./github/instructions/csharp.instructions.md` and `./github/instructions/net-core-fw.instructions.md` during your implementation.
2. Strictly follow the Testing Guidelines in `./github/instructions/signnow.instructions.md` to cover your implementation with tests.
3. Use existing code as an example and implement the new endpoint in the same way.
