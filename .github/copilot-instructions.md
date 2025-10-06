# SignNow .NET SDK Development Guide

## Overview & Purpose

This is the official .NET SDK for SignNow's electronic signature API, targeting .NET Framework 4.6.2+, .NET Standard 2.0/2.1, and modern .NET versions. The SDK follows a service-oriented architecture and provides a simple, modern, and efficient way to interact with the SignNow API.

## High-Level Architecture

This SDK is designed as cross-platform library with service-oriented architecture, supporting asynchronous programming patterns and strong typing.

- **Project Type**: Class Library targeting multiple frameworks
- **Architecture**: Service-oriented with clear separation of concerns
- **Distribution**: Available via NuGet package manager
- **Target Frameworks**: Multi-targeted for `.NET Core` and `.NET Framework 4.6.2` with support of standards - .NET Standard 2.0/2.1
- **Key Design Patterns**:
    - **Dependency Injection (DI)**: The SDK is designed to be easily integrated into DI containers with minimal configuration (`SignNowContext` as the main entry point).
    - **Service Layer**: Core services are exposed via interfaces and represented by concrete classes in the `Service` folder.
    - **Model Layer**: Comprehensive model classes represent API requests and responses, ensuring strong typing and JSON serialization.
    - **Infrastructure Layer**: HTTP client infrastructure, including request/response handling, exceptions handling is encapsulated in base classes.
    - **Async/Await**: Full support for asynchronous operations with cancellation tokens.
    - **Token Management**: Built-in support for OAuth2 authentication, including token acquisition and refresh mechanisms.

## Solution & Project Structure

```
├── SignNow.Net/                     // Main SDK library
│   ├── _Internal/
│   │   ├── Constants/               // API endpoint URLs, error messages, and other constants
│   │   ├── Extensions/              // Extension methods for string manipulation, validation, etc.
│   │   ├── Helpers/                 // Utility classes for HTTP requests, serialization, validation
|   |   │   ├── Converters/          // Custom JSON converters for complex types (e.g., Uri, Enums)
│   │   ├── Infrastructure/          // Telemetry and platform-specific implementations
│   │   ├── Model/                   // Internal model classes not exposed publicly
│   │   └── Requests/                // Internal request classes for complex operations
│   ├── Exceptions/                  // Custom exception classes for API errors
│   ├── Interfaces/                  // Public service interfaces
│   ├── Model/                       // All model classes for API requests/responses
│   │   ├── ComplexTags/             // Complex tag types (e.g., SignatureTag, TextTag, etc.)
│   │   ├── EditFields/              // Edit field types (e.g., TextField, CheckboxField, etc.)
│   │   ├── FieldContents/           // Field content types (e.g., TextContent, CheckboxContent, etc.)
│   │   ├── Requests/                // Request model classes
│   │   └── Responses/               // Response model classes
│   ├── Service/                     // Service implementations
│   │   ├── DocumentGroupService.cs  // Document group operations service
│   │   ├── DocumentService.cs       // Document operations service
│   │   ├── EventSubscriptionService.cs // Event subscription service
│   │   ├── FolderService.cs         // Folder management service
│   │   ├── OAuth2Service.cs         // OAuth2 token management service
│   │   ├── SignNowClient.cs         // Core HTTP client implementation
│   │   ├── UserService.cs           // User management service
│   │   └── WebClientBase.cs         // Base class for HTTP clients
│   ├── SignNowContext.cs            // Main entry point and DI container setup
│   └── SignNow.Net.csproj           // Multi-target SDK project file (.NET Framework 4.6.2+, .NET Standard 2.0/2.1)
│
├── SignNow.Net.Examples/            // Usage examples and samples
│   ├── Document group/              // Document group operations examples
│   ├── Documents/                   // Document operations examples
│   ├── Folders/                     // Folder management examples
|   ├── Invites/                     // Event subscription examples
│   ├── OAuth2/                      // OAuth2 authentication examples
│   ├── Template/                    // Template management examples
│   ├── TestExamples/                // Sample documents and templates for testing
│   ├── Users/                       // User management examples
│   ├── Webhooks/                    // Webhook management examples
│   ├── ExamplesBase.cs              // Base class for examples with common setup
│   └── SignNow.Net.Examples.csproj  // Example project file
│
├── SignNow.Net.Test/                // Comprehensive test suite
│   ├── AcceptanceTests/             // End-to-end acceptance tests
│   ├── Constants/                   // Test constants (e.g., error messages, test user IDs)
│   ├── Context/                     // Tests for Credentials loading and SignNowContext setup
│   ├── FeatureTests/                // Feature-specific integration tests
│   ├── TestData/                    // Test fixtures and sample data
│   │   ├── Documents/               // Sample documents for testing         
│   │   └── FakeModels               // Fake model classes for generating test data
│   ├── UnitTests/                   // Unit tests with no external dependencies
│   ├── AssertExtensions.cs          // Custom assertion extensions for tests
│   ├── AuthorizedApiTestBase.cs     // Base class for tests requiring authorized API access
│   ├── Directory.Build.props        // Shared MSBuild properties for tests
│   ├── SignNowTestBase.cs           // Base class for all tests with common setup/teardown
│   ├── SignNow.Net.Test.csproj      // Test project targeting multiple frameworks
│   └── TestUtils.cs                 // Utility methods for tests (e.g., JSON serialization)
│
├── .editorconfig                    // Code formatting and style rules
├── .netconfig                       // .NET runtime configuration (ReportGenerator configuration for codecoverage)
├── CHANGELOG.md                     // SDK change log and release notes
├── Directory.Build.props            // Shared MSBuild properties and versioning
├── netfx.props                      // .NET Framework specific build properties for multi-targeting and cross-platform support
├── SignNow.Net.sln                  // Main solution file
├── SignNow.props                    // Shared build properties for multi-targeting, versioning, and packaging (Nuget config)
└── README.md                        // SDK documentation and getting started guide
```

## Core Concepts & Implementation Details

### Public API Surface

- The public API is primarily defined by interfaces located in the `SignNow.Net/Interfaces` folder.
- Consumers of the SDK should only need to depend on these interfaces and the models they expose.
- All implementation classes with complex request transformations should be marked as `internal` and placed into `SignNow.Net/_Internal` folder.
- All the public responses and request models are located in the `SignNow.Net/Model` folder.
- All the entities of the SignNow API should be represented by strongly typed models. Models should be groupped by feature areas (e.g., `Document`, `User`, `Folder`, etc.) and further subdivided into `Requests` and `Responses` folders.
- SignNow API entities could be groupped for re-usage purposes (e.g., `ComplexTags`, `EditFields`, `FieldContents`). The groups should be placed into separate folders under `Model` folder and represent single SignNow API entity.
- Avoid exposing implementation details, such as HTTP client classes or internal helpers, in the public API.
- For dynamic API responses, use internal Json converters to handle deserialization without exposing complexity to consumers.

### Dependency Injection

- The SDK provides a single entry point - `SignNowContext` with minimalistic parameters for configuration, like API base URL, Token (if any exists) and HTTP client.
- This `SignNowContext` class registers all necessary services with a single context.
- For User Authentication - use `SignNowContext.SetAppCredentials()` and `SignNowContext.GetAccessToken()` methods to configure application and user credentials respectively.
- For advanced scenarious or re-usage of existing HTTP clients, the SDK provides an ability to set custom `ISignNowClient` implementation via `SignNowContext` constructor.
- Application secrets and user credentials are not stored in the SDK, and should be managed by the consumer application.

### Platform-Specific Code

- The SDK targets multiple frameworks: .NET Framework 4.6.2, .NET Standard 2.0/2.1, and modern .NET versions (e.g., .NET 7.0/8.0).
- To compile .NET SDK 2.x projects targeting .NET 4.x on Mono (Linux, macOS), we use `netfx.props` file with specific build properties for MSBuild system.
- We use preprocessor directives (`#if NETSTANDARD`, `#if NET462`, `#if NETFRAMEWORK`) to compile code conditionally for each target.
- Platform-specific and Framework-specific code is most commonly used for networking (e.g., `HttpClient` configuration) or file system differences.
- All the business logic should be platform-agnostic and reusable across all target frameworks.
- We use `System.Net.ServicePointManager` to enforce TLS 1.2 on .NET Framework 4.6.2, as it defaults to older protocols.

### Error Handling

- Exception handling should be consistent across platforms, with custom exceptions defined in the `Exceptions` folder.
- The SDK throws own custom exceptions `SignNowException` with all the inner Exceptions and their details.
- The SDK throws `ArgumentException`, `ArgumentNullException` for invalid method arguments or state.
- Consumers should `try-catch` these specific exceptions to handle SDK-related errors gracefully.
- Avoid throwing generic exceptions like `System.Exception` or specific for json conversion exception `JsonSerializationException`. All the exceptions should be wrapped into `SignNowException` with all the inner exceptions preserved.

## Coding & Contribution Guidelines

- **Style**: Follow the standard .NET coding conventions. Use the `.editorconfig` file provided in the repository.
- **Testing**: Any new feature or bug fix must be accompanied by corresponding unit tests.
- **Documentation**: All public types, methods, and properties must have XML documentation comments (`///`).
- **Dependencies**: Minimize external dependencies. Before adding a new NuGet package, discuss it with the team.
- **Change Log**: Update `CHANGELOG.md` with a summary of changes for each release and pull request.
